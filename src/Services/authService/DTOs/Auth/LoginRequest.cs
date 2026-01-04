using System.ComponentModel.DataAnnotations;
namespace authService.DTOs.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "username là bắt buộc")]
        public required string username { get; set; }
        [Required(ErrorMessage = "password là bắt buộc")]
        public required string password { get; set; }
    }
}
