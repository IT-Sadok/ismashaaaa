using BookingSystem.Models;

namespace BookingSystem.Interfaces
{
    public interface IDataService
    {
        List <Room> LoadAllRooms();

        List <Reservation> LoadAllReservation();

        void SaveRoom(Room room);

        void SaveReservation(Reservation reservation);

        void RemoveReservation(Reservation reservation);
    }
}
