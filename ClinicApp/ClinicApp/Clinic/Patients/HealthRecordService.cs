using ClinicApp.Users;
using System;

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
    }
}
