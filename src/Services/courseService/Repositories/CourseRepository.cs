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
        //Get  course by id
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
    }
}
