namespace enrollmentService.DTOs
{
    public class CourseListResponse
    {
        public List<CourseDTO>? Courses { get; set; }
    }
    public class CourseDTO
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;
        public string LanguageName { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;

        public int DurationHours { get; set; }
        public decimal Fee { get; set; }

        public string ThumbnailUrl { get; set; } = string.Empty;
        public string CourseStatus { get; set; } = string.Empty;
    }
}
