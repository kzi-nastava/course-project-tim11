using ClinicApp.AdminFunctions;
using ClinicApp.Clinic;
using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Dialogs
{
    class DoctorDialog
    {
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

        public static Appointment GetAppointmentByID(Doctor doctor)
        {
            Console.WriteLine("Enter the ID of the appointment you wish to edit?");
            int id = OtherFunctions.EnterNumber();
            Appointment appointment = DoctorService.FindAppointment(doctor, id);

            if (appointment == null)
            {
                Console.WriteLine($"No appointment matches ID: {id}");
            }
            return appointment;

        }

        public static void ViewSchedule(ref Doctor doctor)
        {
            Console.WriteLine("Enter a date for which you wish to see your schedule (e.g. 22/10/1987): ");
            DateTime date = OtherFunctions.GetGoodDate();
            Console.WriteLine($"Appointments on date: {date.ToShortDateString()} and the next three days: \n");

            DoctorService.ShowAppointmentsByDate(date, doctor);

            string choice = "Y";
            while (choice.ToUpper() == "Y")
            {
                Console.Write("Do you wish to view additional detail for any appointment?(y/n)\n>> ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    ViewAppointmentInfo();
                }
            }
            Console.WriteLine("Do you wish to perform an examination/operation(y/n)?");
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                AskPerform(ref doctor);
            }

        }
        private static void ViewAppointmentInfo()
        {

            Console.Write("\n\nEnter the ID of the appointment you wish to view\n>> ");
            int id = CLI.CLIEnterNumber();
            Appointment chosenAppointment;
            if (!AppointmentRepo.CurrentAppointments.TryGetValue(id, out chosenAppointment))
            {
                Console.WriteLine("No appointment with that ID found");
                return;
            }
            Console.WriteLine("Searching for medical record");
            HealthRecord healthRecord = HealthRecordService.Search(chosenAppointment.Patient);
            Console.WriteLine("Information about patient:");
            PatientService.ViewPatient(healthRecord.Patient);
            HealthRecordService.ShowHealthRecord(healthRecord);

        }

        private static void AskPerform(ref Doctor doctor)
        {

            Console.Write("\n\nEnter the ID of the appointment you wish to perform\n>> ");
            int id = OtherFunctions.EnterNumber();

            Appointment chosenAppointment;
            if (!AppointmentRepo.CurrentAppointments.TryGetValue(id, out chosenAppointment))
            {
                Console.WriteLine("No appointment with that ID found");
                return;
            }
            Perform(ref chosenAppointment, ref doctor);
        }

        private static void Perform(ref Appointment appointment, ref Doctor doctor)
        {
            string type;
            if (appointment.Type == 'e') type = "Examination";
            else type = "Operation";

            Console.WriteLine($"{type} starting. Searching for medical record");
            HealthRecord healthRecord = HealthRecordService.Search(appointment.Patient);
            HealthRecordService.ShowHealthRecord(healthRecord);
            AnamnesisService.WriteAnamnesis(ref healthRecord, ref doctor);

            Console.WriteLine("\nDo you want to change medical record? (y/n)");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                HealthRecordService.ChangePatientRecord(ref healthRecord);
            }
            Patient patient = healthRecord.Patient;
            Console.WriteLine("Create referral for patient? (y/n)");
            choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                ReferralService.CreateReferral(ref patient, ref doctor);
            }
            do
            {
                Console.WriteLine("Write prescription for patient? (y/n)");
                choice = Console.ReadLine().ToUpper();
                if (choice.ToUpper() == "Y")
                {
                    PrescriptionService.WritePrescription(ref patient, doctor);

                }
            } while (choice.ToUpper() == "Y");

            DoctorService.UpdateAfterPerforming(ref appointment, ref doctor);
            Console.WriteLine($"{type} ended.");
        }

        public static void ReviewMedicineRequests()
        {
            string choice;
            Console.WriteLine("Medicine requests: ");
            List<MedicineRequest> allRequests = MedicineRequestRepository.LoadMedicineRequests();
            foreach (var request in allRequests)
            {

                if (request.Comment == "")
                {
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("Request ID: " + request.Id +
                        "\nMedicine name: " + request.Medicine.Name +
                        "\nMedicine ingrediants: " + MedicineRequestService.WriteMedicineIngrediants(request.Medicine.Ingredients) + "\n");
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("Do you want to approve this medicine(y/n)");
                    choice = Console.ReadLine();
                    if (choice.ToUpper() == "Y")
                    {
                        MedicineRequestService.Approve(request.Id);
                    }
                    else
                    {
                        Console.WriteLine("Do you want to reject this medicine(y/n)");
                        choice = Console.ReadLine();
                        if (choice.ToUpper() == "Y")
                        {
                            Console.WriteLine("Why do you want to reject this medicine? Write a short comment.");
                            string comment = Console.ReadLine();
                            MedicineRequestService.Reject(request.Id, comment);
                        }

                    }
                }
            }
        }

        public static void GatherInfoFreeDayRequest(ref Doctor doctor)
        {
            Console.WriteLine("Is this request urgent(y/n)?");
            bool urgent = OtherFunctions.EnterBool();
            Console.WriteLine("Enter the start date for your free days (e.g. 22/10/1987): ");
            DateTime dateFrom = CLI.CLIEnterNonPastDate();
            if (DateTime.Today.AddDays(2) <= dateFrom && !urgent)
            {
                Console.WriteLine("You have to request free days at least two days before your leave!");
                return;
            }
            Console.WriteLine("Enter the end date for your free days (e.g. 22/10/1987): ");
            DateTime dateTo = CLI.CLIEnterNonPastDate();
            if (dateTo < dateFrom)
            {
                Console.WriteLine("Invalid end date.");
                return;
            }
            if (!DoctorService.IsDoctorFree(dateFrom, dateTo, doctor))
            {
                Console.WriteLine("You are not free, you have scheduled appointments in that time period!");
                return;
            }
            if ((dateTo - dateFrom).TotalDays > 5 && urgent)
            {
                Console.WriteLine("An urgent request can't be longer than 5 days.");
                return;
            }
            Console.WriteLine("Enter the reason behind your request?");
            string reason = Console.ReadLine();
            FreeDaysRequestService.RequestFreeDay(ref doctor, dateFrom, dateTo, urgent, reason);
            Console.WriteLine("Successfully created your request.");
        }

    }
}
