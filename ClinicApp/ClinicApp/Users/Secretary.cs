using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Users
{
    public class Secretary : User
    {
        public Secretary(int id, string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
            : base(id, userName, password, 1)
        {
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender; 
        }

        public void SecretaryMenu() {
            Console.WriteLine("Secretary menu: ");
        }
    }
}
