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
                Console.WriteLine("No health record found, creating a new record");
                healthRecord = new HealthRecord(patient);
                HealthRecordRepo.Add(healthRecord);
            }
            return healthRecord;
        }

        public static void ShowHealthRecord(HealthRecord healthRecord)
        {
            Console.WriteLine($"Patient {healthRecord.Patient.Name} {healthRecord.Patient.LastName}'s health record\n");
            Console.WriteLine($"Weight: {healthRecord.Weight}\nHeight: {healthRecord.Height}\n");
            Console.Write("Medical history: ");
            foreach (string illness in healthRecord.MedicalHistory)
            {
                Console.Write(illness + ", ");
            }
            Console.Write("\nKnown alergies: ");
            foreach (string alergy in healthRecord.Alergies)
            {
                Console.Write(alergy + ", ");
            }
            int i = 1;
            Console.WriteLine("\nPrevious anamnesis: ");
            foreach (Anamnesis anamnesis in healthRecord.Anamneses)
            {
                Console.WriteLine("\n\n" + i + ". Anamnesis");
                anamnesis.ShowAnamnesis();
                i++;
            }
        }
        //=======================================================================================================================================================================
        // CHANGE PATIENT HEALTH RECORD

        public static void ChangePatientRecord(ref HealthRecord healthRecord)
        {
            Console.Write("Change weight?(y/n): ");
            string choice;
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                Console.Write("New weight: ");
                double weight = CLI.CLIEnterDouble();
                healthRecord.Weight = weight;
            }
            Console.Write("Change height? (y/n): ");
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                Console.Write("New height: ");
                double height = CLI.CLIEnterDouble();
                healthRecord.Height = height;
            }
            Console.Write("Add to medical history(y/n): ");
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                string illness = Console.ReadLine();
                healthRecord.MedicalHistory.Add(illness);
            }
            Console.Write("Add to list of allergies(y/n): ");
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                string alergy = Console.ReadLine();
                healthRecord.Alergies.Add(alergy);
            }
            if (!HealthRecordRepo.HealthRecords.TryAdd(healthRecord.Patient.UserName, healthRecord))
            {
                HealthRecordRepo.HealthRecords[healthRecord.Patient.UserName] = healthRecord;
                
            }
            HealthRecordRepo.PersistChanges();
        }
    }
}
