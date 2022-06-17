using ClinicApp.AdminFunctions;
using System;
using ClinicApp;

namespace ClinicApp.AdminFunctions
{
    public static class RoomService
    {
        public static int GetValidRoomId()
        {
            Room room;
            int id = CLI.CLIEnterNumber();
            room = RoomRepository.Get(id);
            while (room is null)
            {
                Console.WriteLine("Invalid ID");
                id = CLI.CLIEnterNumber();
                room = RoomRepository.Get(id);
            }
            return id;
        }
        public static RoomType ChooseRoomType()
        {
            RoomType type;
            int input = CLI.CLIEnterNumberWithLimit(1, 3);
            switch (input)
            {
                case 1:
                    type = RoomType.Operations;
                    break;
                case 2:
                    type = RoomType.Examinations;
                    break;
                case 3:
                    type = RoomType.Waiting;
                    break;
                default:
                    type = RoomType.STORAGE;
                    break;
            }
            return type;
        }
        public static void ListAllRooms()
        {
            Console.WriteLine("ID | NAME | TYPE");
            foreach (Room room in RoomRepository.Rooms)
            {
                Console.WriteLine(room.Id + " " + room.Name + " " + room.Type);
            }
        }
        public static void AddNewRoom()
        {
            Console.WriteLine("Enter name: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            Console.WriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = ChooseRoomType();
            Room room = new Room { Name = name, Type = roomType };
            RoomRepository.Add(room);
        }
        public static void EditRoom()
        {
            Room room;
            string name;
            RoomType roomType;
            int id = GetValidRoomId();
            room = RoomRepository.Get(id);
            if (room.Id == 0)
            {
                Console.WriteLine("You cannot edit Storage!");
                return;
            }
            Console.WriteLine("Do you wish to edit this rooms' name? Y/N");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                Console.WriteLine("Enter new name: ");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            else name = room.Name;
            Console.WriteLine("Do you wish to edit this rooms' type? Y/N");
            answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                Console.WriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
                roomType = ChooseRoomType();
            }
            else roomType = room.Type;
            RoomRepository.Update(room.Id, name, roomType);
        }
        public static void DeleteRoom()
        {
            Console.WriteLine("Enter ID of the room you want to Delete");
            int id = GetValidRoomId();
            if (id == 0)
            {
                Console.WriteLine("You cannot delete Storage!");
            }
            else RoomRepository.Delete(id);
        }

    }
}
