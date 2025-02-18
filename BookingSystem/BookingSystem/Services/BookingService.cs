using BookingSystem.Exceptions;
using BookingSystem.Interfaces;
using BookingSystem.Models;

namespace BookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDataService _dataService;

        public BookingService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void AddRoom(Room room)
        {
            room.Id = Guid.NewGuid();
            _dataService.SaveRoom(room);
        }

        public List<Room> GetAvailableRoomsByDate(DateTime date)
        {
            var rooms = _dataService.LoadAllRooms();
            var reservations = _dataService.LoadAllReservation();

            return rooms
                .Where(room => room.AvailableDates.Contains(date)
                    && !reservations.Any(reservation => reservation.RoomId == room.Id
                    && reservation.Date == date))
                .ToList();
        }

        public void MakeReservation(Guid roomId, DateTime date)
        {
            var room = _dataService.LoadAllRooms().FirstOrDefault(room => room.Id == roomId);
            if (room == null)
            {
                throw new BookingException("Room not found.");
            }

            if (!room.AvailableDates.Contains(date))
            {
                throw new BookingException("Room is not available on this date.");
            }

            var reservation = _dataService.LoadAllReservation().FirstOrDefault(room => room.RoomId == roomId && room.Date == date);
            if (reservation != null)
            {
                throw new BookingException("Room is already reserved on this date.");
            }

            var newReservation = new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                Date = date
            };

            _dataService.SaveReservation(newReservation);
        }

        public void CancelReservation(Guid reservationId)
        {
            var reservationToCancel = _dataService.LoadAllReservation().FirstOrDefault(room => room.Id == reservationId);

            if (reservationToCancel == null)
            {
                throw new BookingException("Reservation not found.");
            }

            _dataService.RemoveReservation(reservationToCancel);
        }
    }
}
