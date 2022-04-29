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
                option = EnterNumber();
                while(option < 0 || option > numberOfOptions)
                {
                    Console.WriteLine("You didn't enter a valid option. Try again.");
                    option = EnterNumber();
                }
                if (option == numberOfOptions)
                    currentUser = new Nobody();
                else if(option > 0)
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
    }
}
