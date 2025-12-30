using Microsoft.AspNetCore.Mvc;
using authService.Services.Interfaces;
using authService.DTOs.Auth;
using authService.DTOs;     

namespace authService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<ApiResponse<LoginResponse, object?>> Login([FromBody] LoginRequest login)
        {
            try
            {
                var result = _authService.Login(login);

                if (result == null)
                {
                    return Unauthorized(new ApiResponse<LoginResponse, object?>
                    (
                        success: false,
                        message: "Tài khoản hoặc mật khẩu không chính xác",
                        data: null,
                        metaData: null
                    ));
                }

                return Ok(new ApiResponse<LoginResponse, object?>
                (
                    success: true,
                    message: "Đăng nhập thành công",
                    data: result,
                    metaData: null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<LoginResponse, object?>
                (
                    success: false,
                    message: "Lỗi hệ thống",
                    data: null,
                    metaData: new { error = ex.Message }
                ));
            }
        }
    }
}