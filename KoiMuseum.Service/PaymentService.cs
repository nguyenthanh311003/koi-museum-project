using KoiMuseum.Service.Base;

namespace KoiMuseum.Service
{
    public interface IPaymentService
    {
        Task<IServiceResult> GetAllByUserId();
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> GetAll();
    }
    public class PaymentService
    {
    }
}
