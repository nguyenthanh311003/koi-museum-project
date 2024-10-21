using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;

namespace KoiMuseum.Service
{
    public interface IPaymentService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> Save(Payment payment);
        Task<IServiceResult> DeleteById(int id);
    }

    public class PaymentService : IPaymentService
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> GetAll()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();

            if (payments == null || payments.Count == 0)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, payments);
            }
        }

        public async Task<IServiceResult> GetById(int id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);

            if (payment == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, payment);
            }
        }

        public async Task<IServiceResult> Save(Payment payment)
        {
            try
            {
                int result = -1;

                if (payment != null && payment.Id <= 0)
                {
                    result = await _unitOfWork.PaymentRepository.CreateAsync(payment);
                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, payment);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
                else
                {
                    result = await _unitOfWork.PaymentRepository.UpdateAsync(payment);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, payment);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, payment);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> DeleteById(int id)
        {
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);

                if (payment == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Payment());
                }
                else
                {
                    bool isDeleted = await _unitOfWork.PaymentRepository.RemoveAsync(payment);
                    if (!isDeleted)
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, payment);
                    }
                    return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, payment);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
