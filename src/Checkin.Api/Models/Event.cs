using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Checkin.Api.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateOnly Date { get; set; }       // só o dia
        public TimeOnly Time { get; set; }       // só a hora
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // já inicializa
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int OrganizerId { get; set; }

        //relacionamentos
        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; } = [];
        public User? Organizer { get; set; } = null;
    }
}