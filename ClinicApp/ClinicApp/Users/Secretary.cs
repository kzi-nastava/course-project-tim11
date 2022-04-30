using System;
using System.Collections.Generic;
using System.Text;

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
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Manage patient accounts");
            Console.WriteLine("0: Exit");

            return 1;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    PatientsCRUD();
                    break;
            }
        }

        private void PatientsCRUD()
        {
            int option = 1, numberOfOptions = 4;
            while (option != 0)
            {
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1: Create a patient account");
                Console.WriteLine("2: View all patient accounts");
                Console.WriteLine("3: Update a patient account");
                Console.WriteLine("4: Delete a patient account");
                Console.WriteLine("0: Back to menue");
                Console.Write(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                Console.WriteLine();
                switch(option)
                {
                    case 1:
                        User patient = OtherFunctions.Register(Roles.Patient);
                        SystemFunctions.Users.Add(patient.UserName, patient);
                        SystemFunctions.Patients.Add(patient.UserName, (Patient)patient);
                        break;
                }
            }
        }
    }
}
