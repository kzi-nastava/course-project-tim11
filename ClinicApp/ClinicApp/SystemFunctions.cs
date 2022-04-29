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

    public class SystemFunctions
    {

        // Dictionary of users created for faster and easier acces to information from the database
        public static Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();


        // User file path may change in release mode, this is the file path in debug mode

        public static string UsersFilePath =  "../../../Data/users.txt";


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
                }
            }
        }

        // Parsing functions
        private static User ParseUser(string line)
        {
            string[] data = line.Split('|');
            if(data[6] == Roles.Admin.ToString())
                return new Admin(line);
            if(data[6] == Roles.Secretary.ToString())
                return new Secretary(line);
            if(data[6] == Roles.Doctor.ToString())
                return new Doctor(line);
            if(data[6] == Roles.Patient.ToString())
                return new Patient(line);
            return new Nobody();
        }
    }
}
