using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ClinicApp
{
    public class SystemFunctions
    {

        // Dictionary of users created for faster and easier acces to information from the database
        public static Dictionary<string, User> Users { get; set; } = new Dictionary<string, User>();
        public static Dictionary<string, Admin> Admins { get; set; } = new Dictionary<string, Admin>();
        public static Dictionary<string, Secretary> Secretaries { get; set; } = new Dictionary<string, Secretary>();
        public static Dictionary<string, Doctor> Doctors { get; set; } = new Dictionary<string, Doctor>();
        public static Dictionary<string, Patient> Patients { get; set; } = new Dictionary<string, Patient>();


        // User file path may change in release mode, this is the file path in debug mode

        public static string UsersFilePath =  "../../../Data/users.txt";
        public static string AdminsFilePath = "../../../Data/admins.txt";
        public static string SecretariesFilePath = "../../../Data/secretaries.txt";
        public static string DoctorsFilePath = "../../../Data/doctors.txt";
        public static string PatientsFilePath = "../../../Data/patients.txt";


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
            using (StreamReader reader = new StreamReader(AdminsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Admin admin = ParseAdmin(line);
                    Admins.Add(admin.UserName, admin);
                }
            }
            using (StreamReader reader = new StreamReader(SecretariesFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Secretary secretary = ParseSecretary(line);
                    Secretaries.Add(secretary.UserName, secretary);
                }
            }

            using (StreamReader reader = new StreamReader(DoctorsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Doctor doctor = ParseDoctor(line);
                    Doctors.Add(doctor.UserName, doctor);
                }
            }
            using (StreamReader reader = new StreamReader(PatientsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Patient patient = ParsePatient(line);
                    Patients.Add(patient.UserName, patient);
                }
            }
        }

        // Parsing functions
        private static User ParseUser(string line)
        {
            string[] parameters = line.Split('|');
            User user = new User(Convert.ToInt32(parameters[0]), parameters[1], parameters[2], Convert.ToInt32(parameters[3]));
            return user;
        }
        private static Admin ParseAdmin(string line)
        {
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            
            string[] parameters = line.Split('|');
            Admin admin = new Admin(Convert.ToInt32(parameters[0]), parameters[1], parameters[2], parameters[3], parameters[4], DateTime.ParseExact(parameters[5], format, provider), Convert.ToChar(parameters[6]));
            return admin;
        }

        private static Secretary ParseSecretary(string line)
        {
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;

            string[] parameters = line.Split('|');
            Secretary secretary = new Secretary(Convert.ToInt32(parameters[0]), parameters[1], parameters[2], parameters[3], parameters[4], DateTime.ParseExact(parameters[5], format, provider), Convert.ToChar(parameters[6]));
            return secretary;
        }
        private static Doctor ParseDoctor(string line)
        {
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;

            string[] parameters = line.Split('|');
            Doctor doctor = new Doctor(Convert.ToInt32(parameters[0]), parameters[1], parameters[2], parameters[3], parameters[4], DateTime.ParseExact(parameters[5], format, provider), Convert.ToChar(parameters[6]));
            return doctor;
        }
        private static Patient ParsePatient(string line)
        {
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;

            string[] parameters = line.Split('|');
            Patient patient = new Patient(Convert.ToInt32(parameters[0]), parameters[1], parameters[2], parameters[3], parameters[4], DateTime.ParseExact(parameters[5], format, provider), Convert.ToChar(parameters[6]));
            return patient;
        }
    }
}
