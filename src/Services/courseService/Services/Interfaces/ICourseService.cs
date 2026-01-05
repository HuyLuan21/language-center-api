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
        bool DeleteCourse(Guid id);
        List<CourseResponse> SearchCourses(string keyword);
        CourseResponse? UpdateCourseThumbnail(Guid courseId, string thumbnailUrl);
        List<ClassesResponse> GetClassesByCourseId(Guid courseId);
        List<ClassesResponse> GetAllClasses();
        ClassesResponse? Createclass(ClassesCreateRequest request);
        ClassesResponse? Updateclass(Guid id,ClassesUpdateRequest request);
        bool DeleteClass(Guid id);

    }
}
