using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        //public Dictionary<>
        //public Dictionary<DateTime, string>;

        public Patient(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Patient;
            Blocked = Blocked.Unblocked;
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
            Blocked = Blocked.Unblocked;
            Examinations = new List<Examination>();
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
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

        public static void ViewAllPatients()
        {
            int i = 1;
            foreach (KeyValuePair<string, Patient> entry in SystemFunctions.Patients)
            {
                Patient patient = entry.Value;
                Console.WriteLine($"{i}. User name: {patient.UserName}; Name: {patient.Name}; Last name: {patient.LastName}; Date of birth: {patient.DateOfBirth}");
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

        private void DeleteExamination()
        {
            Console.WriteLine("Enter the ID of the examination you want to delete?");
            string id = Console.ReadLine();
            Examination examination = null;
            foreach (Examination tmp in this.Examinations)
            {
                if(tmp.ID == Convert.ToInt32(id))
                {
                    examination = tmp;
                    DateTime now = DateTime.Now;
                    Console.WriteLine("Are you sure? (y/n)");
                    string choice = Console.ReadLine();
                    int dateValidation = DateTime.Compare(now, tmp.DateTime);
                    DateTime beforeExamination = tmp.DateTime - TimeSpan.FromDays(2);
                    int dateValidationSecretary = DateTime.Compare(now, beforeExamination);
                    if (choice.ToUpper() == "Y")
                    {
                        if (!(dateValidation<0))
                        {
                            Console.WriteLine("You can not perform this action.");
                            return;
                        }       
                        else if (!(dateValidationSecretary<0))
                        {
                            Console.WriteLine("You can not perform this activity. Your request will be sent to secretary.");
                            string line = examination.ID.ToString() + "|" + "DELETE";
                            using (StreamWriter sw = File.AppendText(SystemFunctions.PatientRequestsFilePath))
                            {
                                sw.WriteLine(line);
                            }
                            //todo activity history write
                            return;
                        }
                        this.Examinations.Remove(examination);
                        Examination deletedExamination = new Examination(examination.ID, examination.DateTime,examination.Doctor,this,true);
                        SystemFunctions.AllExamtinations.Add(deletedExamination.ID, deletedExamination);
                        SystemFunctions.CurrentExamtinations.Remove(examination.ID);
                        //todo remove from doctors schedule
                        //todo acitvity history write
                    }
                }
            }
        }
    }
}
