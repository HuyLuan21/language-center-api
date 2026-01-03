using authService.Models.Enums;

using System.Text.Json.Serialization;

namespace authService.DTOs.Auth
{
    public class LoginResponse
    {
        [JsonPropertyName("access_token")] 
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        [JsonPropertyName("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [JsonPropertyName("user")]
        public UserResponse User { get; set; } = new UserResponse();
    }

    public class UserResponse
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; } = UserRole.student;

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }
}