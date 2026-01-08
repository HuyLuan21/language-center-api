namespace enrollmentService.DTOs
{
    public class GetStudentEnrollmentResponse
    {
        public Guid StudentId { get; set; }
        public List<EnrollmentDTO>? studentEnrollments { get; set; }
    }


    public class EnrollmentDTO
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
    } 
}
