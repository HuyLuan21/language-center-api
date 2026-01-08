using System.Data;
using enrollmentService.DTOs;
using enrollmentService.Repositories;
namespace enrollmentService.Services
{
    public class EnrollmentService
    {
        private EnrollmentRepository _enrollmentRepository;
        public EnrollmentService(EnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public GetStudentEnrollmentResponse GetStudentEnrollment(Guid studentId)
        {
            DataTable enrollmentData = _enrollmentRepository.GetEnrollmentsByStudentId(studentId);
            if (enrollmentData.Rows.Count == 0)
            {
                throw new Exception("No enrollment found for the given student ID.");
            }
            var enrollments = new List<EnrollmentDTO>();
            foreach (DataRow row in enrollmentData.Rows)
            {
                var enrollment = new EnrollmentDTO
                {
                    EnrollmentId = Guid.Parse(row["enrollment_id"].ToString()!),
                    StudentId = Guid.Parse(row["student_id"].ToString()!),
                    ClassId = Guid.Parse(row["class_id"].ToString()!),
                    EnrollmentDate = DateTime.Parse(row["enrollment_date"].ToString()!),
                    EnrollmentStatus = row["enrollment_status"].ToString()!
                };
                enrollments.Add(enrollment);
            }
            return new GetStudentEnrollmentResponse
            {
                StudentId = studentId,
                studentEnrollments = enrollments
            };
        }

        public EnrollmentDetailDTO GetEnrollmentDetail(Guid enrollmentId)
        {
            DataTable enrollmentDetailData = _enrollmentRepository.GetEnrollmentDetail(enrollmentId);
            if (enrollmentDetailData.Rows.Count == 0)
            {
                throw new Exception("No enrollment detail found for the given enrollment ID.");
            }
            DataRow row = enrollmentDetailData.Rows[0];
            var enrollmentDetail = new EnrollmentDetailDTO
            {
                // ===== Enrollment =====
                EnrollmentId = Guid.Parse(row["enrollment_id"].ToString()!),
                EnrollmentDate = DateTime.Parse(row["enrollment_date"].ToString()!),
                EnrollmentStatus = row["enrollment_status"].ToString()!,
                // ===== Student =====
                StudentId = Guid.Parse(row["student_id"].ToString()!),
                UserId = Guid.Parse(row["user_id"].ToString()!),
                StudentName = row["StudentName"].ToString()!,
                Email = row["email"].ToString()!,
                Phone = row["phone"].ToString()!,
                // ===== Class =====
                ClassId = Guid.Parse(row["class_id"].ToString()!),
                ClassName = row["class_name"].ToString()!,
                StartDate = DateTime.Parse(row["start_date"].ToString()!),
                EndDate = DateTime.Parse(row["end_date"].ToString()!),
                MaxStudents = int.Parse(row["max_students"].ToString()!),
                ClassStatus = row["class_status"].ToString()!,
                // ===== Course =====
                CourseId = Guid.Parse(row["course_id"].ToString()!),
                CourseName = row["course_name"].ToString()!,
                DurationHours = int.Parse(row["duration_hours"].ToString()!),
                Fee = decimal.Parse(row["fee"].ToString()!),
                // ===== Language =====
                LanguageCode = row["language_code"].ToString()!,
                LanguageName = row["language_name"].ToString()!,
                LevelName = row["level_name"].ToString()!,
                // ===== Teacher =====
                TeacherId = Guid.Parse(row["teacher_id"].ToString()!),
                TeacherName = row["TeacherName"].ToString()!,
                Specialization = row["specialization"].ToString()!,
                YearsOfExperience = int.Parse(row["years_of_experience"].ToString()!),
                // ===== Invoice (optional)
                InvoiceId = row["invoice_id"] != DBNull.Value ? Guid.Parse(row["invoice_id"].ToString()!) : null,
                InvoiceAmount = row["InvoiceAmount"] != DBNull.Value ? decimal.Parse(row["InvoiceAmount"].ToString()!) : null,
                InvoiceStatus = row["invoice_status"] != DBNull.Value ? row["invoice_status"].ToString()! : string.Empty
            };
            return enrollmentDetail;
        }

        public bool CreateEnrollment(EnrollmentDTO enrollment)
        {
            enrollment.EnrollmentId = Guid.NewGuid();
            return _enrollmentRepository.CreateEnrollment(enrollment);
        }

        public bool UpdateEnrollmentStatus(Guid enrollmentId, string newStatus)
        {
            return _enrollmentRepository.UpdateEnrollmentStatus(enrollmentId, newStatus);
        }

        public bool DeleteEnrollment(Guid enrollmentId)
        {
            return _enrollmentRepository.DeleteEnrollment(enrollmentId);
        }

        public CourseListResponse GetCourseList()
        {
            CourseListResponse response = new CourseListResponse();
            DataTable courseData = _enrollmentRepository.GetAllCourse();
            var courses = new List<CourseDTO>();
            foreach (DataRow row in courseData.Rows)
            {
                var course = new CourseDTO
                {
                    CourseId = Guid.Parse(row["course_id"].ToString()!),
                    CourseName = row["course_name"].ToString()!,
                    LanguageCode = row["language_code"].ToString()!,
                    LanguageName = row["language_name"].ToString()!,
                    LevelName = row["level_name"].ToString()!,
                    DurationHours = int.Parse(row["duration_hours"].ToString()!),
                    Fee = decimal.Parse(row["fee"].ToString()!),
                    ThumbnailUrl = row["thumbnail_url"].ToString()!,
                    CourseStatus = row["course_status"].ToString()!
                };
                courses.Add(course);
            }
            response.Courses = courses;
            return response;

        }
    }
}
