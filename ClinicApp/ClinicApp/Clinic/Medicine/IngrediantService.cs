using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class IngrediantService
    {
        public static void Update(string ingr, string newIng)
        {
            IngrediantRepository.Update(ingr, newIng);
        }
        public static void Delete(string selected)
        {
            IngrediantRepository.Delete(selected);
        }
        public static void Add(string ingrediant)
        {
            IngrediantRepository.Add(ingrediant);
        }
    }
}
