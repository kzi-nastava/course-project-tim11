using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
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

    }
}
