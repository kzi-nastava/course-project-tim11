using System.Collections.Generic;

namespace ClinicApp.Clinic
{
    public class Medicine
    {
        public string Name {get; set;}
        public List<string> Ingredients { get; set; }

        public Medicine(string name, List<string> ingredients) {
            Name = name;
            Ingredients = ingredients;
        }

        public Medicine(string line)
        {
            string[] tokens = line.Split('|');
            Name = tokens[0];
            string[] ingredients = tokens[1].Split('/');
            Ingredients = new List<string>();
            foreach (string ingredient in ingredients) {
                Ingredients.Add(ingredient);
            }
        }

        public string Compress()
        {
            string allIngredients = "";
            foreach (string ingredient in Ingredients)
            {
                allIngredients += ingredient + "/";
            }
            allIngredients = allIngredients.Remove(allIngredients.Length - 1);
            return this.Name + "|" + allIngredients;
        }
    }
}
