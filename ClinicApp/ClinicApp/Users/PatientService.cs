using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Clinic;
using System.IO;

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

        public static void VievAllPatientsWithBlockedStatus()
        {
            CLI.CLIWriteLine(OtherFunctions.LineInTable() + "-----------------+");
            CLI.CLIWriteLine(OtherFunctions.TableHeader() + " Blocked by      |");
            CLI.CLIWriteLine(OtherFunctions.LineInTable() + "-----------------+");
            foreach (KeyValuePair<string, Patient> pair in UserRepository.Patients)
            {
                CLI.CLIWriteLine(pair.Value.TextInTable() + " " + pair.Value.Blocked.ToString() + OtherFunctions.Space(15, pair.Value.Blocked.ToString()) + " |");
                CLI.CLIWriteLine(OtherFunctions.LineInTable() + "-----------------+");
            }
            CLI.CLIWriteLine();
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

        //Manages blocked and unblocked patients.
        public static void ManageBlockedPatients()
        {
            int option = 1, numberOfOptions = 4;
            Patient patient;
            while (option != 0)
            {
                CLI.CLIWriteLine("\nWhat would you like to do?");
                CLI.CLIWriteLine("1: List patient accounts");
                CLI.CLIWriteLine("2: Block patient accounts");
                CLI.CLIWriteLine("3: Unblock patient accounts");
                CLI.CLIWriteLine("0: Back to menue");
                option = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
                CLI.CLIWriteLine();
                switch (option)
                {
                    //List all
                    case 1:
                        PatientService.VievAllPatientsWithBlockedStatus();
                        break;
                    //Block
                    case 2:
                        patient = (Patient)OtherFunctions.FindUser("\nEnter the username of the account you want to block:", Roles.Patient);
                        if (patient != null)
                            if (patient.Blocked == Blocked.Unblocked)
                                patient.Blocked = Blocked.Secretary;
                            else
                                CLI.CLIWriteLine("This patient's account is already blocked.");
                        break;
                    //Unblock
                    case 3:
                        patient = (Patient)OtherFunctions.FindUser("\nEnter the username of the account you want to unblock:", Roles.Patient);
                        if (patient != null)
                            if (patient.Blocked != Blocked.Unblocked)
                                patient.Blocked = Blocked.Unblocked;
                            else
                                CLI.CLIWriteLine("This patient's account is already unblocked.");
                        break;
                }
            }
        }

        //Manages patient requests.
        public static void ManageExaminationRequests()
        {
            int id, option;
            Clinic.Appointment appointment;

            CLI.CLIWriteLine();
            using (StreamReader reader = new StreamReader(SystemFunctions.PatientRequestsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    CLI.CLIWriteLine(line);
                    CLI.CLIWriteLine("1: Approve");
                    CLI.CLIWriteLine("2: Deny");
                    option = CLI.CLIEnterNumberWithLimit(1, 2);
                    if (option == 1)
                    {
                        if (!int.TryParse(line.Split("|")[0], out id))
                            id = -1;
                        if (line.Split("|")[1] == "DELETE")
                        {
                            appointment = AppointmentRepo.AllAppointments[id];
                            appointment.Doctor.Appointments.Remove(appointment);
                            appointment.Patient.Appointments.Remove(appointment);
                            var last = AppointmentRepo.AllAppointments.Values.Last();
                            Clinic.Examination deletedExamination = new Clinic.Examination(last.ID + 1, appointment.DateTime, appointment.Doctor, appointment.Patient, appointment.Finished, appointment.ID, appointment.Edited);
                            AppointmentRepo.AllAppointments.Add(deletedExamination.ID, deletedExamination);
                            AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
                        }
                        else if (line.Split("|")[1] == "UPDATE" && AppointmentRepo.AllAppointments.TryGetValue(id, out appointment))
                        {

                            appointment.DateTime = DateTime.Parse(line.Split("|")[2]);
                            Doctor doctor;
                            if (UserRepository.Doctors.TryGetValue(line.Split("|")[3], out doctor))
                            {
                                appointment.Doctor.Appointments.Remove(appointment);
                                appointment.Patient.Appointments.Remove(appointment);
                                var last = AppointmentRepo.AllAppointments.Values.Last();
                                Clinic.Examination editedExamination = new Clinic.Examination(last.ID + 1, appointment.DateTime, doctor, appointment.Patient, appointment.Finished, 0, appointment.ID);
                                AppointmentRepo.AllAppointments.Add(editedExamination.ID, editedExamination);
                                AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
                                AppointmentRepo.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                                Patient patient = appointment.Patient;
                                PatientService.InsertAppointmentPatient(ref patient, editedExamination);
                                DoctorService.InsertAppointment(editedExamination, ref doctor);
                            }
                        }
                    }
                }
            }
            CLI.CLIWriteLine();
        }
    }
}
