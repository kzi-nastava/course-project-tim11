using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Users
{
    class Nobody : User
    {
        public Nobody()
        {
            UserName = "";
            Password = "";
            Name = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gender = ' ';
            Role = Roles.Nobody;
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth + "|" + Gender + "|" + Role;
        }

        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log in");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");

            return 2;
        }

        public override void MenuDo(int option)
        {
            switch(option)
            {
                case 2:
                    Register();
                    break;
            }
        }

        public void Register()
        {

        }
    }
}
