namespace examService.DTOs.Exams
{
    public class UpdateExamRequest
    {
        public Guid ClassId { get; set; }
        public DateTime ExamDate { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string? Room { get; set; }
        public double MaxScore { get; set; }
        public double? PassingScore { get; set; }
        public double Weightage { get; set; }

        public List<UpdateExamPartRequest> ExamParts { get; set; } = new List<UpdateExamPartRequest>();
    }

    public class UpdateExamPartRequest
    {
        // Không cần ID cũ, vì ta sẽ xóa hết tạo mới ID
        public string PartName { get; set; } = string.Empty;
        public double MaxScore { get; set; }
        public double? PassingScore { get; set; }
        public double Weightage { get; set; }
    }
}