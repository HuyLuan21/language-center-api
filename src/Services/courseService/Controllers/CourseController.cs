using Microsoft.AspNetCore.Mvc;
using courseService.Services;
//using courseService.DTOs.Auth;
using courseService.DTOs;
using courseService.Models;

namespace courseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(CourseService courseService) : ControllerBase
    {
        private readonly CourseService _courseService = courseService;
        [HttpGet]
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
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CourseResponse, object?>> GetCourseById(Guid id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound(new ApiResponse<CourseResponse, object?>(
                    false,
                    "Không tìm thấy khóa học",
                    null,
                    null
                ));
            }

            return Ok(new ApiResponse<CourseResponse, object?>(
                true,
                "Lấy khóa học thành công",
                course,
                null
            ));
        }
        [HttpPost]
        public ActionResult<ApiResponse<CourseResponse, object?>> CreateCourse([FromBody] CourseCreateRequest request)
        {
            try
            {
                var createdCourse = _courseService.CreateCourse(request);
                return Ok(new ApiResponse<CourseResponse, object?>(
                    true,
                    "Tạo khóa học thành công",
                    createdCourse,
                    null
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CourseResponse, object?>(
                    false,
                    ex.Message,
                    null,
                    null
                ));
            }
        }
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<CourseResponse, object?>> UpdateCourse(
    Guid id,
    [FromBody] CourseUpdateRequest request
)
        {
            try
            {
                var updatedCourse = _courseService.UpdateCourse(id, request);

                if (updatedCourse == null)
                {
                    return NotFound(new ApiResponse<CourseResponse, object?>(
                        false,
                        "Không tìm thấy khóa học để cập nhật",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<CourseResponse, object?>(
                    true,
                    "Cập nhật khóa học thành công",
                    updatedCourse,
                    null
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CourseResponse, object?>(
                    false,
                    ex.Message,
                    null,
                    null
                ));
            }
        }
    }
}
