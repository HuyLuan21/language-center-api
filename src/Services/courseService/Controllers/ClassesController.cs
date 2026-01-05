using courseService.DTOs;
using courseService.Models;
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
        [HttpPost]
        public ActionResult<ApiResponse<ClassesResponse, object?>> CreateClass(ClassesCreateRequest request)
        {
            try
            {
                var createdClass = _courseService.Createclass(request);
                return Ok(new ApiResponse<ClassesResponse, object?>(
                    true,
                    "Tạo lớp học thành công",
                    createdClass,
                    null
                     ));

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ClassesResponse, object?>(
                    false,
                    $"Tạo lớp học thất bại: {ex.Message}",
                    null,
                    null
                ));
            }

        }
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<ClassesResponse, object?>> UpdateClass(Guid id, [FromBody] ClassesUpdateRequest request)
        {
            try
            {
                var updatedClass = _courseService.Updateclass(id, request);
                if (updatedClass == null)
                {
                    return NotFound(new ApiResponse<ClassesResponse, object?>(
                        false,
                        "Không tìm thấy lớp học để cập nhật",
                        null,
                        null
                    ));
                }
                return Ok(new ApiResponse<ClassesResponse, object?>(
                    true,
                    "Cập nhật lớp học thành công",
                    updatedClass,
                    null
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ClassesResponse, object?>(
                    false,
                    $"Cập nhật lớp học thất bại: {ex.Message}",
                    null,
                    null
                ));
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object?, object?>> DeleteClass(Guid id)
        {
            try
            {
                bool isDeleted = _courseService.DeleteClass(id);
                if (!isDeleted)
                {
                    return NotFound(new ApiResponse<object?, object?>(
                        false,
                        "Không tìm thấy lớp học để xóa",
                        null,
                        null
                    ));
                }
                return Ok(new ApiResponse<object?, object?>(
                    true,
                    "Xóa lớp học thành công",
                    null,
                    null
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object?, object?>(
                    false,
                    $"Xóa lớp học thất bại: {ex.Message}",
                    null,
                    null
                ));
            }
        }
        [HttpGet("{id}/students")]
        public ActionResult<ApiResponse<List<StudentsResponse>, object?>> GetStudentsByClassesId(Guid id)
        {
            try
            {
                var students = _courseService.GetStudentsByClassesId(id);

                if (students == null || students.Count == 0)
                {
                    return NotFound(new ApiResponse<List<StudentsResponse>, object?>(
                        false,
                        "Không tìm thấy học sinh trong lớp học này",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<List<StudentsResponse>, object?>(
                    true,
                    "Lấy danh sách học sinh thành công",
                    students,
                    null
                    ));
             
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<List<StudentsResponse>, object?>(
                  false,
                  ex.Message,
                  null,
                  null
              ));
            }
        }
    }
}
