using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class PrescriptionService
    {
        //=======================================================================================================================================================================
        // WRITE UP A PRESCRIPTION FOR A PATIENT
        public static void WritePrescription(ref Patient patient, Doctor doctor)
        {
            Medicine medicine = MedicineService.GetMedicine();
            bool alergic = MedicineService.CheckAlergy(medicine, patient.UserName);
            if (alergic) return;
            int[] frequency = { 0, 0, 0 };
            Console.WriteLine("Enter the number of pills to take in 1) the morning 2) at noon 3) the afternoon:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write((i + 1) + ") >> ");
                frequency[i] = CLI.CLIEnterNumber();
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

    }
}
