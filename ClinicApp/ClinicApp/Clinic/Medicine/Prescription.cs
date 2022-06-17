﻿using ClinicApp.Users;
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
            Patient = UserRepository.Patients[data[0]];
            Doctor = UserRepository.Doctors[data[1]];
            Date = DateTime.Parse(data[2]);
            Medicine = MedicineRepo.Medicine[data[3]];
            Frequency = new int[3];
            Frequency[0] = Convert.ToInt32(data[4][0]);
            Frequency[1] = Convert.ToInt32(data[4][1]);
            Frequency[2] = Convert.ToInt32(data[4][2]);
            MedicineFoodIntake tmp;
            Enum.TryParse(data[5], out tmp);
            MedicineFoodIntake = tmp;
        }

        

        public string PresrciptionToMessage()
        {
            return "\nPrescription details:\nDate : "+Date.Date+"; Medicine: "+Medicine.Name+"\nNumber of pills to take:\nMorning:"+Frequency[0]+",Noon"+Frequency[1]+",Afternoon:"+Frequency[2]+"\nTake before/during/after food:"+MedicineFoodIntake;
        }


        public string Compress()
        {
            return Patient.UserName + "|" + Doctor.UserName + "|" + Date + "|" + Medicine.Name + "|" + Frequency[0] + Frequency[1] + Frequency[2] + "|" + MedicineFoodIntake;
        }


    }
}
