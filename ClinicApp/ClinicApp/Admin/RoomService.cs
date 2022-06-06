using ClinicApp.AdminFunctions;
using System;
using ClinicApp;

public static class RoomService {

    public static void ListAllRooms()
    {
        CLI.CLIWriteLine("ID | NAME | TYPE");
        foreach (Room room in RoomRepo.ClinicRooms)
        {
            CLI.CLIWriteLine(room.Id + " " + room.Name + " " + room.Type);
        }
    }
    public static void AddNewRoom()
    {
        CLI.CLIWriteLine("Enter name: ");
        string name = CLI.CLIEnterStringWithoutDelimiter("|");
        CLI.CLIWriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
        RoomType roomType = OtherFunctions.ChooseRoomType();
        Room room = new Room { Name = name, Type = roomType };
        RoomRepo.Add(room);
    }
    public static void EditRoom()
    {
        Room room;
        string name;
        RoomType roomType;
        int id = OtherFunctions.GetValidRoomId();
        room = RoomRepo.Get(id);
        if (room.Id == 0)
        {
            Console.WriteLine("You cannot edit Storage!");
            return;
        }
        Console.WriteLine("Do you wish to edit this rooms' name? Y/N");
        string answer = OtherFunctions.EnterString();
        if (answer.ToLower() == "y")
        {
            Console.WriteLine("Enter new name: ");
            name = OtherFunctions.EnterStringWithoutDelimiter("|");
        }
        else name = room.Name;
        Console.WriteLine("Do you wish to edit this rooms' type? Y/N");
        answer = OtherFunctions.EnterString();
        if (answer.ToLower() == "y")
        {
            Console.WriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            roomType = OtherFunctions.ChooseRoomType();
        }
        else roomType = room.Type;
        RoomRepo.Update(room.Id, name, roomType);
    }
    public static void DeleteRoom()
    {
        Console.WriteLine("Enter ID of the room you want to Delete");
        int id = OtherFunctions.GetValidRoomId();
        if (id == 0)
        {
            Console.WriteLine("You cannot delete Storage!");
        }
        else RoomRepo.Delete(id);
    }

}