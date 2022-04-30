using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
                catch
                {
                    Console.WriteLine("You didn't enter a number. Try again.");
                }
                return x;
            }
        }

        public static double EnterDouble()
        {
            double x = -1;
            string s;
            while (true)
            {
                s = EnterString();
                try
                {
                    x = Convert.ToDouble(s);
                }
                catch (Exception e)
                {
                    Console.WriteLine("You didn't enter a decimal number. Try again.");
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

        public static DateTime AskForDate()
        {
            DateTime? date = null;
            string format = "dd/MM/yyyy";

            while (date == null) {
                CultureInfo provider = CultureInfo.InvariantCulture;
                try
                {
                    date = DateTime.ParseExact(Console.ReadLine(), format, provider);
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nIncorrect date format, try again");
                }
            }
            return (DateTime)date;
        }
        public static DateTime AskForTime()
        {
            DateTime? time = null;
            string format = "HH:mm";
            while (time == null) {
                
                CultureInfo provider = CultureInfo.InvariantCulture;
                try
                {
                    time = DateTime.ParseExact(Console.ReadLine(), format, provider);
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nIncorrect time format, try again");
                }
            }
            return (DateTime)time;
            
        }

        public static bool AskQuit()
        {
            Console.WriteLine("Do you wish to quit? (y/n)");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                return true;
            }
            else return false;
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

            Console.Write("Gender (m/f/n): ");
            temp = EnterString();
            while(temp != "m" && temp != "f" && temp!= "n")
            {
                Console.Write("You didn't enter a valid option. Please, try again (m/f/n): ");
                temp = EnterString();
            }
            text += temp + "|";

            Console.WriteLine("Chose your role: Doctor(d), Admin(a), Secretary(s), Patient(p)");
            string choice = EnterString();
            switch(choice.ToUpper())
            {
                case "A":
                    text += "Admin";
                    return RegisterAdmin(text);
                case "S":
                    text += "Secretary";
                    return RegisterSecretary(text);
                case "D":
                    text += "Doctor";
                    return RegisterDoctor(text);
                case "P":
                    text += "Patient";
                    return RegisterPatient(text);
                default:
                    return new Nobody();
            }
        }

        private static User RegisterAdmin(string text)
        {
            Users.Admin admin =  new Users.Admin(text);

            return admin;
        }

        private static User RegisterSecretary(string text)
        {
            Secretary secretary = new Secretary(text);

            return secretary;
        }

        private static User RegisterDoctor(string text)
        {
            Doctor doctor = new Doctor(text);
           
            return doctor;
        }

        private static User RegisterPatient(string text)
        {
            Patient patient = new Patient(text);
            
            return patient;
        }
    }
}
