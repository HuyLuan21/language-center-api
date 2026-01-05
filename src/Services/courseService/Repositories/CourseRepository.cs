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

        public Courese GetCourseById(Guid course)
        {
            string query = $"SELECT * from Courses where course_id = '{course}'";
            var dataTable = _dbContext.ExecuteQuery(query);
            var courses = DatatableHelper.ConvertDataTable<Models.Courese>(dataTable);
            return courses.FirstOrDefault();
        }
        public void CreateCourse(Courese course)
        {
            string query = $"INSERT INTO Courses (course_name, language_level_id, description, duration_hours, fee, thumbnail_url, course_status) " +
            $"VALUES ('{course.course_name}', '{course.language_level_id}', '{course.description}', {course.duration_hours}, {course.fee}, '{course.thumbnail_url}', '{course.course_status}')";
            _dbContext.ExecuteNonQuery(query);

        }
        public void UpdateCourse(Courese course)
        {
            string query = $"UPDATE Courses SET " +
            $"course_name = '{course.course_name}', " +
            $"language_level_id = '{course.language_level_id}', " +
            $"description = '{course.description}', " +
            $"duration_hours = {course.duration_hours}, " +
            $"fee = {course.fee}, " +
            $"thumbnail_url = '{course.thumbnail_url}', " +
            $"course_status = '{course.course_status}' " +
            $"WHERE course_id = '{course.course_id}'";
            _dbContext.ExecuteNonQuery(query);
        }
        public void DeleteCourse(Guid courseId)
        {
            string query = $"DELETE FROM Courses WHERE course_id = '{courseId}'";
            _dbContext.ExecuteNonQuery(query);
        }
        public List<Courese> SearchCourses(string keyword)
        {
            bool isGuid = Guid.TryParse(keyword, out Guid guid);

            string query;

            if (isGuid)
            {
                query = $@"
            SELECT * FROM Courses
            WHERE course_name LIKE '%{keyword}%'
            OR course_id = '{guid}'
            OR language_level_id = '{guid}'";
            }
            else
            {
                query = $@"SELECT * FROM Courses WHERE course_name LIKE '%{keyword}%'";
            }

            var dataTable = _dbContext.ExecuteQuery(query);
            return DatatableHelper.ConvertDataTable<Courese>(dataTable);
        }
        public void UpdateCourseThumbnail(Guid courseId, string thumbnailUrl)
        {
            string query = $@"UPDATE Courses
            SET thumbnail_url = '{thumbnailUrl}'
            WHERE course_id = '{courseId}'";
            _dbContext.ExecuteNonQuery(query);
        }
        public List<Classes> GetClassesByCourseId(Guid courseId)
        {
            string query = $"SELECT * from Classes where course_id = '{courseId}'";
            var dataTable = _dbContext.ExecuteQuery(query);
            var classes = DatatableHelper.ConvertDataTable<Models.Classes>(dataTable);
            return classes;
        }
        public List<Classes> GetAllClasses()
        {
            string query = $"SELECT * from Classes";
            var dataTable = _dbContext.ExecuteQuery(query);
            var classes = DatatableHelper.ConvertDataTable<Models.Classes>(dataTable);
            return classes;
        }
        public Classes GetClassById(Guid classId)
        {
            string query = $"SELECT * FROM Classes WHERE class_id = '{classId}'";
            var dataTable = _dbContext.ExecuteQuery(query);

            var classList = DatatableHelper.ConvertDataTable<Models.Classes>(dataTable);
            return classList.FirstOrDefault();
        }

        public void CreateClass(Classes classes)
        {
            string query = "INSERT INTO Classes (course_id, teacher_id, class_name, start_date, end_date, max_students, class_status) " +
                           $"VALUES ('{classes.course_id}', '{classes.teacher_id}', N'{classes.class_name}', '{classes.start_date:yyyy-MM-dd}', '{classes.end_date:yyyy-MM-dd}', {classes.max_students}, N'{classes.class_status}')";

            _dbContext.ExecuteNonQuery(query);
        }
        public void UpdateClass(Classes classes)
        {
            string query = "UPDATE Classes SET " +
                           $"course_id = '{classes.course_id}', " +
                           $"teacher_id = '{classes.teacher_id}', " +
                           $"class_name = N'{classes.class_name}', " +
                           $"start_date = '{classes.start_date:yyyy-MM-dd}', " +
                           $"end_date = '{classes.end_date:yyyy-MM-dd}', " +
                           $"max_students = {classes.max_students}, " +
                           $"class_status = N'{classes.class_status}' " +
                           $"WHERE class_id = '{classes.class_id}'";
            _dbContext.ExecuteNonQuery(query);
        }
        public void DeleteClass(Guid classId)
        {
            string query = $"DELETE FROM Classes WHERE class_id = '{classId}'";
            _dbContext.ExecuteNonQuery(query);
        }

    }
}
