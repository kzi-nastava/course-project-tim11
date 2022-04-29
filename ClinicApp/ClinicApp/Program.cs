using ClinicApp.Users;
using System;

namespace ClinicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // First we load all data from our data bases into the working memorry
            // (implemented using the SystemFunctions class)
            SystemFunctions.LoadData();

            // Next we communicate with our user
            int option = 1, numberOfOptions;
            User currentUser = new Nobody();
            while(option != 0)
            {
                numberOfOptions = currentUser.MenuWrite();
                option = EnterNumberWithLimit(numberOfOptions);
                if (option == 1)
                {
                    if (currentUser.Role == Roles.Nobody)
                        currentUser = LogIn();
                    else
                        currentUser = new Nobody();
                }
                else if (option == 2 && currentUser.Role == Roles.Nobody)
                    currentUser = Register();
                else if (option > 0)
                    currentUser.MenuDo(option);
            }

            // Finally, we upload all the data to the database
            // ...
        }

        public static int EnterNumber()
        {
            int x = -1;
            string s;
            while(true)
            {
                s = Console.ReadLine();
                try
                {
                    x = Int32.Parse(s);
                }
                catch(Exception e)
                {
                    Console.WriteLine("You didn't enter a number. Try again.");
                }
                return x;
            }
        }

        public static int EnterNumberWithLimit(int limit)
        {
            if (limit < 0)
                limit = 0;
            int option = EnterNumber();
            while (option < 0 || option > limit)
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
            while(option != 0)
            {
                Console.Write("Username: ");
                string userName = Console.ReadLine();
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
                        option = EnterNumberWithLimit(1);
                    }
                }
                else
                {
                    Console.WriteLine("\nUser does not exist. Want to try again?");
                    Console.WriteLine("1: Yes");
                    Console.WriteLine("0: No");
                    option = EnterNumberWithLimit(1);
                }
            }
            
            return user;
        }

        public static User Register()
        {
            User user = new Nobody();


            return user;
        }
    }
}
