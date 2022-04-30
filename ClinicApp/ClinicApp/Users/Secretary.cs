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
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Manage patient accounts");
            Console.WriteLine("0: Exit");

            return 2;
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
            int option = 1, option2, numberOfOptions = 4;
            User tempUser;
            while (option != 0)
            {
                Console.WriteLine("\nWhat would you like to do?");
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
                    case 2:
                        OtherFunctions.PrintUsers(role : Roles.Patient);
                        break;
                    case 3:
                        option2 = 1;
                        while (option2 != 0)
                        {
                            Console.WriteLine("\nWrite the username of the patient who's account you want deleted:");
                            string userName = OtherFunctions.EnterString();
                            if (SystemFunctions.Users.TryGetValue(userName, out tempUser))
                            {
                                if (tempUser.Role == Roles.Patient)
                                {
                                    UpdatePatient((Patient)tempUser);
                                }
                                else
                                {
                                    Console.WriteLine("\nThis account doesn't belong to a patient. Want to try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThere is no account with this username. Want to try again?");
                                Console.WriteLine("1: Yes");
                                Console.WriteLine("0: No");
                                option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                            }
                        }
                        break;
                    case 4:
                        option2 = 1;
                        while(option2 != 0)
                        {
                            Console.WriteLine("\nWrite the username of the patient who's account you want deleted:");
                            string userName = OtherFunctions.EnterString();
                            if (SystemFunctions.Users.TryGetValue(userName, out tempUser))
                            {
                                if (tempUser.Role == Roles.Patient)
                                {
                                    SystemFunctions.Users.Remove(userName);
                                    SystemFunctions.Patients.Remove(userName);
                                }
                                else
                                {
                                    Console.WriteLine("\nThis account doesn't belong to a patient. Want to try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThere is no account with this username. Want to try again?");
                                Console.WriteLine("1: Yes");
                                Console.WriteLine("0: No");
                                option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                            }
                        }
                        break;
                }
            }
        }

        private static void UpdatePatient(Patient patient)
        {
            int option = 1;
            string temp;

            while(option != 0)
            {
                patient.Print();
                Console.WriteLine("\nWhat would you like to change?");
                Console.WriteLine("1: Username");
                Console.WriteLine("2: Password");
                Console.WriteLine("3: Name");
                Console.WriteLine("4: Last name");
                Console.WriteLine("5: Gender");
                Console.WriteLine("6: Date of birth");
                Console.WriteLine("0: Back to menu");
                option = OtherFunctions.EnterNumberWithLimit(0, 6);

                switch(option)
                {
                    case 1:
                        Console.Write("Username: ");
                        temp = OtherFunctions.EnterString();
                        while (SystemFunctions.Users.ContainsKey(temp))
                        {
                            Console.WriteLine("This username is taken. Please, try again.");
                            Console.Write("Username: ");
                            temp = OtherFunctions.EnterString();
                        }
                        patient.UserName = temp;
                        break;
                }
            }
        }
    }
}
