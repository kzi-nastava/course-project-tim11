using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.HelperClasses;

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
            MessageBox = new MessageBox(this);
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        public override int MenuWrite()
        {
            return Menu.NobodyMenuWrite(this);
        }

        public override void MenuDo(int option)
        {
            Menu.NobodyMenuDo(this, option);
        }
    }
}
