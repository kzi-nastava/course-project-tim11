using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class EquipmentManagment
    {
        public static void Menu()
        {
            while (true)
            {
                EquipmentMovementService.CheckForMovements();
                Console.WriteLine("Manage Equipment");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. Search");
                Console.WriteLine("3. Move equipment");
                Console.WriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 3);
                switch (choice)
                {
                    case 1:
                        EquipmentService.ListAllEquipment();
                        break;
                    case 2:
                        EquipmentSearchService.SearchEquipment();
                        break;
                    case 3:
                        MoveEquipmentMenu();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void MoveEquipmentMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Move Equipment");
                Console.WriteLine("2. Add new Equipment to Storage");
                Console.WriteLine("0 to return");
                int answer = CLI.CLIEnterNumberWithLimit(0, 2);
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        EquipmentMovementService.MoveEquipment();
                        break;
                    case 2:
                        EquipmentService.AddEqToStorage();
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
