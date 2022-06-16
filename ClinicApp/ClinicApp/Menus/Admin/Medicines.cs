using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class Medicines
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("1. Create medicine");
            CLI.CLIWriteLine("2. CRUD ingrediants");
            CLI.CLIWriteLine("3. Reviewed medicine requests");
            CLI.CLIWriteLine("0. Return");
            int answer = CLI.CLIEnterNumber();
            while (true)
            {
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        CreateMedicineRequest();
                        return;
                    case 2:
                        CRUDIngrediants();
                        return;
                    case 3:
                        ReviewedMedsMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void CRUDIngrediants()
        {
            CLI.CLIWriteLine("Ingrediants Menu");
            CLI.CLIWriteLine("1. Add new Ingrediant");
            CLI.CLIWriteLine("2. Update Ingrediant");
            CLI.CLIWriteLine("3. Delete Ingrediant");
            CLI.CLIWriteLine("0. to return");
            int answer = CLI.CLIEnterNumberWithLimit(0, 3);
            switch (answer)
            {
                case 1:
                    IngrediantService.CreateIngrediant();
                    break;
                case 2:
                    IngrediantService.UpdateIngrediant();
                    break;
                case 3:
                    IngrediantService.DeleteIngrediant();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }
        }
        public static void CreateMedicineRequest()
        {
            CLI.CLIWriteLine("Enter medicine name");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (Clinic.MedicineRepo.Medicine.ContainsKey(name))
            {
                CLI.CLIWriteLine("Name already taken, enter another name");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = IngrediantService.ChooseIngrediants();
            MedicineRequestService.CreateNew(name, chosenIngrediants);
        }
        public static void ReviewedMedsMenu()
        {
            ListMedicineRequests();
            CLI.CLIWriteLine("Enter ID of the request, 0 to return");
            int id = CLI.CLIEnterNumber();
            if (id == 0) return;
            CLI.CLIWriteLine("Enter medicine name");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (Clinic.MedicineRepo.Medicine.ContainsKey(name))
            {
                CLI.CLIWriteLine("Name already taken, enter another name");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = IngrediantService.ChooseIngrediants();
            MedicineRequestService.UpdateReviewed(id, name, chosenIngrediants);
        }
        public static void ListMedicineRequests()
        {
            {
                CLI.CLIWriteLine("These requests have been reviewed by a doctor and should be fixed up");
                foreach (var request in MedicineRequestRepository.GetAll())
                {
                    if (request.Comment != "")
                    {
                        CLI.CLIWriteLine("----------------------------------------------------------");
                        CLI.CLIWriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + WriteMedicineIngrediants(request.Medicine.Ingredients) +
                            "\nDoctor's comment: " + request.Comment);
                        CLI.CLIWriteLine("----------------------------------------------------------");
                    }
                }
            }
        }
        public static string WriteMedicineIngrediants(List<string> ingrediants)
        {
            string output = "";
            foreach (var ingr in ingrediants)
            {
                output += ingr + ", ";
            }
            return output;
        }
    }
}
