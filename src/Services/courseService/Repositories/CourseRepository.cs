using courseService.Models;
using courseService.Repositories.Interfaces;
using courseService.Utils;
namespace courseService.Repositories
{
    public class CourseRepository(DbContext dbContext) : ICourseRepository
    {
        private readonly DbContext _dbContext = dbContext;
        public List<Courese> GetAllCourses()
        {
            string query = $"SELECT * from Courses";
            var dataTable = _dbContext.ExecuteQuery(query);
            var course = DatatableHelper.ConvertDataTable<Models.Courese>(dataTable);
            return course;

        }

    }
}
