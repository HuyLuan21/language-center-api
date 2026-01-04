namespace examService.DTOs.Exams
{
    public class ExamPartDTO
    {
        public Guid exam_part_id { get; set; }
        public Guid exam_id { get; set; }
        public string part_name { get; set; } = string.Empty;
        public double max_score { get; set; }
        public double? passing_score { get; set; }
        public double weightage { get; set; }
    }
}
