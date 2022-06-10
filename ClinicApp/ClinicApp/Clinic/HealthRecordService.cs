using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class HealthRecordService
    {
        public static void NewHealthRecord(Patient patient) {
            HealthRecord healthRecord = new HealthRecord(patient);
            HealthRecordRepo.Add(healthRecord);
        }

        public static HealthRecord Search(Patient patient) {
            HealthRecord healthRecord;
            if (!HealthRecordRepo.HealthRecords.TryGetValue(patient.UserName, out healthRecord))
            {
                CLI.CLIWriteLine("No health record found, creating a new record");
                healthRecord = new HealthRecord(patient);
                HealthRecordRepo.HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
            }
            return healthRecord;
        }

        public static void ShowHealthRecord(HealthRecord healthRecord)
        {
            CLI.CLIWriteLine($"Patient {healthRecord.Patient.Name} {healthRecord.Patient.LastName}'s health record\n");
            CLI.CLIWriteLine($"Weight: {healthRecord.Weight}\nHeight: {healthRecord.Height}\n");
            CLI.CLIWrite("Medical history: ");
            foreach (string illness in healthRecord.MedicalHistory)
            {
                CLI.CLIWrite(illness + ", ");
            }
            CLI.CLIWrite("\nKnown alergies: ");
            foreach (string alergy in healthRecord.Alergies)
            {
                CLI.CLIWrite(alergy + ", ");
            }
            int i = 1;
            CLI.CLIWriteLine("\nPrevious anamnesis: ");
            foreach (Anamnesis anamnesis in healthRecord.Anamneses)
            {
                CLI.CLIWriteLine("\n\n" + i + ". Anamnesis");
                anamnesis.ShowAnamnesis();
                i++;
            }
        }
        //=======================================================================================================================================================================
        // CHANGE PATIENT HEALTH RECORD

        public static void ChangePatientRecord(ref HealthRecord healthRecord)
        {
            CLI.CLIWrite("Change weight?(y/n): ");
            string choice;
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                CLI.CLIWrite("New weight: ");
                double weight = CLI.CLIEnterDouble();
                healthRecord.Weight = weight;
            }
            CLI.CLIWrite("Change height? (y/n): ");
            choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                CLI.CLIWrite("New height: ");
                double height = CLI.CLIEnterDouble();
                healthRecord.Height = height;
            }
            CLI.CLIWrite("Add to medical history(y/n): ");
            choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                string illness = CLI.CLIEnterString();
                healthRecord.MedicalHistory.Add(illness);
            }
            CLI.CLIWrite("Add to list of allergies(y/n): ");
            choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                string alergy = CLI.CLIEnterString();
                healthRecord.Alergies.Add(alergy);
            }
            if (!HealthRecordRepo.HealthRecords.TryAdd(healthRecord.Patient.UserName, healthRecord))
            {
                HealthRecordRepo.HealthRecords[healthRecord.Patient.UserName] = healthRecord;
            }
        }
    }
}
