using System;
using System.Collections.Generic;

namespace examService.DTOs.Exams
{
    public class ExamResponse
    {
        public Guid ExamId { get; set; }
        public Guid ClassId { get; set; }

        public string ExamDate { get; set; } = string.Empty;
        public string TimeRange { get; set; } = string.Empty;

        public string? Room { get; set; }
        public double MaxScore { get; set; }
        public double PassingScore { get; set; }
        public double Weightage { get; set; }

        public List<ExamPartResponse> Parts { get; set; } = new List<ExamPartResponse>();
    }

    public class ExamPartResponse
    {
        public Guid ExamPartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public double MaxScore { get; set; }
        public double PassingScore { get; set; }
        public double Weightage { get; set; }
    }
}