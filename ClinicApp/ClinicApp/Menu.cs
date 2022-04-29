using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ClinicApp
{
    public class Menu
    {
        // Registration functions
        static void RegistrationDialog()
        {

            // First we create an instance of the User class
            Console.Write("Username: ");
            string userName = Console.ReadLine();
            Console.WriteLine("Chose role: Doctor(d), Admin(a), Secretary(s), Patient(p)");
            Console.Write("(d/a/s/p) >> ");
            string roleStr = Console.ReadLine();
            int role;

           
            switch (roleStr.ToLower())
            {
                case "a": role = 0; break;
                case "s": role = 1; break;
                case "d": role = 2; break;
                case "p": role = 3; break;
                default: 
                    Console.WriteLine("\nWrong input, try again");
                    InitialDialog();
                    return;
            }

            Console.Write("Password: ");
            string password = MaskPassword();
            Console.Write("\nRepeat password: ");
            string passwordCheck = MaskPassword();

           
            bool valid = ValidateRegistration(userName, password, passwordCheck);
            if (valid)
            {
                Console.WriteLine($"\nWelcome {userName}");
                int id;
                try
                {
                    string lastLine = File.ReadLines(SystemFunctions.UsersFilePath).Last();
                    string[] tmp = lastLine.Split('|');
                    id = Convert.ToInt32(tmp[0]) + 1;
                }
                catch (System.InvalidOperationException)
                {
                    id = 1;
                }



                // Creates user and adds them to the dictionary Users in SystemFunctions
                User newUser = new User(id, userName, password, role);
                SystemFunctions.Users.Add(userName, newUser);

                // Writes the information about the user to the database
                // ID|UserName|Password|Role
                // 322|Steven24|ilikebugs22|2
                string newLine = Convert.ToString(id) + "|" + userName + "|" + hashedPassword + "|" + Convert.ToString(role);
                using (StreamWriter sw = File.AppendText(SystemFunctions.UsersFilePath))
                {
                    sw.WriteLine(newLine);
                }

                // We collect additional information about the user 
                char gender;
                string name, lastName;
                DateTime dateOfBirth;
                (name, lastName, dateOfBirth, gender) = CollectUniversalData();

                // We create an object of the User with information specific for their role
                switch (newUser.Role)
                {
                    case Roles.Admin:
                        Admin admin = RegisterAdmin(newUser, name, lastName, dateOfBirth, gender);
                        admin.AdminMenu();
                        return;
                    case Roles.Secretary:
                        Secretary secretary = RegisterSecretary(newUser, name, lastName, dateOfBirth, gender);
                        secretary.SecretaryMenu();
                        return;
                    case Roles.Doctor:
                        Doctor doctor = RegisterDoctor(newUser, name, lastName, dateOfBirth, gender);
                        doctor.DoctorMenu();
                        return;
                    case Roles.Patient:
                        Patient patient = RegisterPatient(newUser, name, lastName, dateOfBirth, gender);
                        patient.PatientMenu();
                        return;
                    default:
                        return;
                }
            }
            else
            {
                InitialDialog();
                return;
            }

        }

        private static bool ValidateRegistration(string userName, string password, string passwordCheck)
        {
            if (passwordCheck != password)
            {
                Console.WriteLine("Passwords don't match, try again");
                return false;
            }
            User tmp = null;
            if (SystemFunctions.Users.TryGetValue(userName, out tmp))
            {
                Console.WriteLine("Username taken, try again");
                return false;
            }
            // TODO: add password checks, for example if password lenght < 8 return false etc.
            return true;
        }

        // TODO: each member should implement their each version for registration because each
        // type of user requires different information to be collected
        // Idea: for each registration dialog create a registration validation method

        private static Admin RegisterAdmin(User user, string name, string lastName, DateTime dateOfBirth, char gender) {
            string format = "dd/MM/yyyy";
            Admin admin = new Admin(user.ID, user.UserName, user.Password, name, lastName, dateOfBirth, gender);
            string newLine = Convert.ToString(user.ID) + "|" + user.UserName + "|" + user.Password + "|" + name + "|" + lastName + "|" + dateOfBirth.ToString(format) + "|" + gender;
            using (StreamWriter sw = File.AppendText(SystemFunctions.AdminsFilePath))
            {
                sw.WriteLine(newLine);
            }
            SystemFunctions.Admins.Add(user.UserName, admin);
            return admin;
        }
        private static Secretary RegisterSecretary(User user, string name, string lastName, DateTime dateOfBirth, char gender) {
            string format = "dd/MM/yyyy";
            Secretary secretary = new Secretary(user.ID, user.UserName, user.Password, name, lastName, dateOfBirth, gender);

            string newLine = Convert.ToString(user.ID) + "|" + user.UserName + "|" + user.Password + "|" + name + "|" + lastName + "|" + dateOfBirth.ToString(format) + "|" + gender;
            using (StreamWriter sw = File.AppendText(SystemFunctions.SecretariesFilePath))
            {
                sw.WriteLine(newLine);
            }
            SystemFunctions.Secretaries.Add(user.UserName, secretary);
            return secretary;
        }
        private static Doctor RegisterDoctor(User user, string name, string lastName, DateTime dateOfBirth, char gender) {
            string format = "dd/MM/yyyy";
            Doctor doctor = new Doctor(user.ID, user.UserName, user.Password, name, lastName, dateOfBirth, gender);
            string newLine = Convert.ToString(user.ID) + "|" + user.UserName + "|" + user.Password + "|" + name + "|" + lastName + "|" + dateOfBirth.ToString(format) + "|" + gender;
            using (StreamWriter sw = File.AppendText(SystemFunctions.DoctorsFilePath))
            {
                sw.WriteLine(newLine);
            }
            SystemFunctions.Doctors.Add(user.UserName, doctor);
            return doctor;
        }
        private static Patient RegisterPatient(User user, string name, string lastName, DateTime dateOfBirth, char gender) {
            string format = "dd/MM/yyyy";
            Patient patient = new Patient(user.ID, user.UserName, user.Password, name, lastName, dateOfBirth, gender);
            string newLine = Convert.ToString(user.ID) + "|" + user.UserName + "|" + user.Password + "|" + name + "|" + lastName + "|" + dateOfBirth.ToString(format) + "|" + gender;
            using (StreamWriter sw = File.AppendText(SystemFunctions.PatientsFilePath))
            {
                sw.WriteLine(newLine);
            }
            SystemFunctions.Patients.Add(user.UserName, patient);

            return patient;
        }




        //==================================================================================================================
        // Login functions


        static void LoginDialog()
        {
            Console.Write("Username: ");
            string userName = Console.ReadLine();
            Console.Write("Password: ");
            string password = MaskPassword();
            User user = null;
            if (SystemFunctions.Users.TryGetValue(userName, out user))
            {
                if (user.Password == password)
                {
                    Console.WriteLine($"\nWelcome {userName}");
                    FindUser(user);
                }
                else
                {
                    Console.WriteLine("\nIncorrect password, try again");
                    InitialDialog();
                }
            }
            else
            {
                Console.WriteLine("\nUser does not exist, try again");
               InitialDialog();
            }

        }


        //==================================================================================================================
        // Other functions

        // Collects the additonal data about users
        private static (string, string, DateTime, char) CollectUniversalData() {
            char gender;
            string name, lastName, genderStr;
            DateTime dateOfBirth;

            Console.Write("\nName: ");
            name = Console.ReadLine();

            Console.Write("\nLast name: ");
            lastName = Console.ReadLine();

            Console.Write("\nDate of birth (e.g. 22/10/1987): ");
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                dateOfBirth = DateTime.ParseExact(Console.ReadLine(), format, provider);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nIncorrect date format, try again");
                (name, lastName, dateOfBirth, gender) = CollectUniversalData();
                return (name, lastName, dateOfBirth, gender);
            }
            Console.Write("\nGender (m/f/n): ");
            genderStr = Console.ReadLine();
            switch (genderStr)
            {
                case "m": gender = 'm'; break;
                case "f": gender = 'f'; break;
                case "n": gender = 'n'; break;
                default:
                    Console.WriteLine("\nInvalid gender input, try again");
                    (name, lastName, dateOfBirth, gender) = CollectUniversalData();
                    return (name, lastName, dateOfBirth, gender);

            }
            return (name, lastName, dateOfBirth, gender);
        }


        private static string MaskPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
                else if (key.Key == ConsoleKey.Enter && password.Length > 0)
                {
                    return password;
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
        }

        private static void FindUser(User user)
        {
            switch (user.Role)
            {
                case Roles.Admin:
                    Admin admin;
                    SystemFunctions.Admins.TryGetValue(user.UserName, out admin);
                    admin.AdminMenu();
                    break;
                case Roles.Secretary:
                    Secretary secretary;
                    SystemFunctions.Secretaries.TryGetValue(user.UserName, out secretary);
                    secretary.SecretaryMenu();
                    break;
                case Roles.Doctor:
                    Doctor doctor;
                    SystemFunctions.Doctors.TryGetValue(user.UserName, out doctor);
                    doctor.DoctorMenu();
                    break;

                case Roles.Patient:
                    Patient patient;
                    SystemFunctions.Patients.TryGetValue(user.UserName, out patient);
                    patient.PatientMenu();
                    break;
                default:
                    return;
            }

        }
    }

}

