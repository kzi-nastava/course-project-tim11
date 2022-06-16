using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class IngrediantsMenu
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("Ingrediants Menu");
            CLI.CLIWriteLine("1. Add new Ingrediant");
            CLI.CLIWriteLine("2. Update Ingrediant");
            CLI.CLIWriteLine("3. Delete Ingrediant");
            CLI.CLIWriteLine("0. to return");
            int answer = CLI.CLIEnterNumberWithLimit(0, 3);
            switch (answer)
            {
                case 1:
                    CreateIngrediant.Menu();
                    break;
                case 2:
                    UpdateIngrediant.Menu();
                    break;
                case 3:
                    DeleteIngrediant.Menu();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }
        }
    }
}
