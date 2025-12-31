using courseService.DTOs;
namespace courseService.Services.Interfaces
{
    public interface ICourseService
    {
        List<CourseResponse> GetAllCourses();
    }
}
