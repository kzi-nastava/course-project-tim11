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
                        CreateMovementMenu();
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
        public static void CreateMovementMenu()
        {
            CLI.CLIWriteLine("Enter ID of equipment to change:");
            int id = CLI.GetValidEquipmentId();
            CLI.CLIWriteLine("Enter amount to move");
            int amount = CLI.CLIEnterNumberWithLimit(1, 100);
            CLI.CLIWriteLine("Enter the Id of the room where the equipment is going to");
            int roomId = CLI.GetValidRoomId();
            CLI.CLIWriteLine("Enter date on which the equipment is being moved");
            DateTime date = CLI.CLIEnterDate();

            EquipmentMovementService.MoveEquipment(id, amount, roomId, date);
        }
    }
}
