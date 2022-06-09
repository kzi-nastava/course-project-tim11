using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Clinic;

namespace ClinicApp.Users
{
    public class PatientService
    {
        public PatientService()
        {
        }

        public static void ViewAllPatients()
        {
            int i = 1;
            foreach (KeyValuePair<string, Patient> entry in UserRepository.Patients)
            {
                Patient patient = entry.Value;
                Console.WriteLine($"{i}. User name: {patient.UserName}; Name: {patient.Name}; Last name: {patient.LastName}; Date of birth: {patient.DateOfBirth}");
                i++;
            }
        }

        public void ViewPatient(Patient patient)
        {
            Console.WriteLine($"Patient {patient.Name} {patient.LastName};\nDate of birth {patient.DateOfBirth.ToShortDateString()}; Gender:\n{patient.Gender}");
        }

        public bool CheckAppointment(Patient patient,DateTime dateTime, int duration)
        {
            foreach (Examination examination in patient.Appointments)
            {
                if (examination.DateTime.Date == dateTime.Date)
                {
                    if ((examination.DateTime <= dateTime && examination.DateTime.AddMinutes(duration) > dateTime) || (dateTime <= examination.DateTime && dateTime.AddMinutes(duration) > examination.DateTime))
                    {
                        return false;
                    }
                }
                if (examination.DateTime.Date > dateTime.Date)
                {
                    break;
                }

            }
            return true;
        }

        public void InsertAppointmentPatient(Patient patient,Appointment newExamination)
        {
            if (patient.Appointments.Count() == 0)
            {
                patient.Appointments.Add(newExamination);
                return;
            }
            for (int i = 0; i < patient.Appointments.Count(); i++)
            {
                if (patient.Appointments[i].DateTime < newExamination.DateTime)
                {
                    patient.Appointments.Insert(i, newExamination);
                    return;
                }
            }
            patient.Appointments.Add(newExamination);

        }
    }
}
