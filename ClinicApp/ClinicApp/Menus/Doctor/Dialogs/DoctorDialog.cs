using ClinicApp.Clinic;
using ClinicApp.Users;
using System;
using System.Collections.Generic;

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
        public static Appointment GetAppointmentByID(Doctor doctor)
        {
            Console.WriteLine("Enter the ID of the appointment you wish to edit?");
            int id = OtherFunctions.EnterNumber();
            Appointment appointment = AppointmentService.FindAppointment(doctor, id);

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

            ShowAppointmentsByDate(date, doctor);

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
            HealthRecordDialog.ShowHealthRecord(healthRecord);

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
            HealthRecordDialog.ShowHealthRecord(healthRecord);
            AnamnesisService.WriteAnamnesis(ref healthRecord, ref doctor);

            Console.WriteLine("\nDo you want to change medical record? (y/n)");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                HealthRecordDialog.ChangePatientRecord(ref healthRecord);
            }
            Patient patient = healthRecord.Patient;
            Console.WriteLine("Create referral for patient? (y/n)");
            choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                CreateReferral(ref patient, ref doctor);
            }
            do
            {
                Console.WriteLine("Write prescription for patient? (y/n)");
                choice = Console.ReadLine().ToUpper();
                if (choice.ToUpper() == "Y")
                {
                    WritePrescription(ref patient, doctor);

                }
            } while (choice.ToUpper() == "Y");

            AppointmentService.UpdateAfterPerforming(ref appointment, ref doctor);
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

        

        public static void WritePrescription(ref Patient patient, Doctor doctor)
        {
            Medicine medicine = MedicineDialog.GetMedicine();
            bool alergic = MedicineDialog.CheckAlergy(medicine, patient.UserName);
            if (alergic) return;
            int[] frequency = { 0, 0, 0 };
            Console.WriteLine("Enter the number of pills to take in 1) the morning 2) at noon 3) the afternoon:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write((i + 1) + ") >> ");
                frequency[i] = OtherFunctions.EnterNumber();
                Console.WriteLine();
            }
            Console.WriteLine("Should the patient take the medicine: \n(1) Before a meal\n(2) After a meal\n(3) During a meal\n(4) Doesn't matter\n\nChose by number");
            int medicineMealInfo = CLI.CLIEnterNumberWithLimit(0, 5);
            MedicineFoodIntake medicineFoodIntake = (MedicineFoodIntake)(medicineMealInfo - 1);
            Prescription prescription = new Prescription(patient, doctor, DateTime.Now, medicine, frequency, medicineFoodIntake);
            ShowPrescription(prescription);
            Console.WriteLine("Prescription created\n");
            PrescriptionRepo.Add(prescription, patient);
            patient.Prescriptions.Add(prescription);

        }

        public static void ShowPrescription(Prescription prescription)
        {
            Console.WriteLine("\nPrescription details:");
            Console.WriteLine($"Date : {prescription.Date.Date}; Medicine: {prescription.Medicine.Name}");
            Console.WriteLine($"Patient full name: {prescription.Patient.Name} {prescription.Patient.LastName}");
            Console.WriteLine($"Doctor full name: {prescription.Doctor.Name}  {prescription.Doctor.LastName}");
            Console.WriteLine("Number of pills to take:");
            Console.WriteLine($"Morning: {prescription.Frequency[0]}, Noon: {prescription.Frequency[1]}, Afternoon: {prescription.Frequency[2]}");
            Console.WriteLine($"Take before/during/after food: {prescription.MedicineFoodIntake}");
        }

        public static void CreateReferral(ref Patient patient, ref Doctor doctorExamined)
        {
            Console.WriteLine("Create referral for (1) specific doctor or (2) specific field");
            int i = CLI.CLIEnterNumberWithLimit(0, 3);
            if (i == 1)
            {
                Doctor doctor = OtherFunctions.AskUsernameDoctor();
                if (doctor == null) return;
                Referral referral = new Referral(doctorExamined, patient, doctor, doctor.Field);
                patient.Referrals.Add(referral);
                ReferralRepo.Referrals.Add(referral);
            }
            else
            {
                Fields field = OtherFunctions.AskField();
                Referral referral = new Referral(doctorExamined, patient, null, field);
                patient.Referrals.Add(referral);
                ReferralRepo.Referrals.Add(referral);

            }
            Console.WriteLine("Referral created successfully!");

        }

    }
}
