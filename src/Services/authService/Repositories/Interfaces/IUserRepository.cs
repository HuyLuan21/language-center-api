using authService.Models;
namespace authService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByUsername(string username);
    }
}
