using classService.DTOs;
using classService.Services;
using classService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace classService.Controllers
{


        [Route("api/[controller]")]
        [ApiController]
        public class ClassController(ClassServices classService) : ControllerBase
        {
            private readonly ClassServices _classService = classService;
            [HttpGet]
            public ActionResult<ApiResponse<List<ClassesResponse>, object?>> GetAllClasses()
            {
                try
                {
                    var classes = _classService.GetAllClasses();
                    return Ok(new ApiResponse<List<ClassesResponse>, object?>(
                        true,
                        "Lấy danh sách lớp học thành công",
                        classes,
                        null
                    ));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiResponse<List<ClassesResponse>, object?>(
                        false,
                        ex.Message,
                        null,
                        null
                    ));
                }
            }
        }
    }

