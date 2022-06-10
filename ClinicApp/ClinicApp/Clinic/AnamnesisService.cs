using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
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
                CLI.CLIWriteLine("Health record with that username does not exist");
                return;
            }
            healthRecord = HealthRecordRepo.HealthRecords[patient.UserName];

            CLI.CLIWriteLine("Patient health record.");
            HealthRecordService.ShowHealthRecord(healthRecord);
            CLI.CLIWriteLine("");
            CLI.CLIWriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++");
            CLI.CLIWriteLine("1.Sort amnesis list by date.");
            CLI.CLIWriteLine("2.Sort amnesis list by doctor.");
            CLI.CLIWriteLine("3.Find amnesis that contains specific word.");
            CLI.CLIWriteLine("4.Return to home page.");
            CLI.CLIWriteLine();
            CLI.CLIWriteLine();
            string user_input = CLI.CLIEnterString();
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
                CLI.CLIWriteLine("Enter the specific word for amnesis search:");
                string specific_word = Console.ReadLine();
                SearchAnamnesis(HealthRecordRepo.HealthRecords[patient.UserName],specific_word);
            }
            else if (user_input == "4")
            {
                return;
            }
            else
            {
                CLI.CLIWriteLine("Invalid input.");
                return;
            }
        }

        public void AnamnesisSortedDoctor(HealthRecord healthRecord)
        {
            healthRecord.Anamneses.Sort(delegate (Anamnesis a1, Anamnesis a2)
            {
                return a1.Doctor.UserName.CompareTo(a2.Doctor.UserName);
            });
            CLI.CLIWriteLine("Sorted anamnesis list by doctors username.");
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
            CLI.CLIWriteLine("Sorted anamnesis list by date.");
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
                CLI.CLIWriteLine("There is no amnesis with specified keyword.");
            }
        }

        public static void WriteAnamnesis(ref HealthRecord healthRecord, ref Doctor doctor)
        {
            CLI.CLIWriteLine("\nWrite you Anamnesis: ");
            string anamnesisText = CLI.CLIEnterString();
            Anamnesis anamnesis = new Anamnesis(anamnesisText, doctor);
            healthRecord.Anamneses.Add(anamnesis);
            CLI.CLIWriteLine("Anamnesis added");
        }


    }



}
