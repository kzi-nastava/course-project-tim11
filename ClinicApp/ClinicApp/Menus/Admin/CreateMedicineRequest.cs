using System.Collections.Generic;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    internal class CreateMedicineRequest
    {
        public static void Menu()
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
    }
}