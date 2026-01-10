using System.Data;

namespace paymentService.Repositories
{
    public class InvoiceRepository
    {
        private readonly DbContext _dbContext;

        public InvoiceRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataTable GetInvoicesByStudentId(Guid studentId)
        {
            string query = @"
                SELECT 
                    i.invoice_id,
                    i.enrollment_id,
                    i.amount,
                    i.issue_date,
                    i.due_date,
                    i.payment_date,
                    i.invoice_status
                FROM Invoices i
                INNER JOIN Enrollments e ON i.enrollment_id = e.enrollment_id
                WHERE e.student_id = @studentId
                ORDER BY i.issue_date DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId }
            };

            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }

        public DataTable GetPaymentHistoryByStudentId(Guid studentId)
        {
            string query = @"
                SELECT 
                    i.invoice_id,
                    i.enrollment_id,
                    i.amount,
                    i.issue_date,
                    i.due_date,
                    i.payment_date,
                    i.invoice_status,
                    e.enrollment_status,
                    e.class_id,
                    cl.class_name,
                    cl.course_id,
                    co.course_name,
                    l.language_name,
                    ll.level_name
                FROM Invoices i
                INNER JOIN Enrollments e ON i.enrollment_id = e.enrollment_id
                INNER JOIN Classes cl ON e.class_id = cl.class_id
                INNER JOIN Courses co ON cl.course_id = co.course_id
                INNER JOIN Languages_Levels ll ON co.language_level_id = ll.language_level_id
                INNER JOIN Languages l ON ll.language_code = l.language_code
                WHERE e.student_id = @studentId
                ORDER BY i.issue_date DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@studentId", studentId }
            };

            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }

        public DataTable GetInvoiceById(Guid invoiceId)
        {
            string query = @"
                SELECT 
                    invoice_id,
                    enrollment_id,
                    amount,
                    issue_date,
                    due_date,
                    payment_date,
                    invoice_status
                FROM Invoices
                WHERE invoice_id = @invoiceId";

            var parameters = new Dictionary<string, object>
            {
                { "@invoiceId", invoiceId }
            };

            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }

        public bool VerifyPayment(Guid invoiceId, DateTime paymentDate)
        {
            string query = @"
                UPDATE Invoices
                SET 
                    payment_date = @paymentDate,
                    invoice_status = 'paid'
                WHERE invoice_id = @invoiceId 
                AND invoice_status != 'paid'";

            var parameters = new Dictionary<string, object>
            {
                { "@invoiceId", invoiceId },
                { "@paymentDate", paymentDate }
            };

            try
            {
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection(_dbContext.ExecuteScalar("SELECT 1").ToString()))
                {
                    connection.ConnectionString = @"Server=DESKTOP-LQ6C96V\SQLEXPRESS;Database=LanguageCenterDB;Trusted_Connection=True;TrustServerCertificate=True;";
                    connection.Open();

                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@invoiceId", invoiceId);
                        command.Parameters.AddWithValue("@paymentDate", paymentDate);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateEnrollmentStatus(Guid enrollmentId, string status)
        {
            string query = @"
                UPDATE Enrollments
                SET enrollment_status = @status
                WHERE enrollment_id = @enrollmentId";

            var parameters = new Dictionary<string, object>
            {
                { "@enrollmentId", enrollmentId },
                { "@status", status }
            };

            try
            {
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection())
                {
                    connection.ConnectionString = @"Server=DESKTOP-LQ6C96V\SQLEXPRESS;Database=LanguageCenterDB;Trusted_Connection=True;TrustServerCertificate=True;";
                    connection.Open();

                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                        command.Parameters.AddWithValue("@status", status);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
