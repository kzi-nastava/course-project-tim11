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

        public override int MenuWrite()
        {
            Console.Writeline("What would you like to do?");
            Console.Writeline("1: Log in");
            Console.Writeline("2: Register");
            Console.Writeline("0: Exit");
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
    }

    public void Register()
    {

    }
}
