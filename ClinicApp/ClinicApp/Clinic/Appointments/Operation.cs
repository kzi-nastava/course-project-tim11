using ClinicApp.Users;
using System;
using ClinicApp.HelperClasses;

namespace ClinicApp.Clinic.Appointmens
{
    public class Operation : Appointment
    {

        public Operation(int id, DateTime dateTime, Doctor doctor, Patient patient, bool finished, int tombstone, int edited, int duration)
        {
            this.ID = id;
            this.DateTime = dateTime;
            this.Doctor = doctor;
            this.Patient = patient;
            this.Tombstone = tombstone;
            this.Finished = finished;
            this.Edited = edited;
            this.Type = 'o';
            this.Duration = duration;
        }

        public Operation(string text)
        {
            string[] data = text.Split("|");

            ID = Convert.ToInt32(data[0]);
            DateTime = DateTime.Parse(data[1]);
            Doctor = UserRepository.Doctors[data[2]];
            Patient = UserRepository.Patients[data[3]];
            Finished = Convert.ToBoolean(data[4]);
            Tombstone = Convert.ToInt32(data[5]);
            Edited = Convert.ToInt32(data[6]);
            this.Type = 'o';
            this.Duration = Convert.ToInt32(data[8]);
        }

        public override string Compress()
        {
            return ID + "|" + DateTime + "|" + Doctor.UserName + "|" + Patient.UserName + "|" + Finished + "|" + Tombstone + "|" + Edited + "|o|" + Duration;
        }


        public override void View()
        {
            OperationService.View(this);
        }

        public override DateTime NextAvailable()
        {
            DateTime nextAvailable = DateTime;

            bool hasFoundTime = false;
            while (hasFoundTime == false)
            {
                Doctor doctor = this.Doctor;
                nextAvailable = nextAvailable.AddMinutes(1);
                DateRange dateRange = new DateRange(nextAvailable, nextAvailable.AddMinutes(Duration));
                if (PatientService.CheckAppointment(Patient, nextAvailable, Duration) &&
                    AppointmentService.CheckAppointment(nextAvailable, Duration, ref doctor) &&
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