using ClinicApp.Users;
using ClinicApp.Users.Doctor;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class Prescription
    {
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        public Medicine Medicine { get; set; }
        public int[] Frequency {get; set;}
        public MedicineFoodIntake MedicineFoodIntake { get; set; }
       

        public Prescription(Patient patient, Doctor doctor, DateTime date, Medicine medicine, int[] frequency, MedicineFoodIntake medicineFoodIntake)
        {
            Patient = patient;
            Doctor = doctor;
            Date = date;
            Medicine = medicine;
            Frequency = frequency;
            MedicineFoodIntake = medicineFoodIntake;
        }
        public Prescription(string text)
        {
            string[] data = text.Split('|');
            Patient = SystemFunctions.Patients[data[0]];
            Doctor = SystemFunctions.Doctors[data[1]];
            Date = DateTime.Parse(data[2]);
            Medicine = SystemFunctions.Medicine[data[3]];
            Frequency = new int[3];
            Frequency[0] = Convert.ToInt32(data[4][0]);
            Frequency[1] = Convert.ToInt32(data[4][1]);
            Frequency[2] = Convert.ToInt32(data[4][2]);
            MedicineFoodIntake tmp;
            Enum.TryParse(data[5], out tmp);
            MedicineFoodIntake = tmp;
        }

        public void ShowPrescription() {
            Console.WriteLine("\nPrescription details:");
            Console.WriteLine($"Date : {Date.Date}; Medicine: {Medicine.Name}");
            Console.WriteLine($"Patient full name: {Patient.Name} {Patient.LastName}");
            Console.WriteLine($"Doctor full name: {Doctor.Name}  {Doctor.LastName}");
            Console.WriteLine("Number of pills to take:");
            Console.WriteLine($"Morning: {Frequency[0]}, Noon: {Frequency[1]}, Afternoon: {Frequency[2]}");
            Console.WriteLine($"Take before/during/after food: {MedicineFoodIntake}");
        }


        public string Compress()
        {
            return Patient.UserName + "|" + Doctor.UserName + "|" + Date + "|" + Medicine.Name + "|" + Frequency[0] + Frequency[1] + Frequency[2] + "|" + MedicineFoodIntake;
        }


    }
}
