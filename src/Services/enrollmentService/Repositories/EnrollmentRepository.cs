using System.Data;
using enrollmentService.DTOs;

namespace enrollmentService.Repositories
{
    public class EnrollmentRepository
    {
        private DbContext _dbContext;
        public EnrollmentRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataTable GetEnrollmentsByStudentId(Guid studentId)
        {
            string query = "SELECT * FROM Enrollments WHERE student_id = @studentId";
            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId }
            };
            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }

        public DataTable GetEnrollmentDetail(Guid enrollmentId)
        {
            var query = $@"
            SELECT 
                e.enrollment_id,
                e.enrollment_date,
                e.enrollment_status,

                s.student_id,
                u.user_id,
                u.full_name AS StudentName,
                u.email,
                u.phone,

                c.class_id,
                c.class_name,
                c.start_date,
                c.end_date,
                c.max_students,
                c.class_status,

                co.course_id,
                co.course_name,
                co.duration_hours,
                co.fee,

                l.language_code,
                l.language_name,
                ll.level_name,

                t.teacher_id,
                tu.full_name AS TeacherName,
                t.specialization,
                t.years_of_experience,

                i.invoice_id,
                i.amount AS InvoiceAmount,
                i.invoice_status
            FROM Enrollments e
            JOIN Students s ON e.student_id = s.student_id
            JOIN Users u ON s.user_id = u.user_id
            JOIN Classes c ON e.class_id = c.class_id
            JOIN Courses co ON c.course_id = co.course_id
            JOIN Languages_Levels ll ON co.language_level_id = ll.language_level_id
            JOIN Languages l ON ll.language_code = l.language_code
            JOIN Teacher t ON c.teacher_id = t.teacher_id
            JOIN Users tu ON t.user_id = tu.user_id
            LEFT JOIN Invoices i ON e.enrollment_id = i.enrollment_id
            WHERE e.enrollment_id = '{enrollmentId}'
            ";

            var table = _dbContext.ExecuteQuery(query);
            return table;
        }

        public bool CreateEnrollment(EnrollmentDTO enrollment)
        {
            string query = $"INSERT INTO Enrollments (enrollment_id, student_id, class_id, enrollment_date, enrollment_status) " +
                           $"VALUES ('{enrollment.EnrollmentId}', '{enrollment.StudentId}', '{enrollment.ClassId}', '{enrollment.EnrollmentDate}', '{enrollment.EnrollmentStatus}')";
            int rowsAffected = _dbContext.ExecuteNonQuery(query);
            return rowsAffected > 0;
        }

        public bool UpdateEnrollmentStatus(Guid enrollmentId, string newStatus)
        {
            string query = $"UPDATE Enrollments SET enrollment_status = '{newStatus}' WHERE enrollment_id = '{enrollmentId}'";
            int rowsAffected = _dbContext.ExecuteNonQuery(query);
            return rowsAffected > 0;
        }

        public bool DeleteEnrollment(Guid enrollmentId)
        {
            string query = $"DELETE FROM Enrollments WHERE enrollment_id = '{enrollmentId}'";
            int rowsAffected = _dbContext.ExecuteNonQuery(query);
            return rowsAffected > 0;
        }

        public DataTable GetAllCourse()
        {
            string query = @"
                SELECT 
                    c.course_id,
                    c.course_name,
                    c.duration_hours,
                    c.fee,
                    c.thumbnail_url,
                    c.course_status,
                    l.language_code,
                    l.language_name,
                    ll.level_name
                FROM Courses c
                JOIN Languages_Levels ll ON c.language_level_id = ll.language_level_id
                JOIN Languages l ON ll.language_code = l.language_code
                WHERE c.course_status = 'active'";
            return _dbContext.ExecuteQuery(query);
        }

        public bool UpdatePaymentStatus(Guid enrollmentId, string paymentStatus)
        {
            string query = $"UPDATE Enrollments SET payment_status = '{paymentStatus}' WHERE enrollment_id = '{enrollmentId}'";
            int rowsAffected = _dbContext.ExecuteNonQuery(query);
            return rowsAffected > 0;
        }
    }
}
