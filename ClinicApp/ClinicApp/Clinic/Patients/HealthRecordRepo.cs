using System.Collections.Generic;
using System.IO;

namespace ClinicApp.Clinic
{
    public class HealthRecordRepo
    {
        public static Dictionary<string, HealthRecord> HealthRecords { get; set; } = new Dictionary<string, HealthRecord>();
        public static string HealthRecordsFilePath = "../../../Data/health_records.txt";

        public static void Load() {

            using (StreamReader reader = new StreamReader(HealthRecordsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    HealthRecord healthRecord = new HealthRecord(line);
                    HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
                }
            }
        }

        public static void Add(HealthRecord healthRecord) {
            HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
            PersistChanges();
        }
        public static void Update(HealthRecord healthRecord)
        {
            HealthRecords[healthRecord.Patient.UserName] = healthRecord;
            PersistChanges();
        }

        public static void Delete(HealthRecord healthRecord)
        {
            HealthRecords.Remove(healthRecord.Patient.UserName);
            PersistChanges();
        }

        public static void PersistChanges() {
            string newLine;
            using (StreamWriter sw = File.CreateText(HealthRecordsFilePath))
            {
                foreach (KeyValuePair<string, HealthRecord> pair in HealthRecords)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }

    }
}
