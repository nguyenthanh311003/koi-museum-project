﻿using KoiMuseum.Common;
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

            var response = registrations.Select(r => new RegistrationResponse
            {
                Id = r.Id,
                ImageUrl = null,
                Size = r.RegisterDetail?.Size,
                Age = r.RegisterDetail?.Age,
                OwnerName = r.RegisterDetail?.Owner?.Name,
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

            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, response);
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

        public async Task<IServiceResult> ChangeStatus(int id, string status)
        {
            var registration = await _unitOfWork.RegistrationRepository.GetByIdAsync(id);
            if (registration == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            var validStatuses = new List<string> { "CANCEL", "APPROVE", "REJECT", "CHECKIN" };
            if (!validStatuses.Contains(status))
            {
                return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, "Invalid status.");
            }

            if (registration.Status == status)
            {
                return new ServiceResult(Const.WARNING_STATUS_CHANGE_CODE, $"Registration is already in {status} status.");
            }

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

                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, registrationById);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
