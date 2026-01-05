//using courseService.DTOs.Auth;
using courseService.DTOs;
using courseService.Services;
using Microsoft.AspNetCore.Mvc;

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
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object?, object?>> DeleteCourse(Guid id)
        {
            try
            {
                bool isDeleted = _courseService.DeleteCourse(id);
                if (!isDeleted)
                {
                    return NotFound(new ApiResponse<object?, object?>(
                        false,
                        "Không tìm thấy khóa học để xóa",
                        null,
                        null
                    ));
                }
                return Ok(new ApiResponse<object?, object?>(
                    true,
                    "Xóa khóa học thành công",
                    null,
                    null
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object?, object?>(
                    false,
                    ex.Message,
                    null,
                    null
                ));
            }
        }
        [HttpGet("search/{keyword}")]
        public ActionResult<ApiResponse<List<CourseResponse>, object?>> SearchCourses([FromRoute] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new ApiResponse<List<CourseResponse>, object?>(
                    false,
                    "Từ khóa tìm kiếm không được để trống",
                    null,
                    null
                ));
            }

            var courses = _courseService.SearchCourses(keyword);

            return Ok(new ApiResponse<List<CourseResponse>, object?>(
                true,
                "Tìm kiếm khóa học thành công",
                courses,
                null
            ));
        }
        [HttpPost("{id}/thumbnail")]
        public ActionResult<ApiResponse<CourseResponse, object?>> UpdateCourseThumbnail(Guid id,
        [FromBody] CourseUpdateThumbnailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.thumbnail_url))
            {
                return BadRequest(new ApiResponse<CourseResponse, object?>(
                    false,
                    "Thumbnail URL không hợp lệ",
                    null,
                    null
                ));
            }

            var updatedCourse = _courseService.UpdateCourseThumbnail(id, request.thumbnail_url);

            if (updatedCourse == null)
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
                "Cập nhật thumbnail thành công",
                updatedCourse,
                null
            ));
        }

    }
}
