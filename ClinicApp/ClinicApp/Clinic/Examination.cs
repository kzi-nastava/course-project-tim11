using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ClinicApp.HelperClasses;

namespace ClinicApp.Clinic
{
    public class Examination : Appointment
    {

        public Examination(int id, DateTime dateTime, Doctor doctor, Patient patient, bool finished, int tombstone, int edited)
        {
            this.ID = id;
            this.DateTime = dateTime;
            this.Doctor = doctor;
            this.Patient = patient;
            this.Tombstone = tombstone;
            this.Finished = finished;
            this.Edited = edited;
            this.Type = 'e';
            this.Duration = 15;
        }

        public Examination(string text)
        {
            string[] data = text.Split("|");

            ID = Convert.ToInt32(data[0]);
            DateTime = DateTime.Parse(data[1]);
            Doctor = UserRepository.Doctors[data[2]];
            Patient = UserRepository.Patients[data[3]];
            Finished = Convert.ToBoolean(data[4]);
            Tombstone = Convert.ToInt32(data[5]);
            Edited = Convert.ToInt32(data[6]);
            Type = 'e';
            this.Duration = 15;

        }

        public override string Compress()
        {
            return ID + "|" + DateTime + "|" + Doctor.UserName + "|" + Patient.UserName + "|" + Finished + "|" + Tombstone + "|" + Edited + "|e|" + Duration;
        }


        public override void View()
        {
            Console.WriteLine($"EXAMINATION ID: {ID}\nDate and time:{DateTime}\nDuration: 15min\nPatient name: {Patient.Name}; ");
            Console.WriteLine($"Patient last name: {Patient.LastName};");
            Console.WriteLine($"Date of birth {Patient.DateOfBirth.ToShortDateString()}");
        }

        public override DateTime NextAvailable()
        {
            DateTime nextAvailable = DateTime;

            bool hasFoundTime = false;
            while (hasFoundTime == false)
            {
                Doctor doctor = this.Doctor;
                nextAvailable = nextAvailable.AddMinutes(1);
                DateRange dateRange = new DateRange(nextAvailable, nextAvailable.AddMinutes(15));
                if (PatientService.CheckAppointment(Patient, nextAvailable, 15) &&
                    DoctorService.CheckAppointment(nextAvailable, 15, ref doctor) &&
                    !OtherFunctions.CheckForRenovations(dateRange, Doctor.RoomId) &&
                    !OtherFunctions.CheckForExaminations(dateRange, Doctor.RoomId))
                {
                    hasFoundTime = true;
                }
            }

            return nextAvailable;
        }
    }
}