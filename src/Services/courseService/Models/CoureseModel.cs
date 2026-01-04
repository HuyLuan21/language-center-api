namespace courseService.Models
{
    public class Courese
    {
        public Guid course_id { get; set; }
        public string course_name { get; set; }
        public Guid language_level_id { get; set; }
        public string description { get; set; }
        public int duration_hours { get; set; }
        public decimal fee { get; set; }
        public string ? thumbnail_url { get; set; }
        public bool course_status { get; set; }

    }
}
