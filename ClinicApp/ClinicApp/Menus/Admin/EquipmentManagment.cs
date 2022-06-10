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
                CLI.CLIWriteLine("Manage Equipment");
                CLI.CLIWriteLine("1. List all");
                CLI.CLIWriteLine("2. Search");
                CLI.CLIWriteLine("3. Move equipment");
                CLI.CLIWriteLine("0 to return");
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
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void MoveEquipmentMenu()
        {
            while (true)
            {
                CLI.CLIWriteLine("1. Move Equipment");
                CLI.CLIWriteLine("2. Add new Equipment to Storage");
                CLI.CLIWriteLine("0 to return");
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
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
