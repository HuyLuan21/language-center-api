using scheduleService.DTOs;

namespace scheduleService.Services.Interfaces
{
    public interface IScheduleService
    {
        List<ScheduleDTO> GetAllSchedules();
        ScheduleDTO? GetScheduleById(Guid scheduleId);
        List<ScheduleDTO> GetSchedulesByClassId(Guid classId);
        List<ScheduleDTO> GetSchedulesByDateRange(DateTime startDate, DateTime endDate);
        ScheduleDetailDTO? GetScheduleDetailById(Guid scheduleId);
        List<ScheduleDetailDTO> GetScheduleDetailsByClassId(Guid classId);
        ScheduleDTO? CreateSchedule(CreateScheduleRequest request);
        ScheduleDTO? UpdateSchedule(Guid scheduleId, UpdateScheduleRequest request);
        bool DeleteSchedule(Guid scheduleId);
    }
}
