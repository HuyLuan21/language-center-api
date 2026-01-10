using Microsoft.AspNetCore.Mvc;
using certificateService.Services;

namespace certificateService.Controllers
{
    [ApiController]
    [Route("api/certificates")]
    public class CertificateController : ControllerBase
    {
        private readonly CertificateService _certificateService;

        public CertificateController(CertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet("student/{studentId}")]
        public IActionResult GetCertificatesByStudentId(Guid studentId)
        {
            try
            {
                var response = _certificateService.GetCertificatesByStudentId(studentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving certificates", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllCertificates()
        {
            try
            {
                var response = _certificateService.GetAllCertificates();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving certificates", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCertificateById(Guid id)
        {
            try
            {
                var response = _certificateService.GetCertificateById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
