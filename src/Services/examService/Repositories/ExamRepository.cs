using examService.DTOs.Exams;
using examService.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
namespace examService.Repositories
{
    public class ExamRepository(DbContext dbContext) : IExamRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public Guid CreateExam(ExamDTO exam, List<ExamPartDTO> parts)
        {
            string jsonParts = parts != null ? JsonSerializer.Serialize(parts) : "[]";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@exam_id", exam.exam_id),
                new SqlParameter("@class_id", exam.class_id),
                new SqlParameter("@exam_date", exam.exam_date),
                new SqlParameter("@start_time", exam.start_time.ToString(@"hh\:mm")),
                new SqlParameter("@end_time", exam.end_time.ToString(@"hh\:mm")),
                new SqlParameter("@room", exam.room ?? ""),
                new SqlParameter("@max_score", exam.max_score),
                new SqlParameter("@passing_score", exam.passing_score ?? 0),
                new SqlParameter("@weightage", exam.weightage),
                new SqlParameter("@json_parts", jsonParts)
            };

            _dbContext.ExecuteNonQuery("sp_CreateExamWithParts", parameters, CommandType.StoredProcedure);
            return exam.exam_id;
        }
        public bool UpdateExam(Guid id, ExamDTO exam, List<ExamPartDTO> parts)
        {
            string jsonParts = parts != null ? JsonSerializer.Serialize(parts) : "[]";

            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@exam_id", id),
            new SqlParameter("@class_id", exam.class_id),
            new SqlParameter("@exam_date", exam.exam_date),
            new SqlParameter("@start_time", exam.start_time.ToString(@"hh\:mm")),
            new SqlParameter("@end_time", exam.end_time.ToString(@"hh\:mm")),
            new SqlParameter("@room", (object?)exam.room ?? DBNull.Value),
            new SqlParameter("@max_score", exam.max_score),
            new SqlParameter("@passing_score", (object?)exam.passing_score ?? 0),
            new SqlParameter("@weightage", exam.weightage),
            new SqlParameter("@json_parts", jsonParts)
            };

            object? result = _dbContext.ExecuteScalar("sp_UpdateExamWithParts", parameters, CommandType.StoredProcedure);

            if (result != null && int.TryParse(result.ToString(), out int status))
            {
                return status == 1;
            }
            return false;
        }
        public bool DeleteExam(Guid id)
        {
            // Implementation for deleting an exam from the database
            throw new NotImplementedException();
        }
    }
}
