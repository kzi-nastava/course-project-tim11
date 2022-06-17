using ClinicApp.AdminFunctions;
using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.Users
{
    public class DoctorService
    {
        public DoctorService()
        {
        }
        public static List<Doctor> SortDoctorsByFirstName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.Name.CompareTo(d2.Name);
            });
            return doctorsUnsorted;
        }

        public static List<Doctor> SortDoctorsByLastName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

        public static List<Doctor> SortDoctorsByField(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

        public static bool CheckAppointment(DateTime dateTime, int duration, ref Doctor doctor)
        {
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (appointment.DateTime.Date == dateTime.Date)
                {
                    if ((appointment.DateTime <= dateTime && appointment.DateTime.AddMinutes(duration) > dateTime) || (dateTime <= appointment.DateTime && dateTime.AddMinutes(duration) > appointment.DateTime))
                    {
                        return false;
                    }
                }
                if (appointment.DateTime.Date > dateTime.Date)
                {
                    break;
                }

            }
            return true;
        }

        public static void InsertAppointment(Appointment newAppointment, ref Doctor doctor)
        {
            if (doctor.Appointments.Count() == 0)
            {
                doctor.Appointments.Add(newAppointment);
                return;
            }
            for (int i = 0; i < doctor.Appointments.Count(); i++)
            {
                if (doctor.Appointments[i].DateTime < newAppointment.DateTime)
                {
                    doctor.Appointments.Insert(i, newAppointment);
                    return;
                }
            }
            doctor.Appointments.Add(newAppointment);

        }

        public static Appointment FindAppointment(Doctor doctor, int id)
        {
            Appointment appointment = null;
            while (appointment == null)
            {
                foreach (Appointment tmp in doctor.Appointments)
                {
                    if (tmp.ID == id)
                    {
                        appointment = tmp;
                        break;
                    }
                }
            }
            return appointment;
        }

        public static void ShowAppointmentsByDate(DateTime date, Doctor doctor)
        {
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (date.Date <= appointment.DateTime.Date && appointment.DateTime.Date <= date.Date.AddDays(3))
                {
                    appointment.View();
                }
            }
        }
        public static void UpdateAfterPerforming( ref Appointment appointment, ref Doctor doctor)
        {
            EquipmentService.UpdateRoomEquipment(doctor);
            appointment.Finished = true;
            AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
            doctor.Appointments.Remove(appointment);
            appointment.Patient.Appointments.Remove(appointment);
        }

        public static bool IsDoctorFree(DateTime startDate, DateTime endDate, Doctor doctor) {
            foreach(Appointment appointment in doctor.Appointments)
            {
                if(appointment.DateTime <= endDate || appointment.DateTime >= startDate)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
