using courseService.DTOs;
using courseService.Services;
using Microsoft.AspNetCore.Mvc;
namespace courseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController(CourseService courseService) : ControllerBase
    {
        private readonly CourseService _courseService = courseService;
        [HttpGet]
        public ActionResult<ApiResponse<List<ClassesResponse>, object?>> GetAllClasses()
        {
            var classes = _courseService.GetAllClasses();
            return Ok(new ApiResponse<List<ClassesResponse>, object?>(
                true,
                "Lấy danh sách lớp học thành công",
                classes,
                null
            ));
        }
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<List<ClassesResponse>, object?>> GetClassById(Guid id)
        {
            var classes = _courseService.GetClassById(id);
            if (classes == null)
            {
                return NotFound(new ApiResponse<ClassesResponse, object?>(
                      false,
                      "Không tìm thấy khóa học",
                      null,
                      null
                  ));
            }
            else
            {
                return Ok(new ApiResponse<ClassesResponse, object?>(
               true,
               "Lấy khóa học thành công",
               classes,
               null
           ));
            }
        }
    }
}
