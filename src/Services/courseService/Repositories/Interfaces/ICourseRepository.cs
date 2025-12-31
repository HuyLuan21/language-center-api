using courseService.Models;
namespace courseService.Repositories.Interfaces

{
    public interface ICourseRepository
    {
      List<Courese> GetAllCourses();
    }
}
