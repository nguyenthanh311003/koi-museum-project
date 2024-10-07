using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Responses.Registration;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;
namespace KoiMuseum.Service
{
    public interface IRegistrationService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetAllV2();
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> Save(Registration registration);
        Task<IServiceResult> DeleteById(int id);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly UnitOfWork _unitOfWork;

        public RegistrationService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> DeleteById(int id)
        {
            try
            {
                var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
                if (registrationById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Registration());
                }

                bool isDelete = await _unitOfWork.RegistrationRepository.RemoveAsync(registrationById);
                if (!isDelete)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, registrationById);
                }

                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, registrationById);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> GetAll()
        {
            var registrations = await _unitOfWork.RegistrationRepository.GetAllAsync();
            return registrations == null
                ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registrations);
        }
        public async Task<IServiceResult> GetAllV2()
        {
            // Get all registrations and include necessary related entities
            var registrations = await _unitOfWork.RegistrationRepository.GetAllAsync("RegisterDetail.Owner", "Contest");

            if (registrations == null || !registrations.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            // Project the result to include OwnerName instead of OwnerId
            var response = registrations.Select(r => new RegistrationResponse
            {
                Id = r.Id,
                ImageUrl = null,  // Assuming no image URL provided
                Size = r.RegisterDetail?.Size,
                Age = r.RegisterDetail?.Age,
                ColorPattern = r.RegisterDetail?.ColorPattern,
                OwnerName = r.RegisterDetail?.Owner?.Name,  // Check if Owner and Name exist
                Rank = r.RegisterDetail?.Rank?.Name,  // Check if Rank exists
                ContestName = r.Contest?.Name,  // Check if Contest exists
                RegistrationDate = r.CreatedDate,  // Corrected to use RegistrationDate
                ApprovalDate = r.ApprovalDate,
                RejectedReason = r.RejectedReason,
                ConfirmationCode = r.ConfirmationCode,
                IntroductionOfOwner = r.IntroductionOfOwner,
                IntroductionOfKoi = r.IntroductionOfKoi,
                Status = r.Status,
                AdminReviewedBy = r.AdminReviewedBy,
                UpdatedDate = r.RegisterDetail?.UpdatedDate,
                UpdatedBy = r.RegisterDetail?.UpdatedBy
            }).ToList();

            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
        }

        public async Task<List<RegistrationResponse>> SearchRegistrations(string ownerName, string contestName, string status, string rank, string colorPattern, DateOnly? approvalDate, string confirmationCode)
        {
            using (var httpClient = new HttpClient())
            {
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(ownerName)) queryParams.Add($"ownerName={ownerName}");
                if (!string.IsNullOrEmpty(contestName)) queryParams.Add($"contestName={contestName}");
                if (!string.IsNullOrEmpty(status)) queryParams.Add($"status={status}");
                if (!string.IsNullOrEmpty(rank)) queryParams.Add($"rank={rank}");
                if (!string.IsNullOrEmpty(colorPattern)) queryParams.Add($"colorPattern={colorPattern}");
                if (approvalDate.HasValue) queryParams.Add($"approvalDate={approvalDate.Value}");
                if (!string.IsNullOrEmpty(confirmationCode)) queryParams.Add($"confirmationCode={confirmationCode}");

                var queryString = string.Join("&", queryParams);
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Registrations/search" + (queryParams.Count > 0 ? "?" + queryString : ""));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        return JsonConvert.DeserializeObject<List<RegistrationResponse>>(result.Data.ToString());
                    }
                }
            }

            return new List<RegistrationResponse>();
        }


        public async Task<IServiceResult> GetById(int id)
        {
            var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
            return registrationById == null
                ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registrationById);
        }

        public async Task<IServiceResult> Save(Registration registration)
        {
            try
            {
                int result = -1;

                if (registration != null && registration.Id <= 0)
                {
                    result = await _unitOfWork.RegistrationRepository.CreateAsync(registration);
                    return result > 0
                        ? new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, registration)
                        : new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                else
                {
                    result = await _unitOfWork.RegistrationRepository.UpdateAsync(registration);
                    return result > 0
                        ? new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, registration)
                        : new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, registration);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
