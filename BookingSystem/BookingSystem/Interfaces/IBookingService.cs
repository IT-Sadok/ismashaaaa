using BookingSystem.Models;

namespace BookingSystem.Interfaces
{
    public interface IBookingService
    {
        void AddRoom(Room room);

        List<Room> GetAvailableRoomsByDate(DateTime date);

        void MakeReservation(Guid roomId, DateTime date);

        void CancelReservation(Guid reservationId);

    }
}
