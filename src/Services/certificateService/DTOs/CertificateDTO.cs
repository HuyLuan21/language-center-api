namespace certificateService.DTOs
{
    public class CertificateDTO
    {
        public Guid CertificateId { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string? CertificateUrl { get; set; }
        public string CertificateStatus { get; set; } = string.Empty;
        public string? StudentName { get; set; }
    }

    public class CertificateDetailDTO
    {
        public Guid CertificateId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? StudentEmail { get; set; }
        public string? StudentPhone { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? CourseDescription { get; set; }
        public int DurationHours { get; set; }
        public string LanguageName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public string? LevelDescription { get; set; }
        public DateTime IssueDate { get; set; }
        public string? CertificateUrl { get; set; }
        public string CertificateStatus { get; set; } = string.Empty;
    }

    public class GetStudentCertificatesResponse
    {
        public Guid StudentId { get; set; }
        public List<CertificateDTO> Certificates { get; set; } = new List<CertificateDTO>();
    }

    public class GetAllCertificatesResponse
    {
        public List<CertificateDTO> Certificates { get; set; } = new List<CertificateDTO>();
        public int TotalCount { get; set; }
    }
}
