using Azure.Core;
using examService.DTOs;
using examService.DTOs.Exams;
using examService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace examService.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamsController(IExamService examService) : ControllerBase
    {
        private readonly IExamService _examService = examService;
        [HttpPost]
        public ActionResult<ApiResponse<CreateExamResponse, object?>> CreateExam([FromBody] CreateExamRequest createExamRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new ApiResponse<CreateExamResponse, object?>(
                    success: false,
                    message: $"Dữ liệu không hợp lệ: {errorMessage}",
                    data: null
                ));
            }

            try
            {
                Guid newExamId = _examService.CreateExam(createExamRequest);

                var responseData = new CreateExamResponse
                {
                    ExamId = newExamId,
                    Message = "Đã lưu đề thi và các phần thi vào hệ thống."
                };

                return Ok(new ApiResponse<CreateExamResponse, object?>(
                    success: true,
                    message: "Tạo đề thi thành công!",
                    data: responseData,
                    metaData: null
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<CreateExamResponse, object?>(
                    success: false,
                    message: ex.Message,
                    data: null,
                    metaData: null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CreateExamResponse, object?>(
                    success: false,
                    message: $"Lỗi hệ thống: {ex.Message}",
                    data: null,
                    metaData: null
                ));
            }

        }

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<UpdateExamResponse, object?>> UpdateExam(Guid id, [FromBody] UpdateExamRequest updateExamRequest)
        {
            try
            {
                var responseData = _examService.UpdateExam(id, updateExamRequest);

                return Ok(new ApiResponse<UpdateExamResponse, object?>(
                    success: true,
                    message: "Cập nhật đề thi thành công.",
                    data: responseData, 
                    metaData: null
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<UpdateExamResponse, object?>(
                    success: false, 
                    message: ex.Message, 
                    data: null, 
                    metaData: null
                    ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<UpdateExamResponse, object?>(
                    success: false, 
                    message: ex.Message, 
                    data: null, 
                    metaData: null
                    ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UpdateExamResponse, object?>(
                    success: false, 
                    message: "Đã xảy ra lỗi hệ thống: " + ex.Message, 
                    data: null, 
                    metaData: null
                    ));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object?, object?>> DeleteExam(Guid id)
        {
            try
            {
                _examService.DeleteExam(id);

                return Ok(new ApiResponse<object?, object?>(
                    success: true,
                    message: "Xóa đề thi thành công.",
                    data: null,
                    metaData: null
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object?, object?>(
                    success: false,
                    message: ex.Message,
                    data: null,
                    metaData: null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object?, object?>(
                    success: false,
                    message: "Không thể xóa đề thi (Lỗi hệ thống hoặc ràng buộc dữ liệu): " + ex.Message,
                    data: null,
                    metaData: null
                ));
            }
        }
    }
}
