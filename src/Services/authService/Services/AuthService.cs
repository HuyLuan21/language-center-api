using authService.DTOs.Auth;
using authService.Models;
using authService.Repositories.Interfaces;
using authService.Services.Interfaces;
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

            if (!BCrypt.Net.BCrypt.Verify(request.password, user.password_hash))
            {
                return null;
            }

            string token = GenerateJwtToken(user);
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expiryMinutes = double.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var response = new LoginResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                User = new UserResponse
                {
                    UserId = user.user_id,
                    Username = user.username,
                    Email = user.email,
                    Role = user.role,
                    AvatarUrl = user.avatar_url
                }
            };
            return response;
        }


        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new Exception("JWT Secret Key is not configured.");
            var issuer = jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer is not configured.");
            var audience = jwtSettings["Audience"] ?? throw new Exception("JWT Audience is not configured.");
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.user_id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email ?? ""),
                new Claim("role", user.role.ToString()),
                new Claim("username", user.username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
