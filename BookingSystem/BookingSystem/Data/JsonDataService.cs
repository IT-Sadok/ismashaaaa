using BookingSystem.Interfaces;
using BookingSystem.Models;
using System.Text.Json;

namespace BookingSystem.Data
{
    public class JsonDataService : IDataService
    {
        private const string RoomsFilePath = "rooms.json";
        private const string ReservationsFilePath = "reservations.json";

        public List<Room> LoadAllRooms()
        {
            return LoadDataFromFile<Room>(RoomsFilePath);

        }

        public List<Reservation> LoadAllReservation()
        {
            return LoadDataFromFile<Reservation>(ReservationsFilePath);
        }

        public void SaveRoom(Room room)
        {
            SaveDataToFile(RoomsFilePath, room);
        }

        public void SaveReservation(Reservation reservation)
        {
            SaveDataToFile(ReservationsFilePath, reservation);
        }

        public void RemoveReservation(Reservation reservation)
        {
            var currentReservations = LoadAllReservation();
            currentReservations.Remove(reservation);
            SaveDataToFile(ReservationsFilePath, currentReservations);
        }

        private static List<T> LoadDataFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();

            using var streamReader = new StreamReader(filePath);
            var content = streamReader.ReadToEnd();

            try
            {
                var singleItem = JsonSerializer.Deserialize<T>(content);
                return singleItem != null ? new List<T> { singleItem } : new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        private static void SaveDataToFile<T>(string filePath, T content)
        {
            using var streamWriter = new StreamWriter(filePath);

            streamWriter.Write(JsonSerializer.Serialize(content));
        }
    }
}
