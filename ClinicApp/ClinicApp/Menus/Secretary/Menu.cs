using System;
using ClinicApp.Users;

namespace ClinicApp.Menus.Secretary
{
    class Menu
    {
        public static int Write(User secretary)
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

        public static void Do(User secretary, int option)
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
                    Clinic.ExaminationService.CreateExaminationsFromReferrals();
                    break;
                case 7:
                    Clinic.ExaminationService.CreateEmergencyExamination();
                    break;
                case 8:
                    EquipmentService.OrderDynamiicEquipment();
                    break;
                case 9:
                    EquipmentService.RedistributeDynamiicEquipment();
                    break;
            }
        }
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
