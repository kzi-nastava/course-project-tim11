using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Users
{
    public class Patient : User
    {
        public Patient(int id, string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
            : base(id, userName, password, 3)
        {
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        public void PatientMenu() {
            Console.WriteLine("Patient menu: ");
        }
    }
}
