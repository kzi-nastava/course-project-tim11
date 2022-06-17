using ClinicApp.Users;
using System;

namespace ClinicApp.Clinic.Appointmens
{
    public class Anamnesis
    {
        public Doctor Doctor { get; set; }
        public string Report { get; set; }

        public DateTime Date { get; set; }

        public Anamnesis(string report, Doctor doctor){
            this.Report = report;
            this.Doctor = doctor;
            this.Date = DateTime.Now;
        }

        public Anamnesis(string text) {
            string[] data = text.Split(';');
            Report = data[1];
            Doctor = UserRepository.Doctors[data[0]];
            Date = DateTime.Parse(data[2]);
        }

        public string Compress() {
            return Doctor.UserName + ";" + Report + ";" + Date;
        }

        public void ShowAnamnesis() {
            Console.WriteLine($"\nDate: {Date}\nDoctor: {Doctor.Name} {Doctor.LastName}, username: {Doctor.UserName}\n\nReport: \n{Report}");
        }
    }

}
