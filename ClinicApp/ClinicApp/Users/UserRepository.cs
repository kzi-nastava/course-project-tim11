using System.Collections.Generic;
using System.IO;

namespace ClinicApp.Users
{
    public class UserRepository
    {
        public static string UsersFilePath = "../../../Data/users.txt";

        public static Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
        public static Dictionary<string, Patient> Patients { get; set; } = new Dictionary<string, Patient>();
        public static Dictionary<string, Doctor> Doctors { get; set; } = new Dictionary<string, Doctor>();

        public static void Load()
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
        }

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

        public static void Upload()
        {
            string newLine;

            using (StreamWriter sw = File.CreateText(UsersFilePath))
            {
                foreach (KeyValuePair<string, User> pair in Users)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
