using authService.Models.Enums;

namespace authService.DTOs.Auth
{
    public class LoginResponse
    {
        public string access_token { get; set; } = string.Empty;
        public DateTime expires_at { get; set; }
        public UserResponse user { get; set; } = new UserResponse();
    }

    public class UserResponse
    {
        public Guid user_id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public UserRole role { get; set; } = UserRole.student;
        public string? avatar_url { get; set; }
    }
}
