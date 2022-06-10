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

        public static void ViewAllDoctors()
        {
            int i = 1;
            foreach (KeyValuePair<string, Doctor> entry in UserRepository.Doctors)
            {
                Doctor doctor = entry.Value;
                Console.WriteLine($"{i}.User name: {doctor.UserName} ; Name: {doctor.Name}; Last Name: {doctor.LastName}");
                i++;
            }
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

        public static Appointment GetAppointmentByID(ref Doctor doctor)
        {
            bool quit = false;
            Appointment appointment = null;
            while (appointment == null)
            {
                CLI.CLIWriteLine("Enter the ID of the appointment you wish to edit?");
                int id = CLI.CLIEnterNumber();
                foreach (Appointment tmp in doctor.Appointments)
                {
                    if (tmp.ID == id){
                        appointment = tmp;
                        break;
                    }
                }
                if (appointment == null)
                {
                    CLI.CLIWriteLine($"No appointment matches ID: {id}");
                    quit = OtherFunctions.AskQuit();
                    if (quit) return null;
                }
            }
            return appointment;

        }

        //=======================================================================================================================================================================
        // VIEW SCHEDULE
        public static void ViewSchedule(ref Doctor doctor)
        {
            CLI.CLIWriteLine("Enter a date for which you wish to see your schedule (e.g. 22/10/1987): ");
            DateTime date = OtherFunctions.GetGoodDate();
            CLI.CLIWriteLine($"Appointments on date: {date.ToShortDateString()} and the next three days: \n");

            ShowAppointmentsByDate(date, ref doctor);

            string choice = "Y";
            while (choice.ToUpper() == "Y")
            {
                CLI.CLIWrite("Do you wish to view additional detail for any appointment?(y/n)\n>> ");
                choice = CLI.CLIEnterString();
                if (choice.ToUpper() == "Y")
                {
                    ViewAppointmentInfo();
                }
            }
            CLI.CLIWriteLine("Do you wish to perform an examination/operation(y/n)?");
            choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                AskPerform(ref doctor);
            }

        }
        private static void ShowAppointmentsByDate(DateTime date, ref Doctor doctor)
        {
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (date.Date <= appointment.DateTime.Date && appointment.DateTime.Date <= date.Date.AddDays(3))
                {
                    appointment.View();
                    CLI.CLIWriteLine();
                }
            }
        }
        private static void ViewAppointmentInfo()
        {
            
            CLI.CLIWrite("\n\nEnter the ID of the appointment you wish to view\n>> ");
            int id = CLI.CLIEnterNumber();
            Appointment chosenAppointment;
            if (!AppointmentRepo.CurrentAppointments.TryGetValue(id, out chosenAppointment))
            {
                CLI.CLIWriteLine("No appointment with that ID found");
                return;
            }
            CLI.CLIWriteLine("Searching for medical record");
            HealthRecord healthRecord = HealthRecordService.Search(chosenAppointment.Patient);
            CLI.CLIWriteLine("Information about patient:");
            PatientService.ViewPatient(healthRecord.Patient);
            HealthRecordService.ShowHealthRecord(healthRecord);

        }

        private static void AskPerform(ref Doctor doctor)
        {
            
            CLI.CLIWrite("\n\nEnter the ID of the appointment you wish to perform\n>> ");
            int id = CLI.CLIEnterNumber();

            Appointment chosenAppointment;
            if (!AppointmentRepo.CurrentAppointments.TryGetValue(id, out chosenAppointment))
            {
                Console.WriteLine("No appointment with that ID found");
                return;
            }
            Perform(ref chosenAppointment, ref doctor);

            
        }

        //=======================================================================================================================================================================
        // PERFORM EXAMINATION
        private static void Perform( ref Appointment appointment, ref Doctor doctor)
        {
            string type;
            if (appointment.Type == 'e') type = "Examination";
            else type = "Operation";

            CLI.CLIWriteLine($"{type} starting. Searching for medical record");
            HealthRecord healthRecord = HealthRecordService.Search(appointment.Patient);
            HealthRecordService.ShowHealthRecord(healthRecord);
            AnamnesisService.WriteAnamnesis(ref healthRecord, ref doctor);

            CLI.CLIWriteLine("\nDo you want to change medical record? (y/n)");
            string choice = CLI.CLIEnterString().ToUpper();
            if (choice == "Y")
            {
                HealthRecordService.ChangePatientRecord(ref healthRecord);
            }
            Patient patient = healthRecord.Patient;
            CLI.CLIWriteLine("Create referral for patient? (y/n)");
            choice = CLI.CLIEnterString().ToUpper();
            if (choice == "Y")
            { 
                ReferralService.CreateReferral(ref patient, ref doctor);
            }
            do
            {
                CLI.CLIWriteLine("Write prescription for patient? (y/n)");
                choice = CLI.CLIEnterString().ToUpper();
                if (choice.ToUpper() == "Y")
                {
                    PrescriptionService.WritePrescription(ref patient, doctor);

                }
            } while (choice.ToUpper() == "Y");

            EquipmentService.UpdateRoomEquipment(doctor);

            appointment.Finished = true;
            AppointmentRepo.CurrentAppointments.Remove(appointment.ID);
            doctor.Appointments.Remove(appointment);
            appointment.Patient.Appointments.Remove(appointment);
            CLI.CLIWriteLine($"{type} ended.");
        }


    }
}
