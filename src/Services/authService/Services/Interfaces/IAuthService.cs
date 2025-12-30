using authService.DTOs.Auth;
namespace authService.Services.Interfaces
{
    public interface IAuthService
    {
        LoginResponse? Login(LoginRequest request);
    }
}
