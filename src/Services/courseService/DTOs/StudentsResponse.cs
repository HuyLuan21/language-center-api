namespace courseService.DTOs
{
    public class StudentsResponse
    {
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
