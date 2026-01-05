using classService.Models;
using classService.Repositories.Interfaces;
using classService.Utils;
namespace classService.Repositories
{
    public class ClassesRepository(DbContext dbContext) : IClassesRepository
    {
        private readonly DbContext _dbContext = dbContext;
        public List<Classes> GetAllClasses()
        {
            string query = $"SELECT * from Classes";
            var dataTable = _dbContext.ExecuteQuery(query);
            var classes = DatatableHelper.ConvertDataTable<Models.Classes>(dataTable);
            return classes;
        }

    }

}