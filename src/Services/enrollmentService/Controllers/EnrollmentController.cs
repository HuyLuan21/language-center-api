using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using enrollmentService.Services;
using enrollmentService.DTOs;

namespace enrollmentService.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;
        public EnrollmentController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpGet("student/{studentId}")]
        public ActionResult<GetStudentEnrollmentResponse> GetEnrollmentsByStudentId(Guid studentId)
        {
            try
            {
                var response = _enrollmentService.GetStudentEnrollment(studentId);
                return Ok(response);
            }
            catch
            {
                return NotFound(new { Message = "No enrollment found for the given student ID." });
            }
        }

        [HttpGet("{enrollmentId}/detail")]
        public ActionResult<EnrollmentDetailDTO> GetEnrollmentDetail(Guid enrollmentId)
        {
            try
            {
                var response = _enrollmentService.GetEnrollmentDetail(enrollmentId);
                return Ok(response);
            }
            catch
            {
                return NotFound(new { Message = "No enrollment detail found for the given enrollment ID." });
            }
        }

        [HttpPost]
        public ActionResult CreateEnrollment([FromBody] EnrollmentDTO enrollment)
        {
            try
            {
                bool isCreated = _enrollmentService.CreateEnrollment(enrollment);
                if (isCreated)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return BadRequest(new { Message = "Failed to create enrollment." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPatch("{enrollmentId}/status")]
        public ActionResult UpdateEnrollmentStatus(Guid enrollmentId, [FromBody] string newStatus)
        {
            try
            {
                bool isUpdated = _enrollmentService.UpdateEnrollmentStatus(enrollmentId, newStatus);
                if (isUpdated)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { Message = "Failed to update enrollment status." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpDelete("{enrollmentId}")]
        public ActionResult DeleteEnrollment(Guid enrollmentId)
        {
            try
            {
                bool isDeleted = _enrollmentService.DeleteEnrollment(enrollmentId);
                if (isDeleted)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { Message = "Failed to delete enrollment." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("courses")]
        public ActionResult<List<CourseDTO>> GetAllCourses()
        {
            try
            {
                var courses = _enrollmentService.GetCourseList();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
