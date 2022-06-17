using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class MedicineService
    {
        public static Medicine GetMedicine()
        {
            Console.Write("Insert the name of Medicine: ");
            string medicineName = Console.ReadLine();
            Console.WriteLine();
            Medicine medicine;
            if (!MedicineRepo.Medicine.TryGetValue(medicineName, out medicine))
            {
                Console.WriteLine("Medicine with that name does not exist.");
                return null;
            }
            return medicine;

        }

        public static bool CheckAlergy(Medicine medicine, string userName)
        {
            HealthRecord healthRecord = HealthRecordRepo.HealthRecords[userName];
            foreach (string alergy in healthRecord.Alergies)
            {
                foreach (string alergen in medicine.Ingredients)
                {
                    if (alergen.ToUpper() == alergy.ToUpper())
                    {
                        Console.WriteLine($"Error: Patient alergic to medicine. Alergen: {alergen}");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
