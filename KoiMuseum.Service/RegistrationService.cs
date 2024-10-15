using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Responses.Registration;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Service.Base;
namespace KoiMuseum.Service
{
    public interface IRegistrationService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetAllV2();
        Task<IServiceResult> SearchSortCombineDataRegistrationAndRegisterDetail(SearchRegistrationFilter searchRegistrationFilter);
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> CountContestantsParticipating(string rankId); 

        /// <summary>
        /// Retrieves a registration by its ID, including related entities such as `RegisterDetail.Owner` and `Contest`.
        /// Maps the retrieved registration data to a `RegistrationResponse` DTO to return relevant information.
        /// </summary>
        /// <param name="id">The ID of the registration to fetch.</param>
        /// <returns>
        /// Returns an `IServiceResult` containing the `RegistrationResponse` DTO if found, or an appropriate warning message if no data is found.
        /// </returns>
        Task<IServiceResult> GetByIdV2(int id);

        Task<IServiceResult> Save(Registration registration);
        Task<IServiceResult> ChangeStatus(int id, string status);
        Task<IServiceResult> DeleteById(int id);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly UnitOfWork _unitOfWork;

        public RegistrationService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        /// <summary>
        /// Changes the status of a registration by its ID to the specified status.
        /// </summary>
        /// <param name="id">The ID of the registration whose status needs to be changed.</param>
        /// <param name="status">The new status to apply to the registration.</param>
        /// <returns>
        /// Returns an `IServiceResult` indicating the success or failure of the status change.
        /// </returns>
        public async Task<IServiceResult> ChangeStatus(int id, string status)
        {
            // Fetch the registration by ID
            var registration = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);

            // Check if the registration exists
            if (registration == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            // Validate the new status
            var validStatuses = new List<string> { "CANCEL", "APPROVE", "REJECT" };
            if (!validStatuses.Contains(status))
            {
                return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, "Invalid status.");
            }

            // Check if the registration is already in the desired status
            if (registration.Status == status)
            {
                return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, $"Registration is already in {status} status.");
            }


            // Update the registration's status to the new status
            registration.Status = status;
            registration.UpdatedDate = DateTime.Now;

            // Save the updated registration to the database
            await _unitOfWork.RegistrationRepository.UpdateAsync(registration);

            // Return success response with the updated registration details
            return new ServiceResult(Const.SUCCESS_UPDATE_CODE, $"Status changed to {status} successfully.", registration);
        }

        public async Task<IServiceResult> CountContestantsParticipating(string rankName)
        {
            try
            {
                var countContestantsParticipating = await _unitOfWork.RegistrationRepository.CountContestantsParticipatingByRankName(rankName);

                if (countContestantsParticipating == 0)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, countContestantsParticipating);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, countContestantsParticipating);
            } catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
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

        public async Task<IServiceResult> GetById(int id)
        {
            var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
            return registrationById == null
                ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registrationById);
        }
        /// <summary>
        /// Retrieves a registration by its ID, including related entities such as `RegisterDetail.Owner` and `Contest`.
        /// Maps the retrieved registration data to a `RegistrationResponse` DTO to return relevant information.
        /// </summary>
        /// <param name="id">The ID of the registration to fetch.</param>
        /// <returns>
        /// Returns an `IServiceResult` containing the `RegistrationResponse` DTO if found, or an appropriate warning message if no data is found.
        /// </returns>
        public async Task<IServiceResult> GetByIdV2(int id)
        {
            // Fetch registration by ID with related entities
            var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id, "RegisterDetail.Owner", "Contest");

            if (registrationById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            // Map registration to RegistrationResponse
            var response = new RegistrationResponse
            {
                Id = registrationById.Id,
                ImageUrl = null,  // Assuming no image URL provided
                Size = registrationById.RegisterDetail?.Size,
                Age = registrationById.RegisterDetail?.Age,
                OwnerName = registrationById.RegisterDetail?.Owner?.Name,  // Check if Owner exists
                Rank = registrationById.RegisterDetail?.Rank?.Name,  // Check if Rank exists
                ContestName = registrationById.Contest?.Name,  // Check if Contest exists
                RegistrationDate = registrationById.CreatedDate,  // Corrected to use RegistrationDate
                ApprovalDate = registrationById.ApprovalDate,
                RejectedReason = registrationById.RejectedReason,
                ConfirmationCode = registrationById.ConfirmationCode,
                IntroductionOfOwner = registrationById.IntroductionOfOwner,
                IntroductionOfKoi = registrationById.IntroductionOfKoi,
                Status = registrationById.Status,
                AdminReviewedBy = registrationById.AdminReviewedBy,
                UpdatedDate = registrationById.RegisterDetail?.UpdatedDate,
                UpdatedBy = registrationById.RegisterDetail?.UpdatedBy
            };
            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
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

        public async Task<IServiceResult> SearchSortCombineDataRegistrationAndRegisterDetail(SearchRegistrationFilter searchRegistrationFilter)
        {
            try
            {
                // Lấy các giá trị phân trang từ searchRegistrationFilter
                int pageNumber = searchRegistrationFilter.PageNumber;
                int pageSize = 10;

                // Gọi phương thức repository để tìm kiếm và phân trang
                var pagedResult = await _unitOfWork.RegistrationRepository.SearchRegistrationsPagedAsync(searchRegistrationFilter, pageNumber, pageSize);

                if (pagedResult == null || !pagedResult.Items.Any())
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                // Tạo danh sách kết quả
                var response = pagedResult.Items.Select(r => new RegistrationResponse
                {
                    Id = r.Id,
                    ImageUrl = null,
                    Size = r.RegisterDetail?.Size,
                    Age = r.RegisterDetail?.Age,
                    OwnerName = r.RegisterDetail?.Owner?.Name,
                    Name = r.RegisterDetail?.Name,
                    Type = r.RegisterDetail?.Type,
                    Rank = r.RegisterDetail?.Rank?.Name,
                    ContestName = r.Contest?.Name,
                    RegistrationDate = r.CreatedDate,
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

                // Trả về kết quả với thông tin phân trang
                var pagedResponse = new PagedResult<RegistrationResponse>
                {
                    Items = response,
                    TotalItems = pagedResult.TotalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagedResponse);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

    }
}
