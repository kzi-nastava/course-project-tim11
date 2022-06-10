using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.Clinic
{
    public class AppointmentRepo
    {
        public static Dictionary<int, Appointment> AllAppointments { get; set; } = new Dictionary<int, Appointment>();
        public static Dictionary<int, Appointment> CurrentAppointments { get; set; } = new Dictionary<int, Appointment>();

        public static string AppointmentsFilePath = "../../../Data/appointments.txt";

        public static Dictionary<int, Appointment> GetAll() => AllAppointments;

        public static void Add(Appointment appointment)
        {
            if (OtherFunctions.ValidateDateTime(appointment.DateTime))
            {
                CurrentAppointments.Add(appointment.ID, appointment);
            }
            AllAppointments.Add(appointment.ID, appointment);
            PresistChanges();
        }
        public static void RemoveCurrent(Appointment appointment)
        {
            CurrentAppointments.Remove(appointment.ID);
            PresistChanges();
        }
        public static void LoadAppointments()
        {
            using (StreamReader reader = new StreamReader(AppointmentsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Appointment appointment = ParseAppointment(line);
                    if (!AllAppointments.TryAdd(appointment.ID, appointment))
                    {
                        AllAppointments[appointment.ID] = appointment;
                    }
                    if (appointment.DateTime.AddMinutes(15) > DateTime.Now)
                    {
                        if (appointment.Tombstone != 0)
                        {
                            CurrentAppointments.Remove(appointment.Tombstone);
                        }
                        else if (appointment.Edited != 0)
                        {
                            CurrentAppointments.Remove(appointment.Edited);
                            if (!appointment.Finished)
                            {
                                CurrentAppointments.Add(appointment.ID, appointment);
                            }

                        }
                        else if (!appointment.Finished)
                        {
                            CurrentAppointments.Add(appointment.ID, appointment);
                        }
                    }

                }
                foreach (int ID in CurrentAppointments.Keys)
                {
                    Appointment currentAppointment = CurrentAppointments[ID];
                    currentAppointment.Doctor.Appointments.Add(currentAppointment);
                    currentAppointment.Patient.Appointments.Add(currentAppointment);
                }
            }
        }
        private static Appointment ParseAppointment(string line)
        {
            string[] data = line.Split('|');
            if (data[8] == "o")
            {
                return new Operation(line);
            }
            else
            {
                return new Examination(line);
            }
        }
        public static void PresistChanges()
        {
            string newLine;
            File.Delete(AppointmentsFilePath);
            using (StreamWriter sw = File.CreateText(AppointmentsFilePath))
            {
                foreach (KeyValuePair<int, Appointment> pair in AllAppointments)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
