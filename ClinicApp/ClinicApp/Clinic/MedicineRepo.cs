using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.Clinic
{
    public class MedicineRepo
    {
        public static Dictionary<string, Medicine> Medicine { get; set; } = new Dictionary<string, Medicine>();
        public static string MedicineFilePath = "../../../Data/medicine.txt";

        public static void Load()
        {
            using (StreamReader reader = new StreamReader(MedicineFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Medicine medicine = new Medicine(line);
                    Medicine.Add(medicine.Name, medicine);
                }
            }
        }

        public static void Add(Medicine medicine)
        {
            Medicine.Add(medicine.Name, medicine);
        }
        public static void Update(Medicine medicine)
        {
            Medicine[medicine.Name] = medicine;
        }

        public static void Delete(Medicine medicine)
        {
            Medicine.Remove(medicine.Name);
        }

        public static void PersistChanges()
        {
            string newLine;
            using (StreamWriter sw = File.CreateText(MedicineFilePath))
            {
                foreach (KeyValuePair<string, Medicine> pair in Medicine)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
