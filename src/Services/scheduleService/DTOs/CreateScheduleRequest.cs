using System.ComponentModel.DataAnnotations;

namespace scheduleService.DTOs
{
    public class CreateScheduleRequest
    {
        [Required(ErrorMessage = "Class ID is required")]
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Study date is required")]
        public DateTime StudyDate { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public string StartTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "End time is required")]
        public string EndTime { get; set; } = string.Empty;

        public string? Room { get; set; }
    }
}
