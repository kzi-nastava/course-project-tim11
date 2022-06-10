using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class MedicineService
    {
        public static Medicine GetMedicine()
        {
            CLI.CLIWrite("Insert the name of Medicine: ");
            string medicineName = CLI.CLIEnterString();
            CLI.CLIWriteLine();
            Medicine medicine;
            if (!MedicineRepo.Medicine.TryGetValue(medicineName, out medicine))
            {
                CLI.CLIWriteLine("Medicine with that name does not exist.");
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
                        CLI.CLIWriteLine($"Error: Patient alergic to medicine. Alergen: {alergen}");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
