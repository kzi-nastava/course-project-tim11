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
        public void ShowHealthRecord() {
            Console.WriteLine($"Patient {Patient.Name} {Patient.LastName}'s health record\n");
            Console.WriteLine($"Weight: {Weight}\nHeight: {Height}\n");
            Console.Write("Medical history: ");
            foreach (string illness in MedicalHistory)
            {
                Console.Write(illness + ", ");
            }
            Console.Write("\nKnown alergies: ");
            foreach (string alergy in Alergies)
            {
                Console.Write(alergy + ", ");
            }
            int i = 1;
            Console.WriteLine("\nPrevious anamnesis: ");
            foreach (Anamnesis anamnesis in Anamneses)
            {
                Console.WriteLine("\n\n" + i + ". Anamnesis");
                anamnesis.ShowAnamnesis();
                i++;
            }
        }

        public void AnamnesisSortedDoctor()
        {
            Anamneses.Sort(delegate (Anamnesis a1, Anamnesis a2)
            {
                return a1.Doctor.UserName.CompareTo(a2.Doctor.UserName);
            });
            Console.WriteLine("Sorted anamnesis list by doctors username.");
            foreach(Anamnesis temp in Anamneses)
            {
                temp.ShowAnamnesis();
            }
        }

        public void AnamnesisSortedDate()
        {
            Anamneses.Sort(delegate (Anamnesis a1, Anamnesis a2)
            {
                return a1.Date.CompareTo(a2.Date);
            });
            Console.WriteLine("Sorted anamnesis list by date.");
            foreach(Anamnesis temp in Anamneses)
            {
                temp.ShowAnamnesis();
            }
        }

        public void SearchAnamnesis(string keyword)
        {
            bool found = false;
            foreach(Anamnesis temp in Anamneses)
            {
                if (temp.Report.Contains(keyword) == true)
                {
                    temp.ShowAnamnesis();
                    found = true;
                }
            }
            if(found == false)
            {
                Console.WriteLine("There is no amnesis with specified keyword.");
            }
        }

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

            Patient = SystemFunctions.Patients[data[0]];
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
