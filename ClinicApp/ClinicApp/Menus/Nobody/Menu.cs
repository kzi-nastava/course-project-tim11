using System;
using ClinicApp.Users;

namespace ClinicApp.Menus.Nobody
{
    class Menu
    {
        public static int Write(User nobody)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log in");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");

            return 2;
        }

        public static void Do(User nobody, int option)
        {
            //Nobody isn't supposed to do anything.
            return;
        }
    }
}
