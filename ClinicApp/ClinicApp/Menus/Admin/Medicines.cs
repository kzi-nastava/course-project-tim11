﻿using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class Medicines
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("1. Create medicine");
            CLI.CLIWriteLine("2. CRUD ingrediants");
            CLI.CLIWriteLine("3. Reviewed medicine requests");
            CLI.CLIWriteLine("0. Return");
            int answer = CLI.CLIEnterNumber();
            while (true)
            {
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        CreateMedicineRequest.Menu();
                        return;
                    case 2:
                        IngrediantsMenu.Menu();
                        return;
                    case 3:
                        ReviewedMeds.Menu();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
