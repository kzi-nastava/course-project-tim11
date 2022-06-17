using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;
using ClinicApp.Clinic.Equipments;
using ClinicApp.Clinic.Rooms;
using ClinicApp.Clinic.Search;

namespace ClinicApp.Menus.Admin.EquipmentManagment
{
    class SearchEquipment
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Search");
            EquipmentSearch STerms = new EquipmentSearch();
            CLI.CLIWrite("Enter search terms: ");
            STerms.SearchTerm = CLI.CLIEnterString();
            AskToFilterByEq(STerms);
            AskToFilterByRoom(STerms);
            AskToFilterByAmount(STerms);
            List <Equipment> results = EquipmentSearchService.Search(STerms);
            PrintResults(results);
        }
        static void AskToFilterByEq(EquipmentSearch STerms)
        {
            CLI.CLIWriteLine("\nFilter by Equipment Type? (y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByEqTypeBool = true;
                CLI.CLIWriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
                STerms.FilterByEq = OtherFunctions.ChooseEquipmentType();
            }
        }
        static void AskToFilterByRoom(EquipmentSearch STerms)
        {
            CLI.CLIWriteLine("Filter by room type? (y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByRoomTypeBool = true;
                CLI.CLIWriteLine("Choose!\n1. Operations\n2. Waiting\n3. STORAGE\n4. Examinations");
                STerms.FilterByRoom = OtherFunctions.ChooseRoomType();
            }
        }
        static void AskToFilterByAmount(EquipmentSearch STerms)
        {
            CLI.CLIWriteLine("Filter by amount?(y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByAmountBool = true;
                CLI.CLIWriteLine("Choose!\n1. 0\n2. 1-10\n3. 10+");
                int amount = CLI.CLIEnterNumberWithLimit(1, 3);
                STerms.Amount = amount;
            }
        }
        static void PrintResults(List<Equipment> results)
        {
            CLI.CLIWriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE | DYNAMIC");
            foreach (Equipment eq in results)
            {
                CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepository.Get(eq.RoomId).Name + " " + RoomRepository.Get(eq.RoomId).Type + " " + eq.Type + " " + eq.Dynamic);
            }
        }
    }
}
