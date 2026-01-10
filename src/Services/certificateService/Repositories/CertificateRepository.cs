using System.Data;

namespace certificateService.Repositories
{
    public class CertificateRepository
    {
        private readonly DbContext _dbContext;

        public CertificateRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataTable GetCertificatesByStudentId(Guid studentId)
        {
            string query = @"
                SELECT 
                    c.certificate_id,
                    c.student_id,
                    c.course_id,
                    co.course_name,
                    l.language_name,
                    ll.level_name,
                    c.issue_date,
                    c.certificate_url,
                    c.certificate_status
                FROM Certificates c
                INNER JOIN Courses co ON c.course_id = co.course_id
                INNER JOIN Languages_Levels ll ON co.language_level_id = ll.language_level_id
                INNER JOIN Languages l ON ll.language_code = l.language_code
                WHERE c.student_id = @studentId
                ORDER BY c.issue_date DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId }
            };

            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }

        public DataTable GetAllCertificates()
        {
            string query = @"
                SELECT 
                    c.certificate_id,
                    c.student_id,
                    c.course_id,
                    co.course_name,
                    l.language_name,
                    ll.level_name,
                    c.issue_date,
                    c.certificate_url,
                    c.certificate_status,
                    u.full_name as student_name
                FROM Certificates c
                INNER JOIN Courses co ON c.course_id = co.course_id
                INNER JOIN Languages_Levels ll ON co.language_level_id = ll.language_level_id
                INNER JOIN Languages l ON ll.language_code = l.language_code
                INNER JOIN Students s ON c.student_id = s.student_id
                INNER JOIN Users u ON s.user_id = u.user_id
                ORDER BY c.issue_date DESC";

            return _dbContext.ExecuteQuery(query);
        }

        public DataTable GetCertificateById(Guid certificateId)
        {
            string query = @"
                SELECT 
                    c.certificate_id,
                    c.student_id,
                    c.course_id,
                    co.course_name,
                    co.description as course_description,
                    co.duration_hours,
                    l.language_name,
                    ll.level_name,
                    ll.description as level_description,
                    c.issue_date,
                    c.certificate_url,
                    c.certificate_status,
                    u.full_name as student_name,
                    u.email as student_email,
                    u.phone as student_phone
                FROM Certificates c
                INNER JOIN Courses co ON c.course_id = co.course_id
                INNER JOIN Languages_Levels ll ON co.language_level_id = ll.language_level_id
                INNER JOIN Languages l ON ll.language_code = l.language_code
                INNER JOIN Students s ON c.student_id = s.student_id
                INNER JOIN Users u ON s.user_id = u.user_id
                WHERE c.certificate_id = @certificateId";

            var parameters = new Dictionary<string, object>
            {
                { "@certificateId", certificateId }
            };

            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }
    }
}
