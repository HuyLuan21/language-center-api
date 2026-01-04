namespace examService.DTOs.Exams
{
    public class UpdateExamResponse
    {
        public Guid ExamId { get; set; }
        public List<ExamPartResponse> UpdatedParts { get; set; } = new List<ExamPartResponse>();
    }

}
