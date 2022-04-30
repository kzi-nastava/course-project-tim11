﻿using System;
using System.Collections.Generic;
using System.IO;

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
            Console.WriteLine("3: Block or unbolck patient accounts");
            Console.WriteLine("4: Manage examination requests");
            Console.WriteLine("0: Exit");

            return 4;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    PatientsCRUD();
                    break;
                case 3:
                    ManageBlockedPatients();
                    break;
                case 4:
                    ManageBlockedPatients();
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
                    case 2:
                        string password, passwordCheck;
                        Console.Write("Password: ");
                        password = OtherFunctions.MaskPassword();
                        Console.Write("\nRepeat password: ");
                        passwordCheck = OtherFunctions.MaskPassword();
                        while (password != passwordCheck)
                        {
                            Console.WriteLine("Passwords don't match. Please, try again.");
                            Console.Write("Password: ");
                            password = OtherFunctions.MaskPassword();
                            Console.Write("\nRepeat password: ");
                            passwordCheck = OtherFunctions.MaskPassword();
                        }
                        patient.Password = password;
                        break;
                    case 3:
                        Console.Write("\nName: ");
                        patient.Name = OtherFunctions.EnterString();
                        break;
                    case 4:
                        Console.Write("\nLast name: ");
                        patient.LastName = OtherFunctions.EnterString();
                        break;
                    case 5:
                        Console.Write("Gender (m/f/n): ");
                        temp = OtherFunctions.EnterString();
                        while (temp != "m" && temp != "f" && temp != "n")
                        {
                            Console.Write("You didn't enter a valid option. Please, try again (m/f/n): ");
                            temp = OtherFunctions.EnterString();
                        }
                        patient.Gender = temp[0];
                        break;
                    case 6:
                        Console.Write("Date of birth (e.g. 02/05/1984): ");
                        patient.DateOfBirth = OtherFunctions.AskForDate();
                        break;
                }
            }
        }

        private static void ManageBlockedPatients()
        {
            int option = 1, numberOfOptions = 4;
            string username;
            Patient patient;
            while (option != 0)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1: List patient accounts");
                Console.WriteLine("2: Block a patient accounts");
                Console.WriteLine("3: Unblock a patient account");
                Console.WriteLine("0: Back to menue");
                Console.Write(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                Console.WriteLine();
                switch (option)
                {
                    case 1:
                        Console.WriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        Console.WriteLine(OtherFunctions.TableHeader() + " Blocked by      |");
                        Console.WriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        foreach (KeyValuePair<string, Patient> pair in SystemFunctions.Patients)
                        {
                            Console.WriteLine(pair.Value.TextInTable() + " " + pair.Value.Blocked.ToString() + OtherFunctions.Space(15, pair.Value.Blocked.ToString()) + " |");
                            Console.WriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        }
                        Console.WriteLine();
                        break;
                    case 2:
                        Console.WriteLine("\nEnter the username of the account you want to block:");
                        username = OtherFunctions.EnterString();
                        if(SystemFunctions.Patients.TryGetValue(username, out patient))
                            if(patient.Blocked == Blocked.Unblocked)
                                patient.Blocked = Blocked.Secretary;
                            else
                                Console.WriteLine("This patient's account is already blocked.");
                        else
                            Console.WriteLine("There is no patient's account with such username.");
                        break;
                    case 3:
                        Console.WriteLine("\nEnter the username of the account you want to unblock:");
                        username = OtherFunctions.EnterString();
                        if(SystemFunctions.Patients.TryGetValue(username, out patient))
                            if(patient.Blocked != Blocked.Unblocked)
                                patient.Blocked = Blocked.Unblocked;
                            else
                                Console.WriteLine("This patient's account is already unblocked.");
                        else
                            Console.WriteLine("There is no patient's account with such username.");
                        break;
                }
            }
        }

        private static void ManageExaminationRequests()
        {
            int id;
            Clinic.Examination examination;

            Console.WriteLine();
            using (StreamReader reader = new StreamReader(SystemFunctions.PatientRequestsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    Console.WriteLine("1: Approve");
                    Console.WriteLine("2: Deny");
                    option = OtherFunctions.EnterNumberWithLimit(1, 2);
                    if(option == 1)
                    {
                        if (!Int32.TryParse(line.Split("|")[0], out id))
                            id = -1;
                        if (line.Split("|")[1] == "DELETE")
                            SystemFunctions.AllExamtinations.Remove(id);
                        else if (line.Split("|")[1] == "UPDATE" && SystemFunctions.AllExamtinations.TryGetValue(id, out examination))
                        {

                            examination.DateTime = DateTime.Parse(line.Split("|")[2]);
                            Doctor doctor;
                            if (SystemFunctions.Doctors.TryGetValue(line.Split("|")[3], out doctor))
                            {
                                examination.Doctor.Examinations.Remove(examination);
                                examination.Doctor = doctor;
                                examination.Doctor.Examinations.Add(examination);
                            }
                        }
                    }
                }
            }
            File.Delete(SystemFunctions.PatientRequestsFilePath);
            Console.WriteLine();
        }
    }
}
