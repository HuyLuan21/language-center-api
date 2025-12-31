using courseService.DTOs;
using courseService.Repositories.Interfaces;
using courseService.Services.Interfaces;

namespace courseService.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        public List<CourseResponse> GetAllCourses()
        {
            var courses = _repository.GetAllCourses();

            return courses.Select(c => new CourseResponse
            {
                course_id = c.course_id,
                course_name = c.course_name,
                language_level_id = c.language_level_id,
                description = c.description,
                duration_hours = c.duration_hours,
                fee = c.fee,
                thumbnail_url = c.thumbnail_url,
            }).ToList();
        }
    }
}
