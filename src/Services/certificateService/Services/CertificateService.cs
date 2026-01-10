using System.Data;
using certificateService.DTOs;
using certificateService.Repositories;

namespace certificateService.Services
{
    public class CertificateService
    {
        private readonly CertificateRepository _certificateRepository;

        public CertificateService(CertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        public GetStudentCertificatesResponse GetCertificatesByStudentId(Guid studentId)
        {
            DataTable certificateData = _certificateRepository.GetCertificatesByStudentId(studentId);
            
            var certificates = new List<CertificateDTO>();
            foreach (DataRow row in certificateData.Rows)
            {
                var certificate = new CertificateDTO
                {
                    CertificateId = Guid.Parse(row["certificate_id"].ToString()!),
                    StudentId = Guid.Parse(row["student_id"].ToString()!),
                    CourseId = Guid.Parse(row["course_id"].ToString()!),
                    CourseName = row["course_name"].ToString()!,
                    LanguageName = row["language_name"].ToString()!,
                    LevelName = row["level_name"].ToString()!,
                    IssueDate = DateTime.Parse(row["issue_date"].ToString()!),
                    CertificateUrl = row["certificate_url"] != DBNull.Value 
                        ? row["certificate_url"].ToString()
                        : null,
                    CertificateStatus = row["certificate_status"].ToString()!
                };
                certificates.Add(certificate);
            }

            return new GetStudentCertificatesResponse
            {
                StudentId = studentId,
                Certificates = certificates
            };
        }

        public GetAllCertificatesResponse GetAllCertificates()
        {
            DataTable certificateData = _certificateRepository.GetAllCertificates();
            
            var certificates = new List<CertificateDTO>();
            foreach (DataRow row in certificateData.Rows)
            {
                var certificate = new CertificateDTO
                {
                    CertificateId = Guid.Parse(row["certificate_id"].ToString()!),
                    StudentId = Guid.Parse(row["student_id"].ToString()!),
                    CourseId = Guid.Parse(row["course_id"].ToString()!),
                    CourseName = row["course_name"].ToString()!,
                    LanguageName = row["language_name"].ToString()!,
                    LevelName = row["level_name"].ToString()!,
                    IssueDate = DateTime.Parse(row["issue_date"].ToString()!),
                    CertificateUrl = row["certificate_url"] != DBNull.Value 
                        ? row["certificate_url"].ToString()
                        : null,
                    CertificateStatus = row["certificate_status"].ToString()!,
                    StudentName = row["student_name"].ToString()
                };
                certificates.Add(certificate);
            }

            return new GetAllCertificatesResponse
            {
                Certificates = certificates,
                TotalCount = certificates.Count
            };
        }

        public CertificateDetailDTO GetCertificateById(Guid certificateId)
        {
            DataTable certificateData = _certificateRepository.GetCertificateById(certificateId);
            
            if (certificateData.Rows.Count == 0)
            {
                throw new Exception("Certificate not found");
            }

            DataRow row = certificateData.Rows[0];
            
            return new CertificateDetailDTO
            {
                CertificateId = Guid.Parse(row["certificate_id"].ToString()!),
                StudentId = Guid.Parse(row["student_id"].ToString()!),
                StudentName = row["student_name"].ToString()!,
                StudentEmail = row["student_email"] != DBNull.Value 
                    ? row["student_email"].ToString()
                    : null,
                StudentPhone = row["student_phone"] != DBNull.Value 
                    ? row["student_phone"].ToString()
                    : null,
                CourseId = Guid.Parse(row["course_id"].ToString()!),
                CourseName = row["course_name"].ToString()!,
                CourseDescription = row["course_description"] != DBNull.Value 
                    ? row["course_description"].ToString()
                    : null,
                DurationHours = int.Parse(row["duration_hours"].ToString()!),
                LanguageName = row["language_name"].ToString()!,
                LevelName = row["level_name"].ToString()!,
                LevelDescription = row["level_description"] != DBNull.Value 
                    ? row["level_description"].ToString()
                    : null,
                IssueDate = DateTime.Parse(row["issue_date"].ToString()!),
                CertificateUrl = row["certificate_url"] != DBNull.Value 
                    ? row["certificate_url"].ToString()
                    : null,
                CertificateStatus = row["certificate_status"].ToString()!
            };
        }
    }
}
