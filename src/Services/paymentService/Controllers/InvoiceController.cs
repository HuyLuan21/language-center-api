using Microsoft.AspNetCore.Mvc;
using paymentService.Services;

namespace paymentService.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("student/{studentId}")]
        public IActionResult GetInvoicesByStudentId(Guid studentId)
        {
            try
            {
                var response = _invoiceService.GetInvoicesByStudentId(studentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving invoices", error = ex.Message });
            }
        }
    }
}
