using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class ReviewedMeds
    {
        public static void Menu()
        {
            OtherFunctions.ListMedicineRequests();
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
    }
}
