using courseService.DTOs;
using courseService.Models;
namespace courseService.Services.Interfaces
{
    public interface ICourseService
    {
        List<CourseResponse> GetAllCourses();
        CourseResponse? GetCourseById(Guid id);
        CourseResponse? CreateCourse(CourseCreateRequest request);
        CourseResponse? UpdateCourse(Guid id, CourseUpdateRequest request);

    }
}
