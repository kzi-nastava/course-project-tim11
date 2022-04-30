using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.Clinic
{
    public class Examination
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }

        public bool Tombstone { get; set; }
        public Examination(int id, DateTime dateTime, Doctor doctor, Patient patient, bool tombstone)
        {
            this.ID = id;
            this.DateTime = dateTime;
            this.Doctor = doctor;
            this.Patient = patient;
            this.Tombstone = tombstone;
        }

        public Examination(string text)
        {
            string[] data = text.Split("|");

            ID = Convert.ToInt32(data[0]);
            DateTime = DateTime.Parse(data[1]);
            Doctor = SystemFunctions.Doctors[data[2]];
            Patient = SystemFunctions.Patients[data[3]];
            Tombstone = Convert.ToBoolean(data[4]);
        }

        public string Compress()
        {
            return ID + "|" + DateTime + "|" + Doctor.UserName + "|" + Patient.UserName + "|" + Tombstone;
        }

        public void ToFile() {
            string line = this.Compress();
            using (StreamWriter sw = File.AppendText(SystemFunctions.ExaminationsFilePath))
            {
                sw.WriteLine(line);
            };
        }
    }
}
