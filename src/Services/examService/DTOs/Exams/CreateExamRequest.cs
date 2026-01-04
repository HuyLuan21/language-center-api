namespace examService.DTOs.Exams
{
    public class CreateExamRequest
    {
        public Guid ClassId { get; set; }
        public DateTime ExamDate { get; set; }
        public string StartTime { get; set; } = string.Empty; // service will parse to TimeSpan
        public string EndTime { get; set; } = string.Empty;

        public string? Room { get; set; }
        public double MaxScore { get; set; }
        public double? PassingScore { get; set; }
        public double Weightage { get; set; } = 0.0;

        public List<CreateExamPartRequest> ExamParts { get; set; } = new List<CreateExamPartRequest>();
    }

    public class CreateExamPartRequest
    {
        public string PartName { get; set; } = string.Empty;
        public double MaxScore { get; set; }
        public double? PassingScore { get; set; }
        public double Weightage { get; set; } = 0.0;
    }
}
