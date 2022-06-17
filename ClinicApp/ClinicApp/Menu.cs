using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;
using ClinicApp.Clinic;
using ClinicApp.Dialogs;
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

            return 7;
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
                    Menus.Admin.RoomRenovations.Menu();
                    break;
                case 6:
                    Menus.Admin.Medicines.Menu();
                    break;
                case 7:
                    Menus.Admin.Surveys.Menu();
                    break;
            }
        }

        //Secretary
        public static int SecretaryMenuWrite(User secretary)
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + secretary.MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage patient accounts");
            Console.WriteLine("4: Block or unbolck patient accounts");
            Console.WriteLine("5: Manage examination requests");
            Console.WriteLine("6: Create examinations based upon referrals");
            Console.WriteLine("7: Create an emergency examination");
            Console.WriteLine("8: Make an order of dynamic equipment");
            Console.WriteLine("9: Redistribute dynamic equipment");
            Console.WriteLine("0: Exit");

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
                    PatientService.ManageBlockedPatients();
                    break;
                case 5:
                    PatientService.ManageExaminationRequests();
                    break;
                case 6:
                    ExaminationService.CreateExaminationsFromReferrals();
                    break;
                case 7:
                    ExaminationService.CreateEmergencyExamination();
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
            Console.WriteLine("6: Request free days");
            Console.WriteLine("0: Exit");

            return 6;
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
                    AppointmentsMenu(ref doctorTemp);
                    break;
                case 4:
                    DoctorDialog.ViewSchedule(ref doctorTemp);
                    break;
                case 5:
                    DoctorDialog.ReviewMedicineRequests();
                    break;
                case 6:
                    DoctorDialog.GatherInfoFreeDayRequest(ref doctorTemp);
                    break;
                default:
                    break;
            }
        }
        public static void AppointmentsMenu(ref Doctor doctor)
        {
            Console.WriteLine("Chose how you wish to manage your appointments: ");
            string options = "\n1. Create\n2. View\n3. Edit(by ID)\n4. Delete(by ID)\n";
            CLI.CLIWrite($"{options}Write the number of your choice\n>> ");
            int choice = CLI.CLIEnterNumberWithLimit(1, 4);
            switch (choice)
            {
                case 1:
                    AppointmentDialog.CreateAppointment(ref doctor);
                    break;
                case 2:
                    AppointmentDialog.ViewAppointments(ref doctor);
                    break;
                case 3:
                    AppointmentDialog.EditAppointment(ref doctor);
                    break;
                case 4:
                    AppointmentDialog.DeleteAppointment(ref doctor);
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

        /*public static void PatientMenuDo(User patient, int option)
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
                    ExaminationService.ViewExaminations((Patient)patient);
                    break;
                case 7:
                    SuggestAppointment();
                    break;
                case 8:
                    ViewAnamnesis();
                    break;
            }
        }*/

        //======================================================================================================
        //Other Menus

        //Manages the Patient CRUD.
        public static void PatientsCRUDMenu()
        {
            int option = 1, numberOfOptions = 4;
            User patient;

            while (option != 0)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1: Create a patient account");
                Console.WriteLine("2: View all patient accounts");
                Console.WriteLine("3: Update a patient account");
                Console.WriteLine("4: Delete a patient account");
                Console.WriteLine("0: Back to menu");
                option = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
                Console.WriteLine();
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
                        patient = OtherFunctions.FindUser("\nEnter the username of the patient whose account you want to update:", Roles.Patient);
                        if (patient != null)
                            PatientService.UpdatePatient((Patient)patient);
                        break;
                    //Delete
                    case 4:
                        patient = OtherFunctions.FindUser("\nEnter the username of the patient whose account you want to delete:", Roles.Patient);
                        if (patient != null)
                            PatientService.DeletePatient((Patient)patient);
                        break;
                }
            }
        }
    }
}
