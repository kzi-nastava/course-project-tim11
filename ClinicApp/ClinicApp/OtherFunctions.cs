using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ClinicApp.AdminFunctions;

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
                catch (Exception)
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
        public static DateTime EnterDate()
        {
            DateTime date;
            while (true)
            {

                string dateString = Console.ReadLine();
                if (DateTime.TryParse(dateString, out date) == false)
                {
                    Console.WriteLine("Invalid option, try again");
                }
                else
                {
                    date = DateTime.Parse(dateString);
                    return date.Date;
                };
            }
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


        public static Fields AskField() {

            Console.WriteLine("\nChose specialization by number:\n");
            int i = 1;
            foreach (string field in Enum.GetNames(typeof(Fields)))
            {
                Console.WriteLine($"{i}. {field}");
                i++;
            }
            int choice = EnterNumberWithLimit(1, Enum.GetNames(typeof(Fields)).Length);
            Fields specialization = (Fields)(choice - 1);
            return specialization;


        }
    public static string MaskPassword()
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

        public static User Register(Roles role = Roles.Nobody)
        {
            string text = "", password, passwordCheck, temp;
            int option;

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

            switch(role)
            {
                case Roles.Nobody:
                    Console.WriteLine("Chose your role.");
                    Console.WriteLine("1: Admin");
                    Console.WriteLine("2: Secretary");
                    Console.WriteLine("3: Doctor");
                    Console.WriteLine("4: Patient");
                    option = EnterNumberWithLimit(1, 4);
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
            return new Users.Admin(text);
        }

        private static User RegisterSecretary(string text)
        {
            return new Secretary(text);
        }

        private static User RegisterDoctor(string text)
        {
            Fields field = AskField();            
            text += "|" + field.ToString();
            return new Doctor(text);
        }

        private static User RegisterPatient(string text)
        {
            text += "|Unblocked";
            return new Patient(text);
        }

        public static string Space(int length, string text)
        {
            string space = "";
            for (int i = 1; i <= length - text.Length; i++)
                space += " ";
            return space;
        }


        public static string LineInTable(bool withRole = false)
        {
            if (withRole)
                return "+----------------------+----------------------+----------------------+------------+-----------------+------------+";
            else
                return "+----------------------+----------------------+----------------------+------------+-----------------+";
        }

        public static string TableHeader(bool withRole = false)
        {
            if(withRole)
                return "| Username             | Name                 | Last Name            | Gender     | Date of Birth   | Role       |";
            else
                return "| Username             | Name                 | Last Name            | Gender     | Date of Birth   |";
        }

        public static void PrintUsers(bool withRole = false, Roles role = Roles.Nobody)
        {
            Console.WriteLine(LineInTable(withRole));
            Console.WriteLine(TableHeader(withRole));
            Console.WriteLine(LineInTable(withRole));
            foreach(KeyValuePair<string, User> pair in SystemFunctions.Users)
            {
                if(role == Roles.Nobody || pair.Value.Role == role)
                {
                    Console.WriteLine(pair.Value.TextInTable(withRole));
                    Console.WriteLine(LineInTable(withRole));
                }
            }
            Console.WriteLine();
        }
        public static DateTime GetGoodDate()
        {
            DateTime date;
            do
            {
                date = OtherFunctions.AskForDate();
                if (date.Date < DateTime.Now.Date)
                {
                    Console.WriteLine("You can't enter a date that's in the past");
                }
            } while (date.Date < DateTime.Now.Date);
            return date;
        }
        public static bool CheckForRenovations(DateRange examinationTime, int roomId)
        {
            foreach(var renovation in RoomRenovationManager.RoomRenovationList)
            {
                if (roomId == renovation.RoomId && renovation.Duration.IsOverlaping(examinationTime))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckForExaminations(DateRange dateRange, int roomId)
        {
            foreach (int examId in SystemFunctions.AllExamtinations.Keys )
            {
                Clinic.Examination exam = SystemFunctions.AllExamtinations[examId];
                if(exam.Doctor.RoomId == roomId && dateRange.IsOverlaping(new DateRange(exam.DateTime, exam.DateTime.AddMinutes(15))))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
