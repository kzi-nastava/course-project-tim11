using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class IngrediantService
    {
        
        public static List<string> ChooseIngrediants()
        {
            List<string> chosenIngrediants = new List<string>();
            List<string> offeredIngrediants = new List<string>(IngrediantRepository.GetAll());
            CLI.CLIWriteLine("Choose ingrediants, 0 to finish choosing");
            while (true)
            {
                foreach (var ingrediant in offeredIngrediants)
                {
                    CLI.CLIWriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
                }
                var choice = CLI.CLIEnterNumberWithLimit(0, offeredIngrediants.Count);
                if (choice == 0 && chosenIngrediants.Count > 0)
                {
                    break;
                }
                chosenIngrediants.Add(offeredIngrediants[choice - 1]);
                offeredIngrediants.Remove(offeredIngrediants[choice - 1]);
            }
            return chosenIngrediants;
        }
    }
}
