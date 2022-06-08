using System;
using System.Collections.Generic;
using System.IO;

namespace ClinicApp.Users
{
    public class PatientRepository
    {
        public static Dictionary<string, Patient> Patients { get; set; } = new Dictionary<string, Patient>();
        public static string UsersFilePath = "../../../Data/users.txt";

        public static void LoadData()
        {
            //Loads the users.
            using (StreamReader reader = new StreamReader(UsersFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    User user = ParseUser(line);
                    else if (user.Role == Roles.Patient)
                    {
                        Patients.Add(user.UserName, (Patient)user);
                    }
                }
            }
        }

            public PatientRepository()
        {
        }

        public string Compress(Patient patient)
        {
            return patient.UserName + "|" + patient.Password + "|" + patient.Name + "|" + patient.LastName + "|" + patient.DateOfBirth.ToString("dd/MM/yyyy") + "|" + patient.Gender + "|" + patient.Role + "|" + patient.Blocked.ToString();
        }

    }
}
