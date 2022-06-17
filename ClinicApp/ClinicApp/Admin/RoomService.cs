using ClinicApp.AdminFunctions;
using System;
using ClinicApp;

namespace ClinicApp.AdminFunctions
{
    public static class RoomService
    {
        
        
        public static void ListAllRooms()
        {
            Console.WriteLine("ID | NAME | TYPE");
            foreach (Room room in RoomRepository.Rooms)
            {
                Console.WriteLine(room.Id + " " + room.Name + " " + room.Type);
            }
        }
        public static void AddNewRoom(string name, RoomType roomType)
        {
            Room room = new Room { Name = name, Type = roomType };
            RoomRepository.Add(room);
        }
        public static void EditRoom(int id, string name,RoomType roomType)
        {
            RoomRepository.Update(id, name, roomType);
        }
        public static void DeleteRoom(int id)
        {
            RoomRepository.Delete(id);
        }

    }
}
