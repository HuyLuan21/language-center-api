namespace courseService.Models
{
    public class Classes
    {
        public Guid class_id { get; set; }
        public Guid course_id { get; set; }
        public Guid teacher_id { get; set; }
        public string class_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int max_students { get; set; }
        public string class_status { get; set; } = "scheduled";
    }
}
