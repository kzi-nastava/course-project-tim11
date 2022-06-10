using System;
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
                        MedicineRequestService.CreateMedicineRequest();
                        return;
                    case 2:
                        CRUDIngrediants();
                        return;
                    case 3:
                        MedicineRequestService.ReviewedMedsMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void CRUDIngrediants()
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
                    IngrediantService.CreateIngrediant();
                    break;
                case 2:
                    IngrediantService.UpdateIngrediant();
                    break;
                case 3:
                    IngrediantService.DeleteIngrediant();
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
