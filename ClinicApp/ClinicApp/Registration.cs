using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Users;

namespace ClinicApp
{
    public class Registration
    {
        public static string GetUsername()
        {
            string temp;

            CLI.CLIWrite("Username: ");
            temp = CLI.CLIEnterString();
            while (UserRepository.Users.ContainsKey(temp))
            {
                CLI.CLIWriteLine("This username is taken. Please, try again.");
                CLI.CLIWrite("Username: ");
                temp = CLI.CLIEnterString();
            }

            return temp;
        }

        public static string GetPassword()
        {
            string password, passwordCheck;

            CLI.CLIWrite("Password: ");
            password = CLI.CLIEnterPassword();
            CLI.CLIWrite("Repeat password: ");
            passwordCheck = CLI.CLIEnterPassword();
            while (password != passwordCheck)
            {
                CLI.CLIWriteLine("Passwords don't match. Please, try again.");
                CLI.CLIWrite("Password: ");
                password = CLI.CLIEnterPassword();
                CLI.CLIWrite("Repeat password: ");
                passwordCheck = CLI.CLIEnterPassword();
            }

            return password;
        }

        public static string GetName()
        {
            string temp;

            CLI.CLIWrite("Name: ");
            temp = CLI.CLIEnterString() + "|";

            return temp;
        }

        public static string GetLastName()
        {
            string temp;

            CLI.CLIWrite("Last name: ");
            temp = CLI.CLIEnterString() + "|";

            return temp;
        }

        public static string GetDateOfBirth()
        {
            string temp;

            CLI.CLIWrite("Date of birth (e.g. 02/05/1984): ");
            temp = CLI.CLIEnterDate().Date.ToString();

            return temp;
        }

        public static string GetGender()
        {
            string temp;

            CLI.CLIWrite("\nGender (m/f/n): ");
            temp = CLI.CLIEnterString();
            while (temp != "m" && temp != "f" && temp != "n")
            {
                CLI.CLIWrite("You didn't enter a valid option. Please, try again (m/f/n): ");
                temp = CLI.CLIEnterString();
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
                    CLI.CLIWriteLine("Chose your role.");
                    CLI.CLIWriteLine("1: Admin");
                    CLI.CLIWriteLine("2: Secretary");
                    CLI.CLIWriteLine("3: Doctor");
                    CLI.CLIWriteLine("4: Patient");
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
