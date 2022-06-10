using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicApp.Clinic
{
    public class AppointmentService
    {
        //=======================================================================================================================================================================
        // CREATE

        public static void CreateAppointment(ref Doctor doctor)
        {
            int type = 0;
            int duration = 0;
            CLI.CLIWrite("\nDo you want to create an (1)EXAMINATION or an (2)OPERATION?\n>> ");
            type = CLI.CLIEnterNumberWithLimit(0, 3);
            if (type == 2)
            {
                CLI.CLIWrite("\nEnter the duration of your Operation in minutes (e.g. 60)\n>> ");
                duration = CLI.CLIEnterNumberWithLimit(15, 1000);
            }
            else duration = 15;

            DateTime dateTime = AskDateTime(duration, ref doctor);
            Patient patient = OtherFunctions.AskUsernamePatient();
            if (patient == null) return;
            int id = Appointment.GetLastID();

            Appointment appointment;
            if (type == 1)
            {
                appointment = new Examination(id, dateTime, doctor, patient, false, 0, 0);
            }
            else
            {
                appointment = new Operation(id, dateTime, doctor, patient, false, 0, 0, duration);
            };

            DoctorService.InsertAppointment(appointment, ref doctor);
            PatientService.InsertAppointmentPatient(ref patient, appointment);
            AppointmentRepo.AllAppointments.Add(id, appointment);
            AppointmentRepo.CurrentAppointments.Add(id, appointment);
            CLI.CLIWriteLine("\nNew appointment successfully created\n");

        }

        private static DateTime AskDateTime(int duration, ref Doctor doctor)
        {
            DateTime dateTime = DateTime.Now;

            CLI.CLIWrite("\nEnter the date of your Appointment (e.g. 22/10/1987)\n>> ");

            DateTime date = OtherFunctions.GetGoodDate();

            CLI.CLIWrite("\nEnter the time of your Appointment (e.g. 14:30)\n>> ");

            DateTime time;

            do
            {
                time = CLI.CLIEnterTime();
                time = date.Date + time.TimeOfDay;
                if (time < DateTime.Now)
                {
                    CLI.CLIWriteLine("You can't enter that time, its in the past");
                }
                else
                {
                    if (DoctorService.CheckAppointment(time, duration, ref doctor)) dateTime = time;
                    else { CLI.CLIWriteLine("You are not availible at that time."); };
                }
            } while (time < DateTime.Now && DoctorService.CheckAppointment(time, duration, ref doctor));

            return dateTime;

        }
        //=======================================================================================================================================================================
        // READ

        public static void ViewAppointments(ref Doctor doctor)
        {

            if (doctor.Appointments.Count == 0)
            {
                CLI.CLIWriteLine("\nNo future appointments\n");
                return;
            }

            int i = 1;
            DateTime now = DateTime.Now;
            string type;
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (appointment.Type == 'e') type = "Examination";
                else type = "Operation";
                CLI.CLIWriteLine($"\n\n{i}. {type}\n\nId: {appointment.ID};\nTime and Date: {appointment.DateTime};\nDuration: {appointment.Duration}min;\nPatient last name: {appointment.Patient.LastName}; Patient name: {appointment.Patient.Name}\n");

                i++;
            }

        }
        //=======================================================================================================================================================================
        // UPDATE
        public static void EditAppointment(ref Doctor doctor)
        {

            Appointment appointment = DoctorService.GetAppointmentByID(ref doctor);
            CLI.CLIWriteLine("Do you want to edit the date or the time? (d/t)");
            string choice = CLI.CLIEnterString();
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
                CLI.CLIWriteLine("Not a valid choice.");
            }
        }

        

        public static void EditDate(Appointment appointment, ref Doctor doctor)
        {
            CLI.CLIWriteLine("Enter the new date of your Appointment (e.g. 22/10/1987)");

            DateTime newDate = OtherFunctions.GetGoodDate();
            newDate += appointment.DateTime.TimeOfDay;
            if (DoctorService.CheckAppointment(newDate, appointment.Duration, ref doctor))
            {

                ChangeAppointment(appointment, newDate);
            }
            else
            {
                CLI.CLIWriteLine("You are not availible at that time.");
                return;
            }

        }
        private static void EditTime(Appointment appointment, ref Doctor doctor)
        {
            CLI.CLIWriteLine("Enter the new time of your Examination (e.g. 12:00)");
            DateTime newTime;
            do
            {
                newTime = CLI.CLIEnterTime();
                newTime = appointment.DateTime.Date + newTime.TimeOfDay;
                if (newTime < DateTime.Now)
                {
                    CLI.CLIWriteLine("You can't enter that time, its in the past");
                }
                else
                {
                    if (DoctorService.CheckAppointment(newTime, appointment.Duration, ref doctor))
                    {
                        ChangeAppointment(appointment, newTime);
                    }
                    else
                    {
                        CLI.CLIWriteLine("You are not availible at that time.");
                        return;
                    }
                }
            } while (newTime < DateTime.Now);

        }

        public static void ChangeAppointment(Appointment appointment, DateTime newDate) {
            var last = AppointmentRepo.AllAppointments.Values.Last();
            Appointment editedAppointment;
            if (appointment.Type == 'o')
            {
                editedAppointment = new Operation(last.ID + 1, newDate, appointment.Doctor, appointment.Patient, appointment.Finished, 0, appointment.ID, appointment.Duration);
            }
            else
            {
                editedAppointment = new Examination(last.ID + 1, newDate, appointment.Doctor, appointment.Patient, appointment.Finished, 0, appointment.ID);
            }

            AppointmentRepo.Add(editedAppointment);
            AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
            appointment.Doctor.Appointments.Remove(appointment);
            appointment.Doctor.Appointments.Add(editedAppointment);
            appointment.Patient.Appointments.Remove(appointment);
            editedAppointment.Patient.Appointments.Add(editedAppointment);
        }

        //=======================================================================================================================================================================
        // DELETE
        public static void DeleteAppointment(ref Doctor doctor)
        {
            CLI.CLIWriteLine("Enter the ID of the appointment you wish to delete.");
            int id = CLI.CLIEnterNumber();
            Appointment appointment = null;
            foreach (Appointment tmp in doctor.Appointments)
            {
                if (tmp.ID == id)
                {
                    appointment = tmp;
                    CLI.CLIWriteLine("Are you sure? (y/n)");
                    string choice = CLI.CLIEnterString();
                    if (choice.ToUpper() == "Y")
                    {
                        doctor.Appointments.Remove(appointment);
                        appointment.Patient.Appointments.Remove(appointment);
                        var last = AppointmentRepo.AllAppointments.Values.Last();
                        Examination deletedExamination = new Examination(last.ID + 1, appointment.DateTime, doctor, appointment.Patient, appointment.Finished, appointment.ID, appointment.Edited);
                        AppointmentRepo.AllAppointments.Add(deletedExamination.ID, deletedExamination);
                        AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
                    }
                    break;
                }
            }

        }
    }
}
