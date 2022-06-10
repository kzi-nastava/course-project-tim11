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
            CLI.CLIWriteLine("Enter the number of pills to take in 1) the morning 2) at noon 3) the afternoon:");
            for (int i = 0; i < 3; i++)
            {
                CLI.CLIWrite((i + 1) + ") >> ");
                frequency[i] = CLI.CLIEnterNumber();
                CLI.CLIWriteLine();
            }
            CLI.CLIWriteLine("Should the patient take the medicine: \n(1) Before a meal\n(2) After a meal\n(3) During a meal\n(4) Doesn't matter\n\nChose by number");
            int medicineMealInfo = CLI.CLIEnterNumberWithLimit(0, 5);
            MedicineFoodIntake medicineFoodIntake = (MedicineFoodIntake)(medicineMealInfo - 1);
            Prescription prescription = new Prescription(patient, doctor, DateTime.Now, medicine, frequency, medicineFoodIntake);
            ShowPrescription(prescription);
            CLI.CLIWriteLine("Prescription created\n");
            PrescriptionRepo.Add(prescription, patient);
            patient.Prescriptions.Add(prescription);

        }

        public static void ShowPrescription(Prescription prescription)
        {
            CLI.CLIWriteLine("\nPrescription details:");
            CLI.CLIWriteLine($"Date : {prescription.Date.Date}; Medicine: {prescription.Medicine.Name}");
            CLI.CLIWriteLine($"Patient full name: {prescription.Patient.Name} {prescription.Patient.LastName}");
            CLI.CLIWriteLine($"Doctor full name: {prescription.Doctor.Name}  {prescription.Doctor.LastName}");
            CLI.CLIWriteLine("Number of pills to take:");
            CLI.CLIWriteLine($"Morning: {prescription.Frequency[0]}, Noon: {prescription.Frequency[1]}, Afternoon: {prescription.Frequency[2]}");
            CLI.CLIWriteLine($"Take before/during/after food: {prescription.MedicineFoodIntake}");
        }

    }
}
