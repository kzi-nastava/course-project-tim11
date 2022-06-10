using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.AdminFunctions;
using ClinicApp.Clinic;
using ClinicApp.HelperClasses;

namespace ClinicApp.Users
{
    public class Secretary : User
    {
        public Secretary(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Secretary;
            MessageBox = new MessageBox(this);
        }

        public Secretary(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Secretary;
            MessageBox = new MessageBox(this);
        }

        //Compresses a Secretary object into a string for easier upload.
        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        //Writes all the options a secretary has once he logs in.
        public override int MenuWrite()
        {
            return Menu.SecretaryMenuWrite(this);
        }

        //Executes the chosen command.
        public override void MenuDo(int option)
        {
            Menu.SecretaryMenuDo(this, option);
        }
    }
}
