using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class RoomRenovation
    {
        public static void Menu()
        {
            while (true)
            {
                RoomRenovationService.CheckForRenovations();
                CLI.CLIWriteLine("Room Renovation Menu");
                CLI.CLIWriteLine("1. Simple Renovation");
                CLI.CLIWriteLine("2. Complex Renovation");
                CLI.CLIWriteLine("3. List all Renovations");
                CLI.CLIWriteLine("0. Return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 3);
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        RoomRenovationService.SimpleRoomRenovation();
                        break;
                    case 2:
                        ComplexRoomRenovationMenu();
                        break;
                    case 3:
                        RoomRenovationService.ListAllRenovations();
                        break;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ComplexRoomRenovationMenu()
        {

            CLI.CLIWriteLine("1. Split room");
            CLI.CLIWriteLine("2. Join 2 rooms");
            CLI.CLIWriteLine("0. Return");
            int answer = CLI.CLIEnterNumberWithLimit(0, 2);
            switch (answer)
            {
                case 0:
                    return;
                case 1:
                    RoomRenovationService.CreateComplexSplitRenovation();
                    return;
                case 2:
                    RoomRenovationService.CreateComplexJoinRenovation();
                    return;
                default:
                    CLI.CLIWriteLine("Invalid option, try again");
                    break;
            }
        }
    }
}
