using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class EquipmentSearchService
    {
        public static List<Equipment> Search(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            var results = new List<Equipment>();
            foreach (var item in EquipmentRepo.ClinicEquipmentList)
            {
                if (item.Name.ToLower().Contains(searchTerm) || item.Type.ToString().ToLower().Contains(searchTerm) || RoomRepo.Get(item.RoomId).Name.ToLower().Contains(searchTerm))
                {
                    results.Add(item);
                }
            }
            return results;
        }
        public static List<Equipment> FilterByEqType(List<Equipment> inputList, EquipmentType type)
        {
            var results = new List<Equipment>();
            foreach (var item in inputList)
            {
                if (item.Type == type)
                {
                    results.Add(item);
                }
            }
            return results;
        }
        public static List<Equipment> FilterByRoomType(List<Equipment> inputList, RoomType type)
        {
            var results = new List<Equipment>();
            foreach (var item in inputList)
            {
                if (RoomRepo.Get(item.RoomId).Type == type)
                {
                    results.Add(item);
                }
            }
            return results;
        }
        public static List<Equipment> FilterByNumbers(List<Equipment> inputList, int lowerBound, int upperBound)
        {
            var results = new List<Equipment>();
            foreach (var item in inputList)
            {
                if (item.Amount >= lowerBound && item.Amount <= upperBound)
                {
                    results.Add(item);
                }
            }
            return results;
        }
        public static void SearchEquipment()
        {
            CLI.CLIWriteLine("Search");
            EquipmentSearch STerms = new EquipmentSearch();
            List<Equipment> Results;
            CLI.CLIWrite("Enter search terms: ");
            STerms.SearchTerm = CLI.CLIEnterString();
            Results = Search(STerms.SearchTerm);
            CLI.CLIWriteLine("\nFilter by Equipment Type? (y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByEqTypeBool = true;
                CLI.CLIWriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
                STerms.FilterByEq = EquipmentService.ChooseEquipmentType();
            }
            CLI.CLIWriteLine("Filter by room type? (y/n): ");
            answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByRoomTypeBool = true;
                CLI.CLIWriteLine("Choose!\n1. Operations\n2. Waiting\n3. STORAGE\n4. Examinations");
                STerms.FilterByRoom = RoomService.ChooseRoomType();
            }


            CLI.CLIWriteLine("Filter by amount?(y/n): ");
            answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                STerms.FilterByAmountBool = true;
                CLI.CLIWriteLine("Choose!\n1. 0\n2. 1-10\n3. 10+");
                int amount = CLI.CLIEnterNumberWithLimit(1, 3);
                switch (amount)
                {
                    case 1:
                        STerms.Amount = 1;
                        break;
                    case 2:
                        STerms.Amount = 2;
                        break;
                    case 3:
                        STerms.Amount = 3;
                        break;
                }
            }
            if (STerms.FilterByEqTypeBool == true)
            {
                Results = FilterByEqType(Results, STerms.FilterByEq);
            }
            if (STerms.FilterByRoomTypeBool == true)
            {
                Results = FilterByRoomType(Results, STerms.FilterByRoom);
            }
            if (STerms.FilterByAmountBool == true)
            {
                switch (STerms.Amount)
                {
                    case 1:
                        Results = FilterByNumbers(Results, 0, 0);
                        break;
                    case 2:
                        Results = FilterByNumbers(Results, 1, 10);
                        break;
                    case 3:
                        Results = FilterByNumbers(Results, 11, 10000000);
                        break;
                }
            }
            CLI.CLIWriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
            foreach (Equipment eq in Results)
            {
                CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepo.Get(eq.RoomId).Name + " " + RoomRepo.Get(eq.RoomId).Type + " " + eq.Type);
            }

        }
    }
}
