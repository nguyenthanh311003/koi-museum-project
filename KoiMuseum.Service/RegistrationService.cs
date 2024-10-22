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
        Task<IServiceResult> GetAllV2(int page, int limit);
        Task<IServiceResult> SearchSortCombineDataRegistrationAndRegisterDetail(SearchRegistrationFilter searchRegistrationFilter);
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> GetByIdWithCombineRegisterDetailResponse(int id);
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
        Task<IServiceResult> ChangeStatus(int id, string status, string? confirmCode, string? reasonReject);
        Task<IServiceResult> DeleteById(int id);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly UnitOfWork _unitOfWork;

        public RegistrationService()
        {
            _unitOfWork ??= new UnitOfWork();
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
        public async Task<IServiceResult> GetAllV2(int pageNumber = 1, int pageSize = 10)
        {
            // Calculate the number of items to skip based on the current page number
            int skip = (pageNumber - 1) * pageSize;

            // Get all registrations, including necessary related entities
            var registrations = await _unitOfWork.RegistrationRepository
                .GetAllAsync("RegisterDetail.Owner", "Contest");

            if (registrations == null || !registrations.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            // Get the total count of registrations
            int totalItems = registrations.Count();

            // Apply pagination by skipping and taking the specified number of items
            var pagedRegistrations = registrations
                .Skip(skip)
                .Take(pageSize)
                .Select(r => new RegistrationResponse
                {
                    Id = r.Id,
                    ImageUrl = null,
                    Size = r.RegisterDetail?.Size,
                    Age = r.RegisterDetail?.Age,
                    OwnerName = r.RegisterDetail?.Owner?.Name,
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
                })
                .ToList();

            // Create the paginated result
            var pagedResult = new PagedResult<RegistrationResponse>
            {
                Items = pagedRegistrations,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            // Return the paginated result as a successful service result
            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagedResult);
        }

        public async Task<IServiceResult> GetById(int id)
        {
            var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
            return registrationById == null
                ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registrationById);
        }

        public async Task<IServiceResult> GetByIdV2(int id)
        {
            var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id, "RegisterDetail.Owner", "Contest");

            if (registrationById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            var response = new RegistrationResponse
            {
                Id = registrationById.Id,
                ImageUrl = null,
                Size = registrationById.RegisterDetail?.Size,
                Age = registrationById.RegisterDetail?.Age,
                OwnerName = registrationById.RegisterDetail?.Owner?.Name,
                Rank = registrationById.RegisterDetail?.Rank?.Name,
                ContestName = registrationById.Contest?.Name,
                RegistrationDate = registrationById.CreatedDate,
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

        public async Task<IServiceResult> GetByIdWithCombineRegisterDetailResponse(int id)
        {
            try
            {
                var registerGetById = await _unitOfWork.RegistrationRepository.findById(id);

                if (registerGetById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                // Tạo danh sách kết quả
                var response = new RegistrationResponse
                {
                    Id = registerGetById.Id,
                    ImageUrl = null,
                    Weight = registerGetById.RegisterDetail?.Weight,
                    Gender = registerGetById.RegisterDetail?.Gender,
                    Size = registerGetById.RegisterDetail?.Size,
                    Age = registerGetById.RegisterDetail?.Age,
                    OwnerName = registerGetById.RegisterDetail?.Owner?.Name,
                    Name = registerGetById.RegisterDetail?.Name,
                    Type = registerGetById.RegisterDetail?.Type,
                    Rank = registerGetById.RegisterDetail?.Rank?.Name,
                    ContestName = registerGetById.Contest?.Name,
                    RegistrationDate = registerGetById.CreatedDate,
                    ApprovalDate = registerGetById.ApprovalDate,
                    RejectedReason = registerGetById.RejectedReason,
                    ConfirmationCode = registerGetById.ConfirmationCode,
                    IntroductionOfOwner = registerGetById.IntroductionOfOwner,
                    IntroductionOfKoi = registerGetById.IntroductionOfKoi,
                    Status = registerGetById.Status,
                    AdminReviewedBy = registerGetById.AdminReviewedBy,
                    UpdatedDate = registerGetById.RegisterDetail?.UpdatedDate,
                    UpdatedBy = registerGetById.RegisterDetail?.UpdatedBy
                };

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> Save(Registration registration)
        {
            try
            {
                int result = registration != null && registration.Id <= 0
                    ? await _unitOfWork.RegistrationRepository.CreateAsync(registration)
                    : await _unitOfWork.RegistrationRepository.UpdateAsync(registration);

                return result > 0
                    ? new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, registration)
                    : new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, registration);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> ChangeStatus(int id, string status, string? confirmCode, string? reasonReject)
        {
            // Retrieve the registration record
            var registration = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
            if (registration == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            // Validate if the status provided is one of the allowed statuses
            var validStatuses = new List<string> { "CANCEL", "APPROVE", "REJECT", "CHECKIN" };
            if (!validStatuses.Contains(status))
            {
                return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, "Invalid status.");
            }

            // Check for CHECKIN status and confirm the code
            if (status.Equals("CHECKIN"))
            {
                if (!registration.ConfirmationCode.Equals(confirmCode))
                {
                    return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, $"Confirm Code did not match.");
                }
            }

            // Check for REJECT status and update the reason for rejection
            if (status.Equals("REJECT"))
            {
                if (string.IsNullOrEmpty(reasonReject))
                {
                    return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, "Reason for rejection cannot be empty.");
                }

                registration.RejectedReason = reasonReject;  // Set the rejection reason
            }

            // Check if the status is already the same
            if (registration.Status == status)
            {
                return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, $"Registration is already in {status} status.");
            }

            // Update the status and last updated date
            registration.Status = status;
            registration.UpdatedDate = DateTime.Now;
            await _unitOfWork.RegistrationRepository.UpdateAsync(registration);

            return new ServiceResult(Const.SUCCESS_UPDATE_CODE, $"Status changed to {status} successfully.", registration);
        }


        public async Task<IServiceResult> DeleteById(int id)
        {
            try
            {
                var registrationById = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
                if (registrationById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                bool isDelete = await _unitOfWork.RegistrationRepository.RemoveAsync(registrationById);
                if (!isDelete)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
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
                int pageSize = searchRegistrationFilter.PageSize;

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
