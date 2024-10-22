using KoiMuseum.Common;
using KoiMuseum.Data.Models;
using KoiMuseum.Service;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using NetCoreDemo.Types;
namespace KoiMuseum.APIService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        public OrderController(PayOS payOS, IPaymentService paymentService)
        {
            _payOS = payOS;
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            try
            {
                // Generate an order code
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

                // Create item data
                ItemData item = new ItemData(body.productName, 1, body.price);
                List<ItemData> items = new List<ItemData> { item };

                // Create payment data for external payment service (e.g., PayOS)



                // Assuming you got a transaction ID from createPayment (e.g., createPayment.transactionId)
                string transactionId = orderCode.ToString();

                // Create and populate Payment entity to store in the database
                Payment payment = new Payment
                {
                    UserId = 1, // replace this with the actual user ID, if available
                    Amount = body.price,
                    PaymentMethod = "PayOS", // Specify your payment method here
                    PaymentStatus = Const.PENDING_STATUS, // Initial status
                    TransactionId = transactionId,
                    Currency = "USD", // Specify the currency here
                    Description = body.description,
                    CreatedAt = DateTime.UtcNow
                };

                // Save the payment to the database using the payment service
                var result = await _paymentService.Save(payment);

                PaymentData paymentData = new PaymentData(orderCode, body.price, body.description, items, "https://localhost:7028/Order/handle-status/" + orderCode, "https://localhost:7028/Order/handle-status/" + orderCode);

                // Call the external service to create a payment link
                var createPayment = await _payOS.createPaymentLink(paymentData);
                if (result.Status == 1)
                {
                    return Ok(new Response(0, "Success", createPayment));
                }
                else
                {
                    return Ok(new Response(-1, "Payment Save Failed", null));
                }
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }


        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpGet("handle-status/{orderId}")]
        public async Task<IActionResult> HandleStatus([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                var paymentResult = await _paymentService.GetByTransactionId(paymentLinkInformation.orderCode.ToString());
                Payment paymentData = (Payment)paymentResult.Data;
                // thanh toán thành công,trả về trang gì đó, cái này để fix lỗi double call của payos
                if (paymentLinkInformation.status.Equals(Const.PAID_STATUS) && paymentData.PaymentStatus.Equals(Const.PAID_STATUS))
                {

                }
                // thanh toán thành công, sử lý status của registration tại đây có thể trả về trang gì đó
                if (paymentLinkInformation.status.Equals(Const.PAID_STATUS) && !paymentData.PaymentStatus.Equals(Const.PAID_STATUS))
                {
                    _paymentService.UpdatePaymentStatus(paymentData.Id, Const.PAID_STATUS);
                }
                else
                {
                    _paymentService.UpdatePaymentStatus(paymentData.Id, Const.CANCEL_STATUS);

                }
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _payOS.confirmWebhook(body.webhook_url);
                return Ok(new Response(0, "Ok", null));
            }
            catch (System.Exception exception)
            {

                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }

        }
    }
}