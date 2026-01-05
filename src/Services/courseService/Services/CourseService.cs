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
        public List<ClassesResponse> GetClassesByCourseId(Guid courseId)
        {
            var classes = _repository.GetClassesByCourseId(courseId);
            return classes.Select(c => new ClassesResponse
            {
                class_id = c.class_id,
                course_id = c.course_id,
                teacher_id = c.teacher_id,
                class_name = c.class_name,
                start_date = c.start_date,
                end_date = c.end_date,
                max_students = c.max_students,
                class_status = c.class_status
            }).ToList();
        }
        public List<ClassesResponse> GetAllClasses()
        {
            var classes = _repository.GetAllClasses();
            return classes.Select(c => new ClassesResponse
            {
                class_id = c.class_id,
                course_id = c.course_id,
                teacher_id = c.teacher_id,
                class_name = c.class_name,
                start_date = c.start_date,
                end_date = c.end_date,
                max_students = c.max_students,
                class_status = c.class_status
            }).ToList();
        }
        public ClassesResponse? GetClassById(Guid classId)
        {
            var classes = _repository.GetClassById(classId);

            if (classes == null)
                return null;

            return new ClassesResponse
            {
                class_id = classes.class_id,
                course_id = classes.course_id,
                teacher_id = classes.teacher_id,
                class_name = classes.class_name,
                start_date = classes.start_date,
                end_date = classes.end_date,
                max_students = classes.max_students,
                class_status = classes.class_status
            };
        }
        public ClassesResponse Createclass(ClassesCreateRequest request)
        {
            var classes = new Classes
            {
                class_id = Guid.NewGuid(),
                course_id = request.course_id,
                teacher_id = request.teacher_id,
                class_name = request.class_name,
                start_date = request.start_date,
                end_date = request.end_date,
                max_students = request.max_students,
                class_status = request.class_status
            };
            _repository.CreateClass(classes);
            return new ClassesResponse
            {
                class_id = classes.class_id,
                course_id = classes.course_id,
                teacher_id = classes.teacher_id,
                class_name = classes.class_name,
                start_date = classes.start_date,
                end_date = classes.end_date,
                max_students = classes.max_students,
                class_status = classes.class_status
            };
        }
        public ClassesResponse? Updateclass(Guid id,ClassesUpdateRequest request)
        {
            var existingClass = _repository.GetClassById(id);
            if (existingClass == null)
                return null;
            var classes = new Classes
            {
                class_id = request.class_id,
                course_id = request.course_id,
                teacher_id = request.teacher_id,
                class_name = request.class_name,
                start_date = request.start_date,
                end_date = request.end_date,
                max_students = request.max_students,
                class_status = request.class_status
            };
            _repository.UpdateClass(classes);
            return new ClassesResponse
            {
                class_id = classes.class_id,
                course_id = classes.course_id,
                teacher_id = classes.teacher_id,
                class_name = classes.class_name,
                start_date = classes.start_date,
                end_date = classes.end_date,
                max_students = classes.max_students,
                class_status = classes.class_status
            };

        }
        public bool DeleteClass(Guid id)
        {
            var existingClass = _repository.GetClassById(id);
            if (existingClass == null)
                return false;
            _repository.DeleteClass(id);
            return true;
        }
    }
}

