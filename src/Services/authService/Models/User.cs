using authService.Models.Enums;

namespace authService.Models
{

    public class User
    {
        public Guid user_id { get; set; }

        public string username { get; set; } = null!;

        public string password_hash { get; set; } = null!;

        public string email { get; set; } = null!;

        public string? phone { get; set; }

        public string full_name { get; set; } = null!;

        public DateTime? date_of_birth { get; set; }

        public string? gender { get; set; }

        public string? address { get; set; }

        public UserRole role { get; set; } = UserRole.student;

        public string? avatar_url { get; set; }

        public bool is_active { get; set; }
    }

}
