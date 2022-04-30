using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicApp.Users
{
    public enum Blocked
    {
        Unblocked, Secretary, System
    };
    public class Patient : User
    {
        public Blocked Blocked { get; set; }
        public List<Examination> Examinations { get; set; }

        public Patient(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender, Blocked blocked)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Patient;
            Blocked = blocked;
            Examinations = new List<Examination>();
        }

        public Patient(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Patient;
            Blocked temp;
            if (Blocked.TryParse(data[7], out temp))
                Blocked = temp;
            else
                Blocked = Blocked.Unblocked;
            Examinations = new List<Examination>();
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role + "|" + Blocked.ToString();
        }

        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("0: Exit");

            return 1;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    //TODO
                    break;
            }
        }

        public void ViewPatient() {
           
            Console.WriteLine($"User name: {UserName}; \nName: {Name}; \nLast name: {LastName}; \nDate of birth: {DateOfBirth};\nGender: {Gender}\n\n");
        }
        public static void ViewAllPatients()
        {
            int i = 1;
            foreach (KeyValuePair<string, Patient> entry in SystemFunctions.Patients)
            {
                Patient patient = entry.Value;
                Console.WriteLine(i + ". Patient");
                patient.ViewPatient();
                i++;
            }
        }

        public void InsertExamination(Examination newExamination)
        {
            if (this.Examinations.Count() == 0) {
                this.Examinations.Add(newExamination);
                return;
            }
            for (int i = 0; i < this.Examinations.Count(); i++)
            {
                if (this.Examinations[i].DateTime < newExamination.DateTime)
                {
                    Examinations.Insert(i, newExamination);
                    return;
                }
            }
            this.Examinations.Add(newExamination);

        }
    }
}
