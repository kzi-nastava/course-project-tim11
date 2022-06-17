using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;

namespace ClinicApp.Menus.Admin
{
    class UpdateIngrediant
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Select the ingrediant to update, 0 to continue");
            List<string> offeredIngrediants = IngrediantRepository.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                CLI.CLIWriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = CLI.CLIEnterNumberWithLimit(0, offeredIngrediants.Count);
            if (indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            CLI.CLIWriteLine("Enter the new ingrediant");
            string newIngr = CLI.CLIEnterStringWithoutDelimiter("|");

            IngrediantService.Update(selected, newIngr);
        }
    }
}
