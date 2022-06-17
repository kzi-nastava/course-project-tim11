using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;
using ClinicApp.Clinic.Medicine;

namespace ClinicApp.Menus.Admin.MedicineRequest
{
    class ReviewedMeds
    {
        public static void Dialog()
        {
            OtherFunctions.ListMedicineRequests();
            CLI.CLIWriteLine("Enter ID of the request, 0 to return");
            int id = CLI.CLIEnterNumber();
            if (id == 0) return;
            CLI.CLIWriteLine("Enter medicine name");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (MedicineRepo.Medicine.ContainsKey(name))
            {
                CLI.CLIWriteLine("Name already taken, enter another name");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = OtherFunctions.ChooseIngrediants();

            MedicineRequestService.UpdateReviewed(id, name, chosenIngrediants);
        }
    }
}
