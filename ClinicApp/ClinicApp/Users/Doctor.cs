using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Users
{
    public class Doctor : User
    {
        public List<Examination> Examinations { get; set; }

        public Doctor(int id, string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
            :base (id, userName, password, 2) {
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Examinations = new List<Examination>();
        }
        public void DoctorMenu() {
            Console.WriteLine("Doctor menu: ");
        }


    }
}
