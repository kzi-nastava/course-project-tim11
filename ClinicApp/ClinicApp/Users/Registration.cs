using System;
using ClinicApp.Users;

namespace ClinicApp
{
    public class Registration
    {
        public static string GetUsername()
        {
            string temp;

            Console.Write("Username: ");
            temp = Console.ReadLine();
            while (UserRepository.Users.ContainsKey(temp))
            {
                Console.WriteLine("This username is taken. Please, try again.");
                Console.Write("Username: ");
                temp = Console.ReadLine();
            }

            return temp;
        }

        public static string GetPassword()
        {
            string password, passwordCheck;

            Console.Write("Password: ");
            password = CLI.CLIEnterPassword();
            Console.Write("Repeat password: ");
            passwordCheck = CLI.CLIEnterPassword();
            while (password != passwordCheck)
            {
                Console.WriteLine("Passwords don't match. Please, try again.");
                Console.Write("Password: ");
                password = CLI.CLIEnterPassword();
                Console.Write("Repeat password: ");
                passwordCheck = CLI.CLIEnterPassword();
            }

            return password;
        }

        public static string GetName()
        {
            string temp;

            Console.Write("Name: ");
            temp = Console.ReadLine() + "|";

            return temp;
        }

        public static string GetLastName()
        {
            string temp;

            Console.Write("Last name: ");
            temp = Console.ReadLine() + "|";

            return temp;
        }

        public static string GetDateOfBirth()
        {
            string temp;

            Console.Write("Date of birth (e.g. 02/05/1984): ");
            temp = CLI.CLIEnterDate().Date.ToString();

            return temp;
        }

        public static string GetGender()
        {
            string temp;

            Console.Write("\nGender (m/f/n): ");
            temp = Console.ReadLine();
            while (temp != "m" && temp != "f" && temp != "n")
            {
                Console.Write("You didn't enter a valid option. Please, try again (m/f/n): ");
                temp = Console.ReadLine();
            }

            return temp;
        }

        public static User Register(Roles role = Roles.Nobody)
        {
            string text = "";
            int option;

            text += GetUsername() + "|";

            text += GetPassword() + "|";

            text += GetName() + "|";

            text += GetLastName() + "|";

            text += GetDateOfBirth() + "|";
            
            text += GetGender() + "|";

            switch (role)
            {
                case Roles.Nobody:
                    Console.WriteLine("Chose your role.");
                    Console.WriteLine("1: Admin");
                    Console.WriteLine("2: Secretary");
                    Console.WriteLine("3: Doctor");
                    Console.WriteLine("4: Patient");
                    option = CLI.CLIEnterNumberWithLimit(1, 4);
                    switch (option)
                    {
                        case 1:
                            text += "Admin";
                            return RegisterAdmin(text);
                        case 2:
                            text += "Secretary";
                            return RegisterSecretary(text);
                        case 3:
                            text += "Doctor";
                            return RegisterDoctor(text);
                        case 4:
                            text += "Patient";
                            return RegisterPatient(text);
                        default:
                            return new Nobody();
                    }
                case Roles.Admin:
                    text += "Admin";
                    return RegisterAdmin(text);
                case Roles.Secretary:
                    text += "Secretary";
                    return RegisterSecretary(text);
                case Roles.Doctor:
                    text += "Doctor";
                    return RegisterDoctor(text);
                case Roles.Patient:
                    text += "Patient";
                    return RegisterPatient(text);
                default:
                    return new Nobody();
            }
        }

        private static User RegisterAdmin(string text)
        {
            return new Admin(text);
        }

        private static User RegisterSecretary(string text)
        {
            return new Secretary(text);
        }

        private static User RegisterDoctor(string text)
        {
            Fields field = OtherFunctions.AskField();
            text += "|" + field.ToString();
            return new Doctor(text);
        }

        private static User RegisterPatient(string text)
        {
            text += "|Unblocked";
            return new Patient(text);
        }
    }
}
