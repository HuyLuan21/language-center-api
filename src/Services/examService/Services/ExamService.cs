using examService.Services.Interfaces;
using examService.DTOs.Exams;
using examService.Repositories.Interfaces;
namespace examService.Services
{
    public class ExamService(IExamRepository examRepository) : IExamService
    {
        private readonly IExamRepository _examRepository = examRepository;

        public Guid CreateExam(CreateExamRequest request)
        {
            if (!TimeSpan.TryParse(request.StartTime, out TimeSpan startTime) ||
                !TimeSpan.TryParse(request.EndTime, out TimeSpan endTime))
            {
                throw new ArgumentException("Định dạng giờ không hợp lệ (HH:mm).");
            }

            if (endTime <= startTime)
            {
                throw new ArgumentException("Giờ kết thúc phải lớn hơn giờ bắt đầu.");
            }

            Guid newExamId = Guid.NewGuid();

            ExamDTO examDto = new ExamDTO
            {
                exam_id = newExamId,
                class_id = request.ClassId,
                exam_date = request.ExamDate,
                start_time = startTime, 
                end_time = endTime,
                room = request.Room,
                max_score = request.MaxScore,
                passing_score = request.PassingScore,
                weightage = request.Weightage
            };

            List<ExamPartDTO> partDtos = new List<ExamPartDTO>();

            if (request.ExamParts != null && request.ExamParts.Count > 0)
            {
                foreach (var partReq in request.ExamParts)
                {
                    partDtos.Add(new ExamPartDTO
                    {
                        exam_part_id = Guid.NewGuid(), 
                        exam_id = newExamId,          
                        part_name = partReq.PartName, 
                        max_score = partReq.MaxScore,
                        passing_score = partReq.PassingScore,
                        weightage = partReq.Weightage
                    });
                }
            }

            _examRepository.CreateExam(examDto, partDtos);

            return newExamId;
        }
        // Đổi void -> UpdateExamResponse
        public UpdateExamResponse UpdateExam(Guid id, UpdateExamRequest request)
        {
            if (!TimeSpan.TryParse(request.StartTime, out TimeSpan startTime) ||
                !TimeSpan.TryParse(request.EndTime, out TimeSpan endTime))
            {
                throw new ArgumentException("Định dạng giờ không hợp lệ.");
            }
            if (endTime <= startTime) throw new ArgumentException("Giờ kết thúc phải lớn hơn giờ bắt đầu.");

            // Map Exam cha
            ExamDTO examDto = new ExamDTO
            {
                exam_id = id,
                class_id = request.ClassId,
                exam_date = request.ExamDate,
                start_time = startTime,
                end_time = endTime,
                room = request.Room,
                max_score = request.MaxScore,
                passing_score = request.PassingScore,
                weightage = request.Weightage
            };

            List<ExamPartDTO> partDtos = new List<ExamPartDTO>();

            UpdateExamResponse response = new UpdateExamResponse
            {
                ExamId = id
            };

            if (request.ExamParts != null)
            {
                foreach (var partReq in request.ExamParts)
                {
                    Guid newPartId = Guid.NewGuid();

                    partDtos.Add(new ExamPartDTO
                    {
                        exam_part_id = newPartId,
                        exam_id = id,
                        part_name = partReq.PartName,
                        max_score = partReq.MaxScore,
                        passing_score = partReq.PassingScore,
                        weightage = partReq.Weightage
                    });

                    response.UpdatedParts.Add(new ExamPartResponse
                    {
                        ExamPartId = newPartId, // ID mới tinh
                        PartName = partReq.PartName,
                        MaxScore = partReq.MaxScore,
                        PassingScore = partReq.PassingScore ?? 0,
                        Weightage = partReq.Weightage
                    });
                }
            }

            bool isSuccess = _examRepository.UpdateExam(id, examDto, partDtos);

            if (!isSuccess)
            {
                throw new KeyNotFoundException($"Không tìm thấy đề thi với ID: {id}");
            }

            return response;
        }
        public void DeleteExam(Guid examId)
        {
            throw new NotImplementedException();
        }
    }
}
