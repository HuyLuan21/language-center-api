using examService.DTOs.Exams;

namespace examService.Services.Interfaces
{
    public interface IExamService
    {
        Guid CreateExam(CreateExamRequest request);

        UpdateExamResponse UpdateExam(Guid id, UpdateExamRequest request);

        void DeleteExam(Guid examId);
    }
}
