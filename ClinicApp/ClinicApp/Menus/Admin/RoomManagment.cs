using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class RoomManagment
    {
        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Manage Rooms");
                Console.WriteLine("1. List all rooms");
                Console.WriteLine("2. Add new room");
                Console.WriteLine("3. Edit an existing room");
                Console.WriteLine("4. Delete a room by ID");
                Console.WriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 4);
                switch (choice)
                {
                    case 1:
                        RoomService.ListAllRooms();
                        break;
                    case 2:
                        RoomService.AddNewRoom();
                        break;
                    case 3:
                        RoomService.EditRoom();
                        break;
                    case 4:
                        RoomService.DeleteRoom();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
