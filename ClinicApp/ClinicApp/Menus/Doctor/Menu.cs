using System;
using ClinicApp.Users;
using ClinicApp.Dialogs;

namespace ClinicApp.Menus.Doctors
{
    class Menu
    {
        public static int Write(User doctor)
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + doctor.MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage examinations");
            Console.WriteLine("4: View schedule");
            Console.WriteLine("5: Manage medicine");
            Console.WriteLine("6: Request free days");
            Console.WriteLine("7: View your requests for free days");
            Console.WriteLine("0: Exit");

            return 7;
        }

        public static void Do(User doctor, int option)
        {
            Users.Doctor doctorTemp = (Users.Doctor)doctor;
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
                    FreeDaysRequestDialog.GatherInfoFreeDayRequest(ref doctorTemp);
                    break;
                case 7:
                    FreeDaysRequestDialog.ViewFreeDays(doctorTemp);
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
    }
}
