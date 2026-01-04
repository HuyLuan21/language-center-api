namespace examService.DTOs.Exams
{
    public class CreateExamResponse
    {
        public Guid ExamId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
