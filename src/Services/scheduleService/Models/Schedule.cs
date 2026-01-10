namespace scheduleService.Models
{
    public class Schedule
    {
        public Guid ScheduleId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime StudyDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Room { get; set; }
    }
}
