using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Users;

namespace ClinicApp
{
    public class Menu
    {
        //======================================================================================================
        //Users' menus

        //Nobody
        public static int NobodyMenuWrite(User nobody)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log in");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");

            return 2;
        }

        public static void NobodyMenuDo(User nobody, int option)
        {
            //Nobody isn't supposed to do anything.
            return;
        }

        //Admin
        public static int AdminMenuWrite(User admin)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + admin.MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage Clinic Rooms");
            Console.WriteLine("4: Manage Clinic Equipment");
            Console.WriteLine("5: Manage Room Renovations");
            Console.WriteLine("6: Manage Medicines");
            Console.WriteLine("0: Exit");

            return 6;
        }

        public static void AdminMenuDo(User admin, int option)
        {
            switch (option)
            {
                case 2:
                    admin.MessageBox.DisplayMessages();
                    break;
                case 3:
                    Menus.Admin.RoomManagment.Menu();
                    break;
                case 4:
                    Menus.Admin.EquipmentManagment.Menu();
                    break;
                case 5:
                    Menus.Admin.RoomRenovation.Menu();
                    break;
                case 6:
                    Menus.Admin.Medicines.Menu();
                    break;
            }
        }

        //Secretary
        public static int SecretaryMenuWrite(User secretary)
        {
            CLI.CLIWriteLine("\nWhat would you like to do?");
            CLI.CLIWriteLine("1: Log out");
            CLI.CLIWriteLine("2: Display new messages (" + secretary.MessageBox.NumberOfMessages + ")");
            CLI.CLIWriteLine("3: Manage patient accounts");
            CLI.CLIWriteLine("4: Block or unbolck patient accounts");
            CLI.CLIWriteLine("5: Manage examination requests");
            CLI.CLIWriteLine("6: Create examinations based upon referrals");
            CLI.CLIWriteLine("7: Create an emergency examination");
            CLI.CLIWriteLine("8: Make an order of dynamic equipment");
            CLI.CLIWriteLine("9: Redistribute dynamic equipment");
            CLI.CLIWriteLine("0: Exit");

            return 9;
        }

        public static void SecretaryMenuDo(User secretary, int option)
        {
            switch (option)
            {
                case 2:
                    secretary.MessageBox.DisplayMessages();
                    break;
                case 3:
                    PatientsCRUDMenu();
                    break;
                case 4:
                    Secretary.ManageBlockedPatients();
                    break;
                case 5:
                    Secretary.ManageExaminationRequests();
                    break;
                case 6:
                    Secretary.CreateExaminationsFromReferrals();
                    break;
                case 7:
                    Secretary.CreateEmergencyExamination();
                    break;
                case 8:
                    EquipmentService.OrderDynamiicEquipment();
                    break;
                case 9:
                    EquipmentService.RedistributeDynamiicEquipment();
                    break;
            }
        }

        //Doctor
        public static int DoctorMenuWrite(User doctor)
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + doctor.MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage examinations");
            Console.WriteLine("4: View schedule");
            Console.WriteLine("5: Manage medicine");
            Console.WriteLine("0: Exit");

            return 5;
        }

        public static void DoctorMenuDo(User doctor, int option)
        {
            Doctor doctorTemp = (Doctor) doctor;
            switch (option)
            {
                case 2:
                    doctor.MessageBox.DisplayMessages();
                    break;
                case 3:
                    doctorTemp.ManageAppointments();
                    break;
                case 4:
                    doctorTemp.ViewSchedule();
                    break;
                case 5:
                    doctorTemp.ManageMedicine();
                    break;
                default:
                    break;
            }
        }

        //Patient
        public static int PatientMenuWrite(User patient)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + patient.MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Make appointment");
            Console.WriteLine("4: Edit appointment");
            Console.WriteLine("5: Cancel appointment");
            Console.WriteLine("6: View appointments");
            Console.WriteLine("7: Appointment suggestion");
            Console.WriteLine("8: View health record history");
            Console.WriteLine("9: Search doctors");
            Console.WriteLine("0: Exit");

            return 9;
        }

        public static void PatientMenuDo(User patient, int option)
        {
            switch (option)
            {
                case 2:
                    patient.MessageBox.DisplayMessages();
                    break;
                case 3:
                    CreateExamination();
                    break;
                case 4:
                    EditExamination();
                    break;
                case 5:
                    DeleteExamination();
                    break;
                case 6:
                    ViewExaminations();
                    break;
                case 7:
                    SuggestAppointment();
                    break;
                case 8:
                    ViewAnamnesis();
                    break;
            }
        }

        //======================================================================================================
        //Other Menus

        //Manages the Patient CRUD.
        public static void PatientsCRUDMenu()
        {
            int option = 1, numberOfOptions = 4;
            User patient;

            while (option != 0)
            {
                CLI.CLIWriteLine("\nWhat would you like to do?");
                CLI.CLIWriteLine("1: Create a patient account");
                CLI.CLIWriteLine("2: View all patient accounts");
                CLI.CLIWriteLine("3: Update a patient account");
                CLI.CLIWriteLine("4: Delete a patient account");
                CLI.CLIWriteLine("0: Back to menu");
                option = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
                CLI.CLIWriteLine();
                switch (option)
                {
                    //Create
                    case 1:
                        PatientService.CreatePatient();
                        break;
                    //Read
                    case 2:
                        OtherFunctions.PrintUsers(role: Roles.Patient);
                        break;
                    //Update
                    case 3:
                        patient = OtherFunctions.FindUser(Roles.Patient);
                        if (patient != null)
                            PatientService.UpdatePatient((Patient)patient);
                        break;
                    //Delete
                    case 4:
                        patient = OtherFunctions.FindUser(Roles.Patient);
                        if (patient != null)
                            PatientService.DeletePatient((Patient)patient);
                        break;
                }
            }
        }
    }
}
