﻿using ClinicApp.Users;
using System;

namespace ClinicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // First we load all data from our data bases into the working memorry
            // (implemented using the SystemFunctions class)
            SystemFunctions.LoadData();

            // Next we communicate with our user
            int option = 1, numberOfOptions;
            User currentUser = new Nobody();
            while(option != 0)
            {
                numberOfOptions = currentUser.MenuWrite();
                Console.Write(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                Console.WriteLine();
                if (option == 1)
                {
                    if (currentUser.Role == Roles.Nobody)
                    {
                        currentUser = OtherFunctions.LogIn();
                    }
                    else
                        currentUser = new Nobody();
                }
                else if (option == 2 && currentUser.Role == Roles.Nobody)
                {
                    currentUser = OtherFunctions.Register();
                    if(currentUser.Role != Roles.Nobody)
                    {
                        SystemFunctions.Users.Add(currentUser.UserName, currentUser);
                        switch(currentUser.Role)
                        {
                            case Roles.Patient:
                                SystemFunctions.Patients.Add(currentUser.UserName, (Patient)currentUser);
                                Patient patient = (Patient)currentUser;
                                patient.AntiTroll();
                                break;
                            case Roles.Doctor:
                                SystemFunctions.Doctors.Add(currentUser.UserName, (Doctor)currentUser);
                                break;
                        }
                        Console.WriteLine($"\nWelcome {currentUser.UserName}\n");
                    }
                }
                else if (option > 0)
                    currentUser.MenuDo(option);
            }

            // Finally, we upload all the data to the database
            SystemFunctions.UploadData();
        }
    }
}
