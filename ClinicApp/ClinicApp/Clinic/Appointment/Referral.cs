﻿using ClinicApp.Users;
using System;

namespace ClinicApp.Clinic
{
    public class Referral
    {
        public Doctor DoctorExaminated { get; set; }
        public Patient Patient { get; set; }
        public Doctor DoctorSpecialist { get; set; }
        public Fields Field { get; set; }


        public Referral(Doctor doctorExaminated, Patient patient, Doctor doctorSpecialist, Fields field)
        {
            Patient = patient;
            DoctorExaminated = doctorExaminated;
            DoctorSpecialist = doctorSpecialist;
            Field = field;
        }
        public Referral(string text)
        {
            string[] data = text.Split('|');
            DoctorExaminated = UserRepository.Doctors[data[0]];
            this.Patient = UserRepository.Patients[data[1]];
            if (data[2] == "null") DoctorSpecialist = null;
            else DoctorSpecialist = UserRepository.Doctors[data[2]];
            Fields field;
            Enum.TryParse(data[3], out field);
            Field = field;
            
        }

        public string Compress()
        {
            string drSpecialist;
            if (DoctorSpecialist == null) drSpecialist = "null";
            else drSpecialist = DoctorSpecialist.UserName;
            return DoctorExaminated.UserName + "|" + this.Patient.UserName + "|" + drSpecialist + "|" + Field.ToString();
        }


    }


}

