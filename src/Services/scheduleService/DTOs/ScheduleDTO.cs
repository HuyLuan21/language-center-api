namespace scheduleService.DTOs
{
    public class ScheduleDTO
    {
        public Guid ScheduleId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime StudyDate { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string? Room { get; set; }
    }
}
