using ClinicApp.AdminFunctions;
using System;
using ClinicApp;

namespace ClinicApp.AdminFunctions
{
    public static class RoomService
    {
        
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
            CLI.CLIWriteLine("ID | NAME | TYPE");
            foreach (Room room in RoomRepository.Rooms)
            {
                CLI.CLIWriteLine(room.Id + " " + room.Name + " " + room.Type);
            }
        }
        public static void AddNewRoom()
        {
            CLI.CLIWriteLine("Enter name: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            CLI.CLIWriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = ChooseRoomType();
            Room room = new Room { Name = name, Type = roomType };
            RoomRepository.Add(room);
        }
        public static void EditRoom()
        {
            Room room;
            string name;
            RoomType roomType;
            int id = CLI.GetValidRoomId();
            room = RoomRepository.Get(id);
            if (room.Id == 0)
            {
                CLI.CLIWriteLine("You cannot edit Storage!");
                return;
            }
            CLI.CLIWriteLine("Do you wish to edit this rooms' name? Y/N");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                CLI.CLIWriteLine("Enter new name: ");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            else name = room.Name;
            CLI.CLIWriteLine("Do you wish to edit this rooms' type? Y/N");
            answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                CLI.CLIWriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
                roomType = ChooseRoomType();
            }
            else roomType = room.Type;
            RoomRepository.Update(room.Id, name, roomType);
        }
        public static void DeleteRoom()
        {
            CLI.CLIWriteLine("Enter ID of the room you want to Delete");
            int id = CLI.GetValidRoomId();
            if (id == 0)
            {
                CLI.CLIWriteLine("You cannot delete Storage!");
            }
            else RoomRepository.Delete(id);
        }

    }
}
