using BookingSystem.Exceptions;
using BookingSystem.Interfaces;
using BookingSystem.Models;

namespace BookingSystem.Menu
{
    public static class BookingMenu
    {
        public static void DisplayMenu(IBookingService bookingService)
        {
            while (true)
            {
                Console.WriteLine("\nBooking System Menu:");
                Console.WriteLine("1. Add Room");
                Console.WriteLine("2. View Available Rooms");
                Console.WriteLine("3. Make a Reservation");
                Console.WriteLine("4. Cancel Reservation");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        bookingService.AddRoom(new Room 
                        { 
                            Name = "Standard Room", 
                            Price = 100, Description = "Amazing room", 
                            Type = Enums.RoomType.Deluxe, 
                            NumberOfGuests = 2, 
                            IsBreakfastIncluded = true, 
                            AvailableDates = new List<DateTime> { DateTime.Today } 
                        });
                        Console.WriteLine("Room added successfully.");
                        break;
                    case "2":
                        var availableRooms = bookingService.GetAvailableRoomsByDate(DateTime.Today);
                        Console.WriteLine("Available Rooms:");
                        foreach (var room in availableRooms)
                            Console.WriteLine($"{room.Id}: {room.Name}, {room.Price} USD, {room.Description}, {room.Type}, {room.NumberOfGuests}, {room.IsBreakfastIncluded}");
                        break;
                    case "3":
                        Console.Write("Enter Room ID to reserve: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid roomId))
                        {
                            try
                            {
                                bookingService.MakeReservation(roomId, DateTime.Today);
                                Console.WriteLine("Reservation made successfully.");
                            }
                            catch (BookingException exception)
                            {
                                Console.WriteLine($"Error: {exception.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Room ID.");
                        }
                        break;
                    case "4":
                        Console.Write("Enter Reservation ID to cancel: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid reservationId))
                        {
                            try
                            {
                                bookingService.CancelReservation(reservationId);
                                Console.WriteLine("Reservation canceled successfully.");
                            }
                            catch (BookingException exception)
                            {
                                Console.WriteLine($"Error: {exception.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Reservation ID.");
                        }
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
