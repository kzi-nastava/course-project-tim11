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
                Console.WriteLine("Room Renovation Menu");
                Console.WriteLine("1. Simple Renovation");
                Console.WriteLine("2. Complex Renovation");
                Console.WriteLine("3. List all Renovations");
                Console.WriteLine("0. Return");
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
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ComplexRoomRenovationMenu()
        {

            Console.WriteLine("1. Split room");
            Console.WriteLine("2. Join 2 rooms");
            Console.WriteLine("0. Return");
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
                    Console.WriteLine("Invalid option, try again");
                    break;
            }
        }
    }
}
