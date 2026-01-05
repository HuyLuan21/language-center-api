using courseService.DTOs;
using courseService.Models;
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
                course_status = c.course_status
            }).ToList();
        }

        public CourseResponse? GetCourseById(Guid id)
        {
            var course = _repository.GetCourseById(id);

            if (course == null)
                return null;

            return new CourseResponse
            {
                course_id = course.course_id,
                course_name = course.course_name,
                language_level_id = course.language_level_id,
                description = course.description,
                duration_hours = course.duration_hours,
                fee = course.fee,
                thumbnail_url = course.thumbnail_url,
                course_status = course.course_status
            };
        }
        public CourseResponse CreateCourse(CourseCreateRequest request)
        {
            var course = new Courese
            {
                course_id = Guid.NewGuid(),
                course_name = request.course_name,
                language_level_id = request.language_level_id,
                description = request.description,
                duration_hours = request.duration_hours,
                fee = request.fee,
                thumbnail_url = request.thumbnail_url,
                course_status = request.course_status
            };

            _repository.CreateCourse(course);

            return new CourseResponse
            {
                course_id = course.course_id,
                course_name = course.course_name,
                language_level_id = course.language_level_id,
                description = course.description,
                duration_hours = course.duration_hours,
                fee = course.fee,
                thumbnail_url = course.thumbnail_url,
                course_status = course.course_status
            };
        }
        public CourseResponse? UpdateCourse(Guid id, CourseUpdateRequest request)
        {
            var existingCourse = _repository.GetCourseById(id);
            if (existingCourse == null)
                return null;

            var course = new Courese
            {
                course_id = id,
                course_name = request.course_name,
                language_level_id = request.language_level_id,
                description = request.description,
                duration_hours = request.duration_hours,
                fee = request.fee,
                thumbnail_url = request.thumbnail_url,
                course_status = request.course_status
            };

            _repository.UpdateCourse(course);

            return new CourseResponse
            {
                course_id = course.course_id,
                course_name = course.course_name,
                language_level_id = course.language_level_id,
                description = course.description,
                duration_hours = course.duration_hours,
                fee = course.fee,
                thumbnail_url = course.thumbnail_url,
                course_status = course.course_status
            };
        }
       public bool DeleteCourse(Guid id)
        {
            var existingCourse = _repository.GetCourseById(id);
            if (existingCourse == null)
                return false;
            _repository.DeleteCourse(id);
            return true;
        }
        public List<CourseResponse> SearchCourses(string keyword)
        {
            var courses = _repository.SearchCourses(keyword);
            return courses.Select(c => new CourseResponse
            {
                course_id = c.course_id,
                course_name = c.course_name,
                language_level_id = c.language_level_id,
                description = c.description,
                duration_hours = c.duration_hours,
                fee = c.fee,
                thumbnail_url = c.thumbnail_url,
                course_status = c.course_status
            }).ToList();
        }
        public CourseResponse? UpdateCourseThumbnail(Guid courseId, string thumbnailUrl)
        {
            var course = _repository.GetCourseById(courseId);
            if (course == null)
                return null;

            _repository.UpdateCourseThumbnail(courseId, thumbnailUrl);

            course.thumbnail_url = thumbnailUrl;

            return new CourseResponse
            {
                course_id = course.course_id,
                course_name = course.course_name,
                language_level_id = course.language_level_id,
                description = course.description,
                duration_hours = course.duration_hours,
                fee = course.fee,
                thumbnail_url = thumbnailUrl,
                course_status = course.course_status
            };
        }
    }
}

