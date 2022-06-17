using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Users;

namespace ClinicApp.Menus.Patients
{
    class Menu
    {
        public static int Write(User patient)
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

        public static void Do(User patient, int option)
        {
            switch (option)
            {
                case 2:
                    patient.MessageBox.DisplayMessages();
                    break;
                case 3:
                    //CreateExamination();
                    break;
                case 4:
                    //EditExamination();
                    break;
                case 5:
                    //DeleteExamination();
                    break;
                case 6:
                    //ExaminationService.ViewExaminations((Patient)patient);
                    break;
                case 7:
                    //SuggestAppointment();
                    break;
                case 8:
                    //ViewAnamnesis();
                    break;
            }
        }
    }
}
