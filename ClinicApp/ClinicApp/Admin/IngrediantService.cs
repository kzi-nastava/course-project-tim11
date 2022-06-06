using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class IngrediantService
    {
        public static void CreateIngrediant()
        {
            CLI.CLIWriteLine("Enter the new ingrediant");
            string ingrediant = CLI.CLIEnterStringWithoutDelimiter("|");
            IngrediantRepo.Add(ingrediant);
        }
        public static void UpdateIngrediant()
        {
            CLI.CLIWriteLine("Select the ingrediant to update, 0 to continue");
            List<string> offeredIngrediants = IngrediantRepo.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                CLI.CLIWriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = CLI.CLIEnterNumberWithLimit(1, offeredIngrediants.Count);
            if (indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            CLI.CLIWriteLine("Enter the new ingrediant");
            string newIngr = CLI.CLIEnterStringWithoutDelimiter("|");
            IngrediantRepo.Update(selected, newIngr);
        }
        public static void DeleteIngrediant()
        {
            CLI.CLIWriteLine("Select the ingrediant to delete, 0 to return");
            List<string> offeredIngrediants = IngrediantRepo.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                CLI.CLIWriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = CLI.CLIEnterNumberWithLimit(1, offeredIngrediants.Count);
            if (indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            IngrediantRepo.Delete(selected);
        }
    }
}
