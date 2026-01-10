using Microsoft.AspNetCore.Mvc;
using scheduleService.DTOs;
using scheduleService.Services.Interfaces;

namespace scheduleService.Controllers
{
    [ApiController]
    [Route("api/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Get all schedules
        /// </summary>
        [HttpGet]
        public ActionResult<ApiResponse<List<ScheduleDTO>, object?>> GetAllSchedules()
        {
            try
            {
                var schedules = _scheduleService.GetAllSchedules();
                return Ok(new ApiResponse<List<ScheduleDTO>, object?>(
                    true,
                    "Lấy danh sách lịch học thành công",
                    schedules,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ScheduleDTO>, object?>(
                    false,
                    "Lỗi khi lấy danh sách lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Get schedule by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ScheduleDTO, object?>> GetScheduleById(Guid id)
        {
            try
            {
                var schedule = _scheduleService.GetScheduleById(id);

                if (schedule == null)
                {
                    return NotFound(new ApiResponse<ScheduleDTO, object?>(
                        false,
                        "Không tìm thấy lịch học",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<ScheduleDTO, object?>(
                    true,
                    "Lấy lịch học thành công",
                    schedule,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ScheduleDTO, object?>(
                    false,
                    "Lỗi khi lấy lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Get schedules by class ID
        /// </summary>
        [HttpGet("class/{classId}")]
        public ActionResult<ApiResponse<List<ScheduleDTO>, object?>> GetSchedulesByClassId(Guid classId)
        {
            try
            {
                var schedules = _scheduleService.GetSchedulesByClassId(classId);
                return Ok(new ApiResponse<List<ScheduleDTO>, object?>(
                    true,
                    "Lấy danh sách lịch học theo lớp thành công",
                    schedules,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ScheduleDTO>, object?>(
                    false,
                    "Lỗi khi lấy danh sách lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Get schedules by date range
        /// </summary>
        [HttpGet("date-range")]
        public ActionResult<ApiResponse<List<ScheduleDTO>, object?>> GetSchedulesByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (endDate < startDate)
                {
                    return BadRequest(new ApiResponse<List<ScheduleDTO>, object?>(
                        false,
                        "Ngày kết thúc phải sau ngày bắt đầu",
                        null,
                        null
                    ));
                }

                var schedules = _scheduleService.GetSchedulesByDateRange(startDate, endDate);
                return Ok(new ApiResponse<List<ScheduleDTO>, object?>(
                    true,
                    "Lấy danh sách lịch học theo khoảng thời gian thành công",
                    schedules,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ScheduleDTO>, object?>(
                    false,
                    "Lỗi khi lấy danh sách lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Get schedule detail by ID (with class and course info)
        /// </summary>
        [HttpGet("{id}/detail")]
        public ActionResult<ApiResponse<ScheduleDetailDTO, object?>> GetScheduleDetailById(Guid id)
        {
            try
            {
                var schedule = _scheduleService.GetScheduleDetailById(id);

                if (schedule == null)
                {
                    return NotFound(new ApiResponse<ScheduleDetailDTO, object?>(
                        false,
                        "Không tìm thấy lịch học",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<ScheduleDetailDTO, object?>(
                    true,
                    "Lấy chi tiết lịch học thành công",
                    schedule,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ScheduleDetailDTO, object?>(
                    false,
                    "Lỗi khi lấy chi tiết lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Get schedule details by class ID (with class and course info)
        /// </summary>
        [HttpGet("class/{classId}/detail")]
        public ActionResult<ApiResponse<List<ScheduleDetailDTO>, object?>> GetScheduleDetailsByClassId(Guid classId)
        {
            try
            {
                var schedules = _scheduleService.GetScheduleDetailsByClassId(classId);
                return Ok(new ApiResponse<List<ScheduleDetailDTO>, object?>(
                    true,
                    "Lấy danh sách chi tiết lịch học theo lớp thành công",
                    schedules,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ScheduleDetailDTO>, object?>(
                    false,
                    "Lỗi khi lấy danh sách chi tiết lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Create a new schedule
        /// </summary>
        [HttpPost]
        public ActionResult<ApiResponse<ScheduleDTO, object?>> CreateSchedule([FromBody] CreateScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new ApiResponse<ScheduleDTO, object?>(
                    false,
                    "Dữ liệu không hợp lệ",
                    null,
                    new { errors }
                ));
            }

            try
            {
                var schedule = _scheduleService.CreateSchedule(request);

                if (schedule == null)
                {
                    return BadRequest(new ApiResponse<ScheduleDTO, object?>(
                        false,
                        "Không thể tạo lịch học",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<ScheduleDTO, object?>(
                    true,
                    "Tạo lịch học thành công",
                    schedule,
                    null
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<ScheduleDTO, object?>(
                    false,
                    ex.Message,
                    null,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ScheduleDTO, object?>(
                    false,
                    "Lỗi khi tạo lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Update an existing schedule
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<ScheduleDTO, object?>> UpdateSchedule(Guid id, [FromBody] UpdateScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new ApiResponse<ScheduleDTO, object?>(
                    false,
                    "Dữ liệu không hợp lệ",
                    null,
                    new { errors }
                ));
            }

            try
            {
                var schedule = _scheduleService.UpdateSchedule(id, request);

                if (schedule == null)
                {
                    return NotFound(new ApiResponse<ScheduleDTO, object?>(
                        false,
                        "Không tìm thấy lịch học để cập nhật",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<ScheduleDTO, object?>(
                    true,
                    "Cập nhật lịch học thành công",
                    schedule,
                    null
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<ScheduleDTO, object?>(
                    false,
                    ex.Message,
                    null,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ScheduleDTO, object?>(
                    false,
                    "Lỗi khi cập nhật lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }

        /// <summary>
        /// Delete a schedule
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object?, object?>> DeleteSchedule(Guid id)
        {
            try
            {
                bool isDeleted = _scheduleService.DeleteSchedule(id);

                if (!isDeleted)
                {
                    return NotFound(new ApiResponse<object?, object?>(
                        false,
                        "Không tìm thấy lịch học để xóa",
                        null,
                        null
                    ));
                }

                return Ok(new ApiResponse<object?, object?>(
                    true,
                    "Xóa lịch học thành công",
                    null,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object?, object?>(
                    false,
                    "Lỗi khi xóa lịch học",
                    null,
                    new { error = ex.Message }
                ));
            }
        }
    }
}
