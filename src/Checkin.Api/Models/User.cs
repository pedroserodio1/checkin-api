using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Participant; // Default role
        [JsonIgnore]
        public string Salt { get; set; } = string.Empty;

        //relacionamentos
        [JsonIgnore]
        public ICollection<Event> Events { get; set; } = new List<Event>();
        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        //entidades nao mapeadas
        [NotMapped]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

    }
}