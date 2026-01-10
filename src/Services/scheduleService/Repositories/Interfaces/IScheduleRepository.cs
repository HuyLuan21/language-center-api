using scheduleService.Models;
using System.Data;

namespace scheduleService.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        DataTable GetAllSchedules();
        DataTable GetScheduleById(Guid scheduleId);
        DataTable GetSchedulesByClassId(Guid classId);
        DataTable GetSchedulesByDateRange(DateTime startDate, DateTime endDate);
        DataTable GetScheduleDetailById(Guid scheduleId);
        DataTable GetScheduleDetailsByClassId(Guid classId);
        bool CreateSchedule(Schedule schedule);
        bool UpdateSchedule(Guid scheduleId, Schedule schedule);
        bool DeleteSchedule(Guid scheduleId);
    }
}
