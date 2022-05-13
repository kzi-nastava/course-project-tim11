using ClinicApp.Clinic;
using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ClinicApp
{
    public enum Roles
    {
        Nobody, Admin, Secretary, Doctor, Patient
    };

    public enum Fields
    {
        Cardiologist, Pneumologist, Gynecologist, Neurologist, Psychiatrist, Anesthesiologist, Pediatrician, Dermatologist, Endocrinologist, Gastroenterologist,
        Oncologist, Ophthalmologist, Urologist, PlasticSurgeon, Pathologist
    };

    public class SystemFunctions
    {

        // Dictionary of users created for faster and easier acces to information from the database
        public static Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
        public static Dictionary<string, Doctor> Doctors { get; set; } = new Dictionary<string, Doctor>();
        public static Dictionary<string, Patient> Patients { get; set; } = new Dictionary<string, Patient>();
        public static Dictionary<string, HealthRecord> HealthRecords { get; set; } = new Dictionary<string, HealthRecord>();
        public static Dictionary<int, Examination> AllExamtinations { get; set; } = new Dictionary<int, Examination>();
        public static Dictionary<int, Examination> CurrentExamtinations { get; set; } = new Dictionary<int, Examination>();
        public static List<Referral> Referrals { get; set; } = new List<Referral>();

        // User file path may change in release mode, this is the file path in debug mode

        public static string UsersFilePath = "../../../Data/users.txt";
        public static string ExaminationsFilePath = "../../../Data/examinations.txt";
        public static string HealthRecordsFilePath = "../../../Data/health_records.txt";
        public static string PatientRequestsFilePath = "../../../Data/patient_requests.txt";
        public static string ReferralsFilePath = "../../../Data/referrals.txt";



        // Loads the information from the database into objects and adds them to coresponding dictionaries
        public static void LoadData()
        {
            using (StreamReader reader = new StreamReader(UsersFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    User user = ParseUser(line);
                    Users.Add(user.UserName, user);
                    if (user.Role == Roles.Doctor)
                    {
                        Doctors.Add(user.UserName, (Doctor)user);
                    }
                    else if (user.Role == Roles.Patient)
                    {
                        Patients.Add(user.UserName, (Patient)user);
                    }
                }
            }
            using (StreamReader reader = new StreamReader(HealthRecordsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    HealthRecord healthRecord = new HealthRecord(line );
                    HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);

                }
            }
            using (StreamReader reader = new StreamReader(ReferralsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Referral referral = new Referral(line);
                    referral.Patient.Referrals.Add(referral);
                    Referrals.Add(referral);
                    ;

                }
            }

            using (StreamReader reader = new StreamReader(ExaminationsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Examination examination = new Examination(line);
                    if (!AllExamtinations.TryAdd(examination.ID, examination))
                    {
                        AllExamtinations[examination.ID] = examination;
                    }
                    if (examination.DateTime.AddMinutes(15) > DateTime.Now)
                    {
                        if (examination.Tombstone != 0)
                        {
                            CurrentExamtinations.Remove(examination.Tombstone);
                        }
                        else if (examination.Edited != 0)
                        {
                            CurrentExamtinations.Remove(examination.Edited);
                           if (!examination.Finished)
                            {
                                CurrentExamtinations.Add(examination.ID, examination);
                            }
                            
                        }
                        else if (!examination.Finished) {
                            CurrentExamtinations.Add(examination.ID, examination);
                        }
                    }
                    
                }
                foreach (int ID in CurrentExamtinations.Keys)
                {
                    Examination currentExamination = CurrentExamtinations[ID];
                    currentExamination.Doctor.Examinations.Add(currentExamination);
                    currentExamination.Patient.Examinations.Add(currentExamination);
                }
            }

        }


        // Parsing functions
        private static User ParseUser(string line)
        {
            string[] data = line.Split('|');
            if (data[6] == Roles.Admin.ToString())
                return new Admin(line);
            if (data[6] == Roles.Secretary.ToString())
                return new Secretary(line);
            if (data[6] == Roles.Doctor.ToString())
                return new Doctor(line);
            if (data[6] == Roles.Patient.ToString())
                return new Patient(line);
            return new Nobody();
        }

        // Uploads the information from the objects into the database
        public static void UploadData()
        {
            string newLine = "";
            using (StreamWriter sw = File.CreateText(UsersFilePath))
            {
                foreach (KeyValuePair<string, User> pair in Users)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
            using (StreamWriter sw = File.CreateText(ExaminationsFilePath))
            {
                foreach (KeyValuePair<int, Examination> pair in AllExamtinations)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }

            using (StreamWriter sw = File.CreateText(HealthRecordsFilePath))
            {
                foreach (KeyValuePair<string, HealthRecord> pair in HealthRecords)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
            using (StreamWriter sw = File.CreateText(ExaminationsFilePath))
            {
                foreach(KeyValuePair<int, Examination> pair in AllExamtinations)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
            using (StreamWriter sw = File.CreateText(ReferralsFilePath))
            {
                foreach (Referral referral in Referrals)
                {
                    newLine = referral.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
