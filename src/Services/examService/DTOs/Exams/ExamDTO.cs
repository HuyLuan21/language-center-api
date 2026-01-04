namespace examService.DTOs.Exams
{
    public class ExamDTO
    {
        public Guid exam_id { get; set; }
        public Guid class_id { get; set; }
        public DateTime exam_date { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public string? room { get; set; }
        public double max_score { get; set; }
        public double? passing_score { get; set; }
        public double weightage { get; set; }
    }
}
