using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class HealthRecord
    {
        public Patient Patient { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public List<string> MedicalHistory { get; set; }
        public List<Anamnesis> Anamneses { get; set; }
        public List<string> Alergies { get; set; }
        public void ShowHealthRecord() {
            Console.WriteLine($"Patient health record\n");
        }
    }
}
