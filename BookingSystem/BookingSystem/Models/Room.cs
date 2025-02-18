using BookingSystem.Enums;

namespace BookingSystem.Models
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public RoomType Type { get; set; }

        public int NumberOfGuests { get; set; }

        public bool IsBreakfastIncluded { get; set; }

        public List<DateTime> AvailableDates { get; set; }

    }
}
