using authService.DTOs.Auth;
using authService.Models;
using authService.Repositories.Interfaces;
using authService.Services.Interfaces;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace authService.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;
        public LoginResponse? Login(LoginRequest request)
        {
            var user = _userRepository.GetUserByUsername(request.username);
            if (user == null)
            {
                return null;
            }

            if(!BCrypt.Net.BCrypt.Verify(request.password, user.password_hash))
            {
                return null;
            }

            string token = GenerateJwtToken(user);

            var response = new LoginResponse
            {
                access_token = token,
                expires_at = DateTime.UtcNow.AddHours(1),
                user = new UserResponse
                {
                    user_id = user.user_id,
                    username = user.username,
                    email = user.email,
                    role = user.role,
                    avatar_url = user.avatar_url
                }
            };
            return response;
        }


        private string GenerateJwtToken(User user)
        {
            return "token ne` ahihi";
        }
    }
}
