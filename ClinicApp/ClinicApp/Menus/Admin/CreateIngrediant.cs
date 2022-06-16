using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class CreateIngrediant
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("Enter the new ingrediant");
            string ingrediant = CLI.CLIEnterStringWithoutDelimiter("|");
            if (IngrediantRepository.GetAll().Contains(ingrediant))
            {
                CLI.CLIWriteLine("Ingrediant already in database");
                return;
            }
            IngrediantRepository.Add(ingrediant);
        }
    }
}
