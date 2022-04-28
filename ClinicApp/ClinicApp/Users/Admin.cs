using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Users
{
    public class Admin : User 
    {
        public Admin(int id, string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
            : base(id, userName, password, 2)
        {
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        internal void AdminMenu()
        {
            Console.WriteLine("Admin menu: ");
        }
    }
}
