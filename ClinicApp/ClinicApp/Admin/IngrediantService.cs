using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class IngrediantService
    {
        public static void CreateIngrediant()
        {
            Console.WriteLine("Enter the new ingrediant");
            string ingrediant = CLI.CLIEnterStringWithoutDelimiter("|");
            if (IngrediantRepository.GetAll().Contains(ingrediant))
            {
                Console.WriteLine("Ingrediant already in database");
                return;
            }
            IngrediantRepository.Add(ingrediant);
        }
        public static void UpdateIngrediant()
        {
            Console.WriteLine("Select the ingrediant to update, 0 to continue");
            List<string> offeredIngrediants = IngrediantRepository.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                Console.WriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = CLI.CLIEnterNumberWithLimit(1, offeredIngrediants.Count);
            if (indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            Console.WriteLine("Enter the new ingrediant");
            string newIngr = CLI.CLIEnterStringWithoutDelimiter("|");
            IngrediantRepository.Update(selected, newIngr);
        }
        public static void DeleteIngrediant()
        {
            Console.WriteLine("Select the ingrediant to delete, 0 to return");
            List<string> offeredIngrediants = IngrediantRepository.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                Console.WriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = CLI.CLIEnterNumberWithLimit(1, offeredIngrediants.Count);
            if (indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            IngrediantRepository.Delete(selected);
        }
        public static List<string> ChooseIngrediants()
        {
            List<string> chosenIngrediants = new List<string>();
            List<string> offeredIngrediants = new List<string>(IngrediantRepository.GetAll());
            Console.WriteLine("Choose ingrediants, 0 to finish choosing");
            while (true)
            {
                foreach (var ingrediant in offeredIngrediants)
                {
                    Console.WriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
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
