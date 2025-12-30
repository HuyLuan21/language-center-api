using authService.Models;
using authService.Repositories.Interfaces;
using authService.Utils;

namespace authService.Repositories
{
    public class UserRepository(DbContext dbContext) : IUserRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public User? GetUserByUsername(string username)
        {
            string query = $"SELECT * FROM Users WHERE Username = '{username}'";
            var dataTable = _dbContext.ExecuteQuery(query);
            var users = DatatableHelper.ConvertDataTable<Models.User>(dataTable);
            return users.FirstOrDefault();
        }
    }
}
