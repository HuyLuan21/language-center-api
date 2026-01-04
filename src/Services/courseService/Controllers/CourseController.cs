using Microsoft.AspNetCore.Mvc;
using courseService.Services;
//using courseService.DTOs.Auth;
using courseService.DTOs;

namespace courseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(CourseService courseService) : ControllerBase
    {
        private readonly CourseService _courseService = courseService;
        [HttpGet("courses")]
        public ActionResult<ApiResponse<List<CourseResponse>, object?>> GetAllCourses()

        {
            try
            {
                var courses = _courseService.GetAllCourses();
                return Ok(new ApiResponse<List<CourseResponse>, object?>(
                    true,
                    "Lấy danh sách khóa học thành công",
                    courses,
                    null
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<CourseResponse>, object?>(
                    false,
                    ex.Message,
                    null,
                    null
                ));
            }
        }

    }
}
