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

        public static void CreatePatient()
        {
            User patient = Registration.Register(Roles.Patient);
            UserRepository.Users.Add(patient.UserName, patient);
            UserRepository.Patients.Add(patient.UserName, (Patient)patient);
        }

        public static void DeletePatient(Patient patient)
        {
            UserRepository.Users.Remove(patient.UserName);
            UserRepository.Patients.Remove(patient.UserName);
        }

        public static void UpdatePatient(Patient patient)
        {
            int option = 1;

            while (option != 0)
            {
                patient.Print();
                CLI.CLIWriteLine("\nWhat would you like to change?");
                CLI.CLIWriteLine("1: Username");
                CLI.CLIWriteLine("2: Password");
                CLI.CLIWriteLine("3: Name");
                CLI.CLIWriteLine("4: Last name");
                CLI.CLIWriteLine("5: Gender");
                CLI.CLIWriteLine("6: Date of birth");
                CLI.CLIWriteLine("0: Back to menu");
                option = CLI.CLIEnterNumberWithLimit(0, 6);

                switch (option)
                {
                    //Username
                    case 1:
                        patient.UserName = Registration.GetUsername();
                        break;
                    //Password
                    case 2:
                        patient.Password = Registration.GetPassword();
                        break;
                    //Name
                    case 3:
                        patient.Name = Registration.GetName();
                        break;
                    //Last name
                    case 4:
                        patient.LastName = Registration.GetName();
                        break;
                    //Gender
                    case 5:
                        patient.Gender = Registration.GetGender()[0];
                        break;
                    //Date of birth
                    case 6:
                        DateTime date;
                        DateTime.TryParse(Registration.GetDateOfBirth(), out date);
                        patient.DateOfBirth = date.Date;
                        break;
                }
            }
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

        public static void ViewPatient(Patient patient)
        {
            Console.WriteLine($"Patient {patient.Name} {patient.LastName};\nDate of birth {patient.DateOfBirth.ToShortDateString()}; Gender:\n{patient.Gender}");
        }

        public static bool CheckAppointment(Patient patient, DateTime dateTime, int duration)
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

        public static void InsertAppointmentPatient(ref Patient patient, Appointment newExamination)
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
