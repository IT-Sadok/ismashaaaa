using System;

namespace BookingSystem.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public Room? Room { get; set; }

        public DateTime Date { get; set; }
    }
}
