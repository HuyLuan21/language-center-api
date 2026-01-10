using scheduleService.DTOs;
using scheduleService.Models;
using scheduleService.Repositories.Interfaces;
using scheduleService.Services.Interfaces;
using System.Data;

namespace scheduleService.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public List<ScheduleDTO> GetAllSchedules()
        {
            DataTable data = _scheduleRepository.GetAllSchedules();
            List<ScheduleDTO> schedules = new List<ScheduleDTO>();

            foreach (DataRow row in data.Rows)
            {
                schedules.Add(MapToScheduleDTO(row));
            }

            return schedules;
        }

        public ScheduleDTO? GetScheduleById(Guid scheduleId)
        {
            DataTable data = _scheduleRepository.GetScheduleById(scheduleId);

            if (data.Rows.Count == 0)
                return null;

            return MapToScheduleDTO(data.Rows[0]);
        }

        public List<ScheduleDTO> GetSchedulesByClassId(Guid classId)
        {
            DataTable data = _scheduleRepository.GetSchedulesByClassId(classId);
            List<ScheduleDTO> schedules = new List<ScheduleDTO>();

            foreach (DataRow row in data.Rows)
            {
                schedules.Add(MapToScheduleDTO(row));
            }

            return schedules;
        }

        public List<ScheduleDTO> GetSchedulesByDateRange(DateTime startDate, DateTime endDate)
        {
            DataTable data = _scheduleRepository.GetSchedulesByDateRange(startDate, endDate);
            List<ScheduleDTO> schedules = new List<ScheduleDTO>();

            foreach (DataRow row in data.Rows)
            {
                schedules.Add(MapToScheduleDTO(row));
            }

            return schedules;
        }

        public ScheduleDetailDTO? GetScheduleDetailById(Guid scheduleId)
        {
            DataTable data = _scheduleRepository.GetScheduleDetailById(scheduleId);

            if (data.Rows.Count == 0)
                return null;

            return MapToScheduleDetailDTO(data.Rows[0]);
        }

        public List<ScheduleDetailDTO> GetScheduleDetailsByClassId(Guid classId)
        {
            DataTable data = _scheduleRepository.GetScheduleDetailsByClassId(classId);
            List<ScheduleDetailDTO> schedules = new List<ScheduleDetailDTO>();

            foreach (DataRow row in data.Rows)
            {
                schedules.Add(MapToScheduleDetailDTO(row));
            }

            return schedules;
        }

        public ScheduleDTO? CreateSchedule(CreateScheduleRequest request)
        {
            // Validate time format and parse
            if (!TimeSpan.TryParse(request.StartTime, out TimeSpan startTime))
                throw new ArgumentException("Invalid start time format. Use HH:mm:ss format.");

            if (!TimeSpan.TryParse(request.EndTime, out TimeSpan endTime))
                throw new ArgumentException("Invalid end time format. Use HH:mm:ss format.");

            // Validate that end time is after start time
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            var schedule = new Schedule
            {
                ScheduleId = Guid.NewGuid(),
                ClassId = request.ClassId,
                StudyDate = request.StudyDate.Date,
                StartTime = startTime,
                EndTime = endTime,
                Room = request.Room
            };

            bool isCreated = _scheduleRepository.CreateSchedule(schedule);

            if (!isCreated)
                return null;

            return new ScheduleDTO
            {
                ScheduleId = schedule.ScheduleId,
                ClassId = schedule.ClassId,
                StudyDate = schedule.StudyDate,
                StartTime = schedule.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = schedule.EndTime.ToString(@"hh\:mm\:ss"),
                Room = schedule.Room
            };
        }

        public ScheduleDTO? UpdateSchedule(Guid scheduleId, UpdateScheduleRequest request)
        {
            // Check if schedule exists
            var existingSchedule = GetScheduleById(scheduleId);
            if (existingSchedule == null)
                return null;

            // Validate time format and parse
            if (!TimeSpan.TryParse(request.StartTime, out TimeSpan startTime))
                throw new ArgumentException("Invalid start time format. Use HH:mm:ss format.");

            if (!TimeSpan.TryParse(request.EndTime, out TimeSpan endTime))
                throw new ArgumentException("Invalid end time format. Use HH:mm:ss format.");

            // Validate that end time is after start time
            if (endTime <= startTime)
                throw new ArgumentException("End time must be after start time.");

            var schedule = new Schedule
            {
                ScheduleId = scheduleId,
                ClassId = request.ClassId,
                StudyDate = request.StudyDate.Date,
                StartTime = startTime,
                EndTime = endTime,
                Room = request.Room
            };

            bool isUpdated = _scheduleRepository.UpdateSchedule(scheduleId, schedule);

            if (!isUpdated)
                return null;

            return new ScheduleDTO
            {
                ScheduleId = schedule.ScheduleId,
                ClassId = schedule.ClassId,
                StudyDate = schedule.StudyDate,
                StartTime = schedule.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = schedule.EndTime.ToString(@"hh\:mm\:ss"),
                Room = schedule.Room
            };
        }

        public bool DeleteSchedule(Guid scheduleId)
        {
            return _scheduleRepository.DeleteSchedule(scheduleId);
        }

        // Helper method to map DataRow to ScheduleDTO
        private ScheduleDTO MapToScheduleDTO(DataRow row)
        {
            return new ScheduleDTO
            {
                ScheduleId = Guid.Parse(row["schedule_id"].ToString()!),
                ClassId = Guid.Parse(row["class_id"].ToString()!),
                StudyDate = DateTime.Parse(row["study_date"].ToString()!),
                StartTime = TimeSpan.Parse(row["start_time"].ToString()!).ToString(@"hh\:mm\:ss"),
                EndTime = TimeSpan.Parse(row["end_time"].ToString()!).ToString(@"hh\:mm\:ss"),
                Room = row["room"] != DBNull.Value ? row["room"].ToString() : null
            };
        }

        // Helper method to map DataRow to ScheduleDetailDTO
        private ScheduleDetailDTO MapToScheduleDetailDTO(DataRow row)
        {
            return new ScheduleDetailDTO
            {
                ScheduleId = Guid.Parse(row["schedule_id"].ToString()!),
                ClassId = Guid.Parse(row["class_id"].ToString()!),
                ClassName = row["class_name"].ToString()!,
                CourseId = Guid.Parse(row["course_id"].ToString()!),
                CourseName = row["course_name"].ToString()!,
                StudyDate = DateTime.Parse(row["study_date"].ToString()!),
                StartTime = TimeSpan.Parse(row["start_time"].ToString()!).ToString(@"hh\:mm\:ss"),
                EndTime = TimeSpan.Parse(row["end_time"].ToString()!).ToString(@"hh\:mm\:ss"),
                Room = row["room"] != DBNull.Value ? row["room"].ToString() : null
            };
        }
    }
}
