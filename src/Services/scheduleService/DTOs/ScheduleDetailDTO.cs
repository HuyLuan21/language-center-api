namespace scheduleService.DTOs
{
    public class ScheduleDetailDTO
    {
        public Guid ScheduleId { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateTime StudyDate { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string? Room { get; set; }
    }
}
