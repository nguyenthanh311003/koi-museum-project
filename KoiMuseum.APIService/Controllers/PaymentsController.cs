using KoiMuseum.Data.Models;
using KoiMuseum.Service;
using KoiMuseum.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiMuseum.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IServiceResult> GetPayments()
        {
            return await _paymentService.GetAll();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<IServiceResult> GetPayment(int id)
        {
            var payment = await _paymentService.GetById(id);
            return payment;
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IServiceResult> PostPayment(Payment payment)
        {
            return await _paymentService.Save(payment);
        }

        // PUT: api/Payments
        [HttpPut]
        public async Task<IServiceResult> PutPayment(Payment payment)
        {
            return await _paymentService.Save(payment);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IServiceResult> DeletePayment(int id)
        {
            return await _paymentService.DeleteById(id);
        }
    }
}
