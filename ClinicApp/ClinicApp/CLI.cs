using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ClinicApp.Clinic;

namespace ClinicApp
{
    public class CLI
    {
        public static void CLIWrite(string text = "")
        {
            Console.Write(text);
        }



        public static void CLIWriteLine(string text = "")
        {
            Console.WriteLine(text);
        }

        public static string CLIEnterString()
        {
            string input = Console.ReadLine();
            input = input.Trim();
            while (input == "")
            {
                CLIWriteLine("You have to write something.");
                input = Console.ReadLine();
                input = input.Trim();
            }
            return input;
        }

        public static string CLIEnterPassword()
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
                        CLIWrite("\b \b");
                    }
                }
                else if (key.Key == ConsoleKey.Enter && password.Length > 0)
                {
                    return password;
                }
                else
                {
                    password += key.KeyChar;
                    CLIWrite("*");
                }
            }
        }

        public static string CLIEnterStringWithoutDelimiter(string delimiter)
        {
            string input = CLIEnterString();
            while (input.Contains(delimiter))
            {
                CLIWriteLine("Invalid option, cannot contain " + delimiter + ", try again.");
                input = CLIEnterString();
            }
            return input;
        }

        public static int CLIEnterNumber()
        {
            int x = -1;
            string s;
            while (true)
            {
                s = CLIEnterString();
                try
                {
                    x = Int32.Parse(s);
                }
                catch
                {
                    CLIWriteLine("You didn't enter a number. Try again.");
                }
                return x;
            }
        }

        public static int CLIEnterNumberWithLimit(int lowerLimit, int upperLimit)
        {
            if (upperLimit < lowerLimit)
                upperLimit = lowerLimit;
            int option = CLIEnterNumber();
            while (option < lowerLimit || option > upperLimit)
            {
                CLIWriteLine("You didn't enter a valid option. Try again.");
                option = CLIEnterNumber();
            }

            return option;
        }

        public static double CLIEnterDouble()
        {
            double x = -1;
            string s;
            while (true)
            {
                s = CLIEnterString();
                try
                {
                    x = Convert.ToDouble(s);
                }
                catch (Exception)
                {
                    CLIWriteLine("You didn't enter a decimal number. Try again.");
                }
                return x;
            }
        }
        public static int GetValidEquipmentId()
        {
            Equipment eq;
            int id = CLI.CLIEnterNumber();
            eq = EquipmentRepository.Get(id);
            while (eq is null)
            {
                CLI.CLIWriteLine("Invalid ID");
                id = CLI.CLIEnterNumber();
                eq = EquipmentRepository.Get(id);
            }
            return id;
        }
        public static int GetValidRoomId()
        {
            Room room;
            int id = CLI.CLIEnterNumber();
            room = RoomRepository.Get(id);
            while (room is null)
            {
                CLI.CLIWriteLine("Invalid ID");
                id = CLI.CLIEnterNumber();
                room = RoomRepository.Get(id);
            }
            return id;
        }
        public static DateTime CLIEnterDate()
        {
            DateTime date;
            string s;
            while (true)
            {
                s = CLIEnterString();
                if (DateTime.TryParse(s, out date) == true)
                {
                    return date.Date;
                }
                else
                {
                    CLIWriteLine("Invalid option, try again");
                }
            }
        }

        public static DateTime CLIEnterNonPastDate()
        {
            DateTime date;
            do
            {
                date = CLIEnterDate();
                if (date.Date < DateTime.Now.Date)
                {
                    CLIWriteLine("You can't enter a date that's in the past");
                }
            } while (date.Date < DateTime.Now.Date);
            return date;
        }

        public static DateTime CLIEnterTime()
        {
            DateTime? time = null;
            string format = "HH:mm";
            while (time == null)
            {

                CultureInfo provider = CultureInfo.InvariantCulture;
                try
                {
                    time = DateTime.ParseExact(Console.ReadLine(), format, provider);
                }
                catch (FormatException)
                {
                    CLIWriteLine("\nIncorrect time format, try again");
                }
            }
            return (DateTime)time;
        }
    }
}
