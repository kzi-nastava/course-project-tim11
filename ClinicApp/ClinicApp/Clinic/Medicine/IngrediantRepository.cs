using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.Clinic
{
    class IngrediantRepository
    {
        public static List<string> Ingrediants;

        static IngrediantRepository()
        {
            Ingrediants = LoadIngrediants();
        }
        public static List<string> GetAll() => Ingrediants;
        
        public static void Add(string ingr)
        {
            if (!Ingrediants.Contains(ingr))
            {
                Ingrediants.Add(ingr);
            }
            PresistChanges();
        }
        public static void Update(string ingr, string newIng)
        {
            if (Ingrediants.Contains(ingr))
            {
                Ingrediants[Ingrediants.IndexOf(ingr)] = newIng;
            }
            PresistChanges();
        }
        public static void Delete(string ingr)
        {
            if (Ingrediants.Contains(ingr))
            {
                Ingrediants.Remove(ingr);
            }
            PresistChanges();
        }
        public static List<string> LoadIngrediants()
        {
            List<string> ing = new List<string>();
            using (StreamReader reader = new StreamReader("../../../Data/ingrediants.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ing.Add(line);
                }
            }
            return ing;
        }
        public static void PresistChanges()
        {
            File.Delete("../../../Data/ingrediants.txt");
            foreach (var ingr in Ingrediants)
            {
                string newLine = ingr;
                using (StreamWriter sw = File.AppendText("../../../Data/ingrediants.txt"))
                {
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
