using examService.DTOs.Exams;
using System.Diagnostics.Eventing.Reader;
namespace examService.Repositories.Interfaces
{
    public interface IExamRepository
    {
        Guid CreateExam(ExamDTO exam, List<ExamPartDTO> parts);
        bool UpdateExam(Guid id, ExamDTO exam, List<ExamPartDTO> parts);
        bool DeleteExam(Guid id);
    }
}
