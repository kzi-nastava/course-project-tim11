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
            Console.WriteLine("1. Create medicine");
            Console.WriteLine("2. CRUD ingrediants");
            Console.WriteLine("3. Reviewed medicine requests");
            Console.WriteLine("0. Return");
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
            Console.WriteLine("Ingrediants Menu");
            Console.WriteLine("1. Add new Ingrediant");
            Console.WriteLine("2. Update Ingrediant");
            Console.WriteLine("3. Delete Ingrediant");
            Console.WriteLine("0. to return");
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
