using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        

        
        public HealthRecord(Patient patient) {
            Patient = patient;
            Weight = 0;
            Height = 0;
            MedicalHistory = new List<string>();
            Alergies = new List<string>();
            Anamneses = new List<Anamnesis>();
        }
        public HealthRecord(string text) {
            string[] data = text.Split('|');

            Patient = UserRepository.Patients[data[0]];
            Weight = Convert.ToDouble(data[1]);
            Height = Convert.ToDouble(data[2]);
            MedicalHistory = new List<string>();
            Alergies = new List<string>();
            Anamneses = new List<Anamnesis>();
            if (data[3].Length > 0) {
                string[] medicalHistory = data[3].Split('/');
                foreach (string illness in medicalHistory)
                {
                    MedicalHistory.Add(illness);
                }
            }
            if (data[4].Length > 0)
            {
                string[] anamneses = data[4].Split('/');
                foreach (string info in anamneses)
                {
                    Anamnesis anamnesis = new Anamnesis(info);
                    Anamneses.Add(anamnesis);
                }
            }
            if (data[5].Length > 0)
            {
                string[] alergies = data[5].Split('/');
                foreach (string alergy in alergies)
                {
                    Alergies.Add(alergy);
                }
            }
        }

        public string Compress() {
            string allIlnesses = "";
            foreach (string illness in MedicalHistory) {
                allIlnesses += illness + "/";
            }
            allIlnesses = allIlnesses.Remove(allIlnesses.Length - 1);
            string anamneses = "";
            foreach (Anamnesis anamnesis in Anamneses)
            {
                anamneses += anamnesis.Compress() + "/";
            }
            anamneses = anamneses.Remove(anamneses.Length - 1);
            string allAlergies = ""; 
            foreach (string alergy in Alergies)
            {
                allAlergies += alergy + "/";
            }
            allAlergies = allAlergies.Remove(allAlergies.Length - 1);
            return Patient.UserName + "|" + Weight + "|" + Height + "|" + allIlnesses + "|" + anamneses + "|" + allAlergies;
        }

        
    }
}
