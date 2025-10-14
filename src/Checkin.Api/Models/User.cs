using System.ComponentModel.DataAnnotations.Schema;
using Checkin.Api.Enums;

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
        public UserRole Role { get; set; } = UserRole.Participant; // Default role
        public string Salt { get; set; } = string.Empty;

        //relacionamentos
        public ICollection<Event> Events { get; set; } = new List<Event>();

        //entidades nao mapeadas
        [NotMapped]
        public string Password { get; set; } = string.Empty;

    }
}