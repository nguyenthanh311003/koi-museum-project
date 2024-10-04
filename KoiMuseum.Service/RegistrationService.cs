using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;

namespace KoiMuseum.Service
{
    public interface IRegistrationService
    {
        Task<IServiceResult> GetAll();
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
