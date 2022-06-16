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
                CLI.CLIWriteLine("Manage Rooms");
                CLI.CLIWriteLine("1. List all rooms");
                CLI.CLIWriteLine("2. Add new room");
                CLI.CLIWriteLine("3. Edit an existing room");
                CLI.CLIWriteLine("4. Delete a room by ID");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 4);
                switch (choice)
                {
                    case 1:
                        RoomService.ListAllRooms();
                        break;
                    case 2:
                        AddNewRoom.Dialog();
                        break;
                    case 3:
                        EditRoom.Dialog();
                        break;
                    case 4:
                        DeleteRoom.Dialog();
                        break;
                    case 0:
                        return;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
