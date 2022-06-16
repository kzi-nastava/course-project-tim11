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
                        ListAllEquipment();
                        break;
                    case 2:
                        SearchEquipmentMenu();
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
                        AddEqToStorage();
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
        public static void SearchEquipmentMenu()
        {
            CLI.CLIWriteLine("Search");
            EquipmentSearch STerms = new EquipmentSearch();
            CLI.CLIWrite("Enter search terms: ");
            STerms.SearchTerm = CLI.CLIEnterString();
            CLI.CLIWriteLine("\nFilter by Equipment Type? (y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByEqTypeBool = true;
                CLI.CLIWriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
                STerms.FilterByEq = OtherFunctions.ChooseEquipmentType();
            }
            CLI.CLIWriteLine("Filter by room type? (y/n): ");
            answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByRoomTypeBool = true;
                CLI.CLIWriteLine("Choose!\n1. Operations\n2. Waiting\n3. STORAGE\n4. Examinations");
                STerms.FilterByRoom = OtherFunctions.ChooseRoomType();
            }
            CLI.CLIWriteLine("Filter by amount?(y/n): ");
            answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByAmountBool = true;
                CLI.CLIWriteLine("Choose!\n1. 0\n2. 1-10\n3. 10+");
                int amount = CLI.CLIEnterNumberWithLimit(1, 3);
                STerms.Amount = amount;
            }
            List<Equipment> results = EquipmentSearchService.Search(STerms);
            CLI.CLIWriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE | DYNAMIC");
            foreach (Equipment eq in results)
            {
                CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepository.Get(eq.RoomId).Name + " " + RoomRepository.Get(eq.RoomId).Type + " " + eq.Type + " " + eq.Dynamic);
            }
        }

        public static void StorageSubmenu()
        {
            CLI.CLIWriteLine("1. Add new Equipment");
            CLI.CLIWriteLine("2. Edit amount of existing Equipment");
            int answer = CLI.CLIEnterNumberWithLimit(0, 2);
            switch (answer)
            {
                case 1:
                    AddNewToStorage();
                    break;
                case 2:
                    EditExisting();
                    break;
                case 0:
                    return;
            }
        }
        public static void AddEqToStorage()
        {
            CLI.CLIWriteLine("List Equipment in Storage? (y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                ListAllEquipmentInRoom(0);
            }
            StorageSubmenu();
        }
        public static void EditExisting()
        {
            CLI.CLIWriteLine("Enter ID of equipment to change:");
            int id = CLI.GetValidEquipmentId();
            Equipment eq = EquipmentRepository.Get(id);
            if (eq.RoomId != 0)
            {
                CLI.CLIWriteLine("Equipment not in Storage cannot be edited directly, use the option 1. in the Manage Equipment menu");
                return;
            }
            else
            {
                CLI.CLIWriteLine("Enter new amount: ");
                int amount = CLI.CLIEnterNumberWithLimit(1, 99999999);
                EquipmentRepository.Update(eq.Id, amount);
            }
        }
        public static void AddNewToStorage()
        {
            List<string> exsistingNames = new List<string>();
            foreach (Equipment item in EquipmentRepository.EquipmentList)
            {
                if (item.RoomId == 0)
                {
                    exsistingNames.Add(item.Name);
                }
            }
            CLI.CLIWrite("Name: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (exsistingNames.Contains(name))
            {
                CLI.CLIWrite("Name already in use! Enter name: ");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            CLI.CLIWriteLine("Enter amount");
            int amount = CLI.CLIEnterNumber();
            CLI.CLIWriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
            EquipmentType type = OtherFunctions.ChooseEquipmentType();
            EquipmentService.AddNew(name, amount, type);
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
