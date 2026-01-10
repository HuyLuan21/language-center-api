using scheduleService.Models;
using scheduleService.Repositories.Interfaces;
using System.Data;

namespace scheduleService.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly DbContext _dbContext;

        public ScheduleRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataTable GetAllSchedules()
        {
            string query = @"
                SELECT schedule_id, class_id, study_date, start_time, end_time, room
                FROM Schedules
                ORDER BY study_date DESC, start_time ASC";

            return _dbContext.ExecuteQuery(query);
        }

        public DataTable GetScheduleById(Guid scheduleId)
        {
            string query = $@"
                SELECT schedule_id, class_id, study_date, start_time, end_time, room
                FROM Schedules
                WHERE schedule_id = '{scheduleId}'";

            return _dbContext.ExecuteQuery(query);
        }

        public DataTable GetSchedulesByClassId(Guid classId)
        {
            string query = $@"
                SELECT schedule_id, class_id, study_date, start_time, end_time, room
                FROM Schedules
                WHERE class_id = '{classId}'
                ORDER BY study_date ASC, start_time ASC";

            return _dbContext.ExecuteQuery(query);
        }

        public DataTable GetSchedulesByDateRange(DateTime startDate, DateTime endDate)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@StartDate", startDate.Date },
                { "@EndDate", endDate.Date }
            };

            string query = @"
                SELECT schedule_id, class_id, study_date, start_time, end_time, room
                FROM Schedules
                WHERE study_date BETWEEN @StartDate AND @EndDate
                ORDER BY study_date ASC, start_time ASC";

            return _dbContext.ExecuteQueryWithParameters(query, parameters);
        }

        public DataTable GetScheduleDetailById(Guid scheduleId)
        {
            string query = $@"
                SELECT 
                    s.schedule_id,
                    s.class_id,
                    cl.class_name,
                    c.course_id,
                    c.course_name,
                    s.study_date,
                    s.start_time,
                    s.end_time,
                    s.room
                FROM Schedules s
                INNER JOIN Classes cl ON s.class_id = cl.class_id
                INNER JOIN Courses c ON cl.course_id = c.course_id
                WHERE s.schedule_id = '{scheduleId}'";

            return _dbContext.ExecuteQuery(query);
        }

        public DataTable GetScheduleDetailsByClassId(Guid classId)
        {
            string query = $@"
                SELECT 
                    s.schedule_id,
                    s.class_id,
                    cl.class_name,
                    c.course_id,
                    c.course_name,
                    s.study_date,
                    s.start_time,
                    s.end_time,
                    s.room
                FROM Schedules s
                INNER JOIN Classes cl ON s.class_id = cl.class_id
                INNER JOIN Courses c ON cl.course_id = c.course_id
                WHERE s.class_id = '{classId}'
                ORDER BY s.study_date ASC, s.start_time ASC";

            return _dbContext.ExecuteQuery(query);
        }

        public bool CreateSchedule(Schedule schedule)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ScheduleId", schedule.ScheduleId },
                { "@ClassId", schedule.ClassId },
                { "@StudyDate", schedule.StudyDate.Date },
                { "@StartTime", schedule.StartTime },
                { "@EndTime", schedule.EndTime },
                { "@Room", (object?)schedule.Room ?? DBNull.Value }
            };

            string query = @"
                INSERT INTO Schedules (schedule_id, class_id, study_date, start_time, end_time, room)
                VALUES (@ScheduleId, @ClassId, @StudyDate, @StartTime, @EndTime, @Room)";

            int rowsAffected = _dbContext.ExecuteNonQueryWithParameters(query, parameters);
            return rowsAffected > 0;
        }

        public bool UpdateSchedule(Guid scheduleId, Schedule schedule)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ScheduleId", scheduleId },
                { "@ClassId", schedule.ClassId },
                { "@StudyDate", schedule.StudyDate.Date },
                { "@StartTime", schedule.StartTime },
                { "@EndTime", schedule.EndTime },
                { "@Room", (object?)schedule.Room ?? DBNull.Value }
            };

            string query = @"
                UPDATE Schedules
                SET class_id = @ClassId,
                    study_date = @StudyDate,
                    start_time = @StartTime,
                    end_time = @EndTime,
                    room = @Room
                WHERE schedule_id = @ScheduleId";

            int rowsAffected = _dbContext.ExecuteNonQueryWithParameters(query, parameters);
            return rowsAffected > 0;
        }

        public bool DeleteSchedule(Guid scheduleId)
        {
            string query = $@"
                DELETE FROM Schedules
                WHERE schedule_id = '{scheduleId}'";

            int rowsAffected = _dbContext.ExecuteNonQuery(query);
            return rowsAffected > 0;
        }
    }
}
