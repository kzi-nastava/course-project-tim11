using ClinicApp.Dialogs;
using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicApp.Clinic
{
    public class AppointmentService
    {
        public static void CreateAppointment(ref Doctor doctor, int id, DateTime dateTime,ref Patient patient,int duration, int type)
        {
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
            AppointmentRepo.Add(appointment);

        }

        public static void ChangeAppointment(Appointment appointment, DateTime newDate)
        {
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
        public static void DeleteAppointment(ref Doctor doctor, ref Appointment appointment)
        {
            doctor.Appointments.Remove(appointment);
            appointment.Patient.Appointments.Remove(appointment);
            var last = AppointmentRepo.AllAppointments.Values.Last();
            Appointment deletedAppointment;
            if (appointment.Type == 'o')
            {
                deletedAppointment = new Operation(last.ID + 1, appointment.DateTime, doctor, appointment.Patient, appointment.Finished, appointment.ID, appointment.Edited, appointment.Duration);
            }
            else
            {
                deletedAppointment = new Examination(last.ID + 1, appointment.DateTime, doctor, appointment.Patient, appointment.Finished, appointment.ID, appointment.Edited);
            }
            AppointmentRepo.AllAppointments.Add(deletedAppointment.ID, deletedAppointment);
            AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
            AppointmentRepo.PresistChanges();
        }
        
    }
}
