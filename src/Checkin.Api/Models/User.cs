using System.ComponentModel.DataAnnotations.Schema;

namespace Checkin.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Default role
        public string Salt { get; set; } = string.Empty;

        //entidades nao mapeadas
        [NotMapped]
        public string Password { get; set; } = string.Empty;

    }
}