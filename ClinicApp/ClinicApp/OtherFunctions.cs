using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClinicApp
{
    public class OtherFunctions
    {
        public static string EnterString()
        {
            string input = Console.ReadLine();
            input = input.Trim();
            while(input == "")
            {
                Console.WriteLine("You have to write something.");
                input = Console.ReadLine();
                input = input.Trim();
            }
            return input;
        }

        public static int EnterNumber()
        {
            int x = -1;
            string s;
            while (true)
            {
                s = EnterString();
                try
                {
                    x = Int32.Parse(s);
                }
                catch (Exception e)
                {
                    Console.WriteLine("You didn't enter a number. Try again.");
                }
                return x;
            }
        }

        public static int EnterNumberWithLimit(int lowerLimit, int upperLimit)
        {
            if (upperLimit < lowerLimit)
                upperLimit = lowerLimit;
            int option = EnterNumber();
            while (option < lowerLimit || option > upperLimit)
            {
                Console.WriteLine("You didn't enter a valid option. Try again.");
                option = EnterNumber();
            }

            return option;
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

        public static User LogIn()
        {
            int option = 1;
            User user = new Nobody(), tempUser = null;
            while (option != 0)
            {
                Console.Write("Username: ");
                string userName = EnterString();
                Console.Write("Password: ");
                string password = MaskPassword();
                if (SystemFunctions.Users.TryGetValue(userName, out tempUser))
                {
                    if (tempUser.Password == password)
                    {
                        Console.WriteLine($"\nWelcome {tempUser.UserName}");
                        user = tempUser;
                        option = 0;
                    }
                    else
                    {
                        Console.WriteLine("\nIncorrect password. Want to try again?");
                        Console.WriteLine("1: Yes");
                        Console.WriteLine("0: No");
                        option = EnterNumberWithLimit(0, 1);
                    }
                }
                else
                {
                    Console.WriteLine("\nUser does not exist. Want to try again?");
                    Console.WriteLine("1: Yes");
                    Console.WriteLine("0: No");
                    option = EnterNumberWithLimit(0, 1);
                }
            }

            return user;
        }

        public static User Register()
        {
            string text = "", password, passwordCheck, temp;
            int option = 1;

            Console.Write("Username: ");
            temp = EnterString();
            while(SystemFunctions.Users.ContainsKey(temp))
            {
                Console.WriteLine("This username is taken. Please, try again.");
                Console.Write("Username: ");
                temp = EnterString();
            }
            text += temp + "|";

            Console.Write("Password: ");
            password = MaskPassword();
            Console.Write("\nRepeat password: ");
            passwordCheck = MaskPassword();
            while(password != passwordCheck)
            {
                Console.WriteLine("Passwords don't match. Please, try again.");
                Console.Write("Password: ");
                password = MaskPassword();
                Console.Write("\nRepeat password: ");
                passwordCheck = MaskPassword();
            }
            text += password + "|";

            Console.Write("\nName: ");
            text += EnterString() + "|";
            Console.Write("Last name: ");
            text += EnterString() + "|";

            Console.Write("Date of birth (e.g. 02/05/1984): ");
            string format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            temp = null;
            while(temp == null)
            {
                try
                {
                    temp = DateTime.ParseExact(EnterString(), format, provider).ToString();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Incorrect date format. Please, try again.");
                }
            }
            text += temp + "|";

            Console.Write("Gender (m/f): ");
            temp = EnterString();
            while(temp != "m" && temp != "f")
            {
                Console.Write("You didn't enter a valid option. Please, try again (m/f): ");
                temp = EnterString();
            }
            text += temp + "|";

            Console.WriteLine("Chose your role: Doctor(d), Admin(a), Secretary(s), Patient(p)");
            Console.WriteLine("1: Admin");
            Console.WriteLine("2: Secretary");
            Console.WriteLine("3: Doctor");
            Console.WriteLine("4: Patient");
            option = EnterNumberWithLimit(1, 4);
            switch(option)
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
        }

        private static User RegisterAdmin(string text)
        {
            return new Users.Admin(text);
        }

        private static User RegisterSecretary(string text)
        {
            return new Secretary(text);
        }

        private static User RegisterDoctor(string text)
        {
            return new Doctor(text);
        }

        private static User RegisterPatient(string text)
        {
            return new Patient(text);
        }
    }
}
