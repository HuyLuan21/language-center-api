namespace enrollmentService.DTOs
{
    public class EnrollmentDetailDTO
    {
        // ===== Enrollment =====
        public Guid EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;

        // ===== Student =====
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // ===== Class =====
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxStudents { get; set; }
        public string ClassStatus { get; set; } = string.Empty;

        // ===== Course =====
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int DurationHours { get; set; }
        public decimal Fee { get; set; }

        // ===== Language =====
        public string LanguageCode { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;

        // ===== Teacher =====
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }

        // ===== Invoice (optional) =====
        public Guid? InvoiceId { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string InvoiceStatus { get; set; } = string.Empty;
    }

}
