using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;

namespace ClinicApp.Menus.Admin
{
    class CreateIngrediant
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Enter the new ingrediant");
            string ingrediant = CLI.CLIEnterStringWithoutDelimiter("|");
            if (IngrediantRepository.GetAll().Contains(ingrediant))
            {
                CLI.CLIWriteLine("Ingrediant already in database");
                return;
            }
            IngrediantService.Add(ingrediant);
        }
    }
}
