using System;
using System.ComponentModel.DataAnnotations;

namespace examService.Models
{
    public class Exam
    {
        [Required]
        public Guid exam_id { get; set; }

        [Required]
        public Guid class_id { get; set; }

        [Required]
        public DateTime exam_date { get; set; }

        [Required]
        public TimeSpan start_time { get; set; }

        [Required]
        public TimeSpan end_time { get; set; }

        public string? room { get; set; }

        [Required]
        public int max_score { get; set; }

        [Required]
        public int passing_score { get; set; }

        [Range(0, 1)]
        public double? weightage { get; set; }
    }
}
