using Microsoft.AspNetCore.Mvc;
using paymentService.Services;
using paymentService.DTOs;

namespace paymentService.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public PaymentController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("history/{studentId}")]
        public IActionResult GetPaymentHistory(Guid studentId)
        {
            try
            {
                var response = _invoiceService.GetPaymentHistory(studentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving payment history", error = ex.Message });
            }
        }

        [HttpPost("verify")]
        public IActionResult VerifyPayment([FromBody] VerifyPaymentRequest request)
        {
            try
            {
                var response = _invoiceService.VerifyPayment(request);
                
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while verifying payment", error = ex.Message });
            }
        }
    }
}
