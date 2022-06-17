using ClinicApp.Clinic;
using ClinicApp.Clinic.Appointmens;
using ClinicApp.Clinic.Scheduling;
using ClinicApp.Users;
using System;

namespace ClinicApp.Menus.Doctors.Dialogs
{
    class AppointmentDialog
    {
        public static void CreateAppointment(ref Doctor doctor, bool patientCalled = false)
        {
            int type = 0;
            int duration = 0;
            if (!patientCalled)
            {
                Console.Write("\nDo you want to create an (1)EXAMINATION or an (2)OPERATION?\n>> ");
                type = OtherFunctions.EnterNumberWithLimit(0, 3);
                if (type == 2)
                {
                    Console.Write("\nEnter the duration of your Operation in minutes (e.g. 60)\n>> ");
                    duration = OtherFunctions.EnterNumberWithLimit(15, 1000);
                }
                else duration = 15;
            }
            DateTime dateTime = AskDateTime(duration, ref doctor);
            if (!FreeDaysRequestService.IsDoctorAvailible(dateTime, doctor)) {
                Console.WriteLine("You have free days at that time.");
                return;
            }
            Patient patient = OtherFunctions.AskUsernamePatient();
            if (patient == null) return;
            int id = Appointment.GetLastID();
            AppointmentService.CreateAppointment(ref doctor, id, dateTime, ref patient, duration, type);
            Console.WriteLine("\nNew appointment successfully created\n");

        }

        private static DateTime AskDateTime(int duration, ref Doctor doctor)
        {
            DateTime dateTime = DateTime.Now;
            Console.Write("\nEnter the date of your Appointment (e.g. 22/10/1987)\n>> ");
            DateTime date = OtherFunctions.GetGoodDate();
            Console.Write("\nEnter the time of your Appointment (e.g. 14:30)\n>> ");
            DateTime time;
            do
            {
                time = OtherFunctions.EnterTime();
                time = date.Date + time.TimeOfDay;
                if (time < DateTime.Now)
                {
                    Console.WriteLine("You can't enter that time, its in the past");
                }
                else
                {
                    if (AppointmentService.CheckAppointment(time, duration, ref doctor)) dateTime = time;
                    else { Console.WriteLine("You are not availible at that time."); };
                }
            } while (time < DateTime.Now && AppointmentService.CheckAppointment(time, duration, ref doctor));
            return dateTime;
        }

        public static void ViewAppointments(ref Doctor doctor)
        {
            if (doctor.Appointments.Count == 0)
            {
                Console.WriteLine("\nNo future appointments\n");
                return;
            }
            int i = 1;
            string type;
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (appointment.Type == 'e') type = "Examination";
                else type = "Operation";
                Console.WriteLine($"\n\n{i}. {type}\n\nId: {appointment.ID};\nTime and Date: {appointment.DateTime};\nDuration: {appointment.Duration}min;\nPatient last name: {appointment.Patient.LastName}; Patient name: {appointment.Patient.Name}\n");
                i++;
            }

        }
        public static void EditAppointment(ref Doctor doctor)
        {

            Appointment appointment = DoctorDialog.GetAppointmentByID(doctor);
            Console.WriteLine("Do you want to edit the date or the time? (d/t)");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "D")
            {
                EditDate(appointment, ref doctor);
            }
            else if (choice.ToUpper() == "T")
            {
                EditTime(appointment, ref doctor);
            }
            else
            {
                Console.WriteLine("Not a valid choice.");
            }
        }

        public static void EditDate(Appointment appointment, ref Doctor doctor)
        {
            Console.WriteLine("Enter the new date of your Appointment (e.g. 22/10/1987)");

            DateTime newDate = OtherFunctions.GetGoodDate();
            newDate += appointment.DateTime.TimeOfDay;
            if (AppointmentService.CheckAppointment(newDate, appointment.Duration, ref doctor))
            {
                if (!FreeDaysRequestService.IsDoctorAvailible(newDate, doctor))
                {
                    Console.WriteLine("You have free days at that time.");
                    return;
                }
                AppointmentService.ChangeAppointment(appointment, newDate);
            }
            else
            {
                Console.WriteLine("You are not availible at that time.");
                return;
            }

        }
        private static void EditTime(Appointment appointment, ref Doctor doctor)
        {
            Console.WriteLine("Enter the new time of your Examination (e.g. 12:00)");
            DateTime newTime;
            do
            {
                newTime = OtherFunctions.EnterTime();
                newTime = appointment.DateTime.Date + newTime.TimeOfDay;
                if (newTime < DateTime.Now)
                {
                    Console.WriteLine("You can't enter that time, its in the past");
                }
                else
                {
                    if (AppointmentService.CheckAppointment(newTime, appointment.Duration, ref doctor))
                    {
                        AppointmentService.ChangeAppointment(appointment, newTime);
                    }
                    else
                    {
                        Console.WriteLine("You are not availible at that time.");
                        return;
                    }
                }
            } while (newTime < DateTime.Now);

        }
        public static void DeleteAppointment(ref Doctor doctor)
        {
            Console.WriteLine("Enter the ID of the appointment you wish to delete.");
            int id = OtherFunctions.EnterNumber();
            Appointment appointment = null;
            foreach (Appointment tmp in doctor.Appointments)
            {
                if (tmp.ID == id)
                {
                    appointment = tmp;
                    Console.WriteLine("Are you sure? (y/n)");
                    string choice = Console.ReadLine();
                    if (choice.ToUpper() == "Y")
                    {
                        AppointmentService.DeleteAppointment(ref doctor, ref appointment);
                    }
                    break;
                }
            }
        }

    }
}
