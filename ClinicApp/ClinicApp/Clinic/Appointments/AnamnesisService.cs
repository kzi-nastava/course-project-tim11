using System;
using ClinicApp.Dialogs;
using ClinicApp.Users;

namespace ClinicApp.Clinic.Appointmens
{
    public class AnamnesisService
    {
        public AnamnesisService()
        {
        }

        private void ViewAnamnesis(Patient patient)
        {
            HealthRecord healthRecord = null;
            if (!HealthRecordRepo.HealthRecords.TryGetValue(patient.UserName, out healthRecord))
            {
                Console.WriteLine("Health record with that username does not exist");
                return;
            }
            healthRecord = HealthRecordRepo.HealthRecords[patient.UserName];

            Console.WriteLine("Patient health record.");
            HealthRecordDialog.ShowHealthRecord(healthRecord);
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("1.Sort amnesis list by date.");
            Console.WriteLine("2.Sort amnesis list by doctor.");
            Console.WriteLine("3.Find amnesis that contains specific word.");
            Console.WriteLine("4.Return to home page.");
            Console.WriteLine();
            Console.WriteLine();
            string user_input = Console.ReadLine();
            if (user_input == "1")
            {
                //sort by date
                AnamnesisSortedDate(HealthRecordRepo.HealthRecords[patient.UserName]);
            }
            else if (user_input == "2")
            {
                //sort by doctor
                AnamnesisSortedDoctor(HealthRecordRepo.HealthRecords[patient.UserName]);
            }
            else if (user_input == "3")
            {
                Console.WriteLine("Enter the specific word for amnesis search:");
                string specific_word = Console.ReadLine();
                SearchAnamnesis(HealthRecordRepo.HealthRecords[patient.UserName],specific_word);
            }
            else if (user_input == "4")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return;
            }
        }

        public void AnamnesisSortedDoctor(HealthRecord healthRecord)
        {
            healthRecord.Anamneses.Sort(delegate (Anamnesis a1, Anamnesis a2)
            {
                return a1.Doctor.UserName.CompareTo(a2.Doctor.UserName);
            });
            Console.WriteLine("Sorted anamnesis list by doctors username.");
            foreach (Anamnesis temp in healthRecord.Anamneses)
            {
                temp.ShowAnamnesis();
            }
        }

        public void AnamnesisSortedDate(HealthRecord healthRecord)
        {
            healthRecord.Anamneses.Sort(delegate (Anamnesis a1, Anamnesis a2)
            {
                return a1.Date.CompareTo(a2.Date);
            });
            Console.WriteLine("Sorted anamnesis list by date.");
            foreach (Anamnesis temp in healthRecord.Anamneses)
            {
                temp.ShowAnamnesis();
            }
        }

        public void SearchAnamnesis(HealthRecord healthRecord,string keyword)
        {
            bool found = false;
            foreach (Anamnesis temp in healthRecord.Anamneses)
            {
                if (temp.Report.Contains(keyword) == true)
                {
                    temp.ShowAnamnesis();
                    found = true;
                }
            }
            if (found == false)
            {
                Console.WriteLine("There is no amnesis with specified keyword.");
            }
        }

        public static void WriteAnamnesis(ref HealthRecord healthRecord, ref Doctor doctor)
        {
            Console.WriteLine("\nWrite you Anamnesis: ");
            string anamnesisText = Console.ReadLine();
            Anamnesis anamnesis = new Anamnesis(anamnesisText, doctor);
            healthRecord.Anamneses.Add(anamnesis);
            Console.WriteLine("Anamnesis added");
        }


    }



}
