using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class DeleteIngrediant
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("Select the ingrediant to delete, 0 to return");
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
            IngrediantRepository.Delete(selected);
        }
    }
}
