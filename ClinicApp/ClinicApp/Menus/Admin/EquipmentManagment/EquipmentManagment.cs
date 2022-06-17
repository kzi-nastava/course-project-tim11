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
                CLI.CLIWriteLine("Manage Equipment");
                CLI.CLIWriteLine("1. List all");
                CLI.CLIWriteLine("2. Search");
                CLI.CLIWriteLine("3. Move equipment");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 3);
                switch (choice)
                {
                    case 1:
                        ListAllEquipment();
                        break;
                    case 2:
                        SearchEquipment.Dialog();
                        break;
                    case 3:
                        MoveEquipment.Menu();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ListAllEquipment()
        {
            Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
            foreach (Equipment eq in EquipmentRepository.EquipmentList)
            {
                CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepository.Get(eq.RoomId).Name + " " + RoomRepository.Get(eq.RoomId).Type + " " + eq.Type);
            }
        }
        public static void ListAllEquipmentInRoom(int id)
        {
            Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
            foreach (Equipment eq in EquipmentRepository.EquipmentList)
            {
                if (eq.RoomId == id)
                    CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepository.Get(eq.RoomId).Name + " " + RoomRepository.Get(eq.RoomId).Type + " " + eq.Type);
            }
        }
    }
}
