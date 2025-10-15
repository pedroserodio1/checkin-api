namespace Checkin.Api.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;
        public bool CheckedIn { get; set; } = false;

        //relacionamentos
        public Event? Event { get; set; } = null;
        public User? User { get; set; } = null;
    }
}