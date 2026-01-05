using courseService.Models;
namespace courseService.Repositories.Interfaces

{
    public interface ICourseRepository
    {
        List<Courese> GetAllCourses();
        Courese? GetCourseById(Guid id);
        void CreateCourse(Courese course);
        void UpdateCourse(Courese courese);
        void DeleteCourse(Guid id);
        List<Courese> SearchCourses(string keyword);
        void UpdateCourseThumbnail(Guid courseId, string thumbnailUrl);

    }
}
