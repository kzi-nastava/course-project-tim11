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
        Unblocked,Secretary,System
    };
    
    public class Patient : User
    {
        public Blocked Blocked { get; set; }
        public List<Examination> Examinations { get; set; }
        public static Dictionary<DateTime, string> ActivityHistory { get; set; } = new Dictionary<DateTime, string>();

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
            ActivityHistory = new Dictionary<DateTime, string>();
            LoadActivityHistory();
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
            ActivityHistory = new Dictionary<DateTime, string>();
            LoadActivityHistory();
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role + "|" + Blocked.ToString();
        }

        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Make appointment");
            Console.WriteLine("3: Edit appointment");
            Console.WriteLine("4: Cancel appointment");
            Console.WriteLine("0: Exit");

            return 1;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    //TODOs
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

        public void LoadActivityHistory()
        {
            string fileName = this.UserName + "activity.txt";
            using (StreamReader reader = new StreamReader("../../../Data/"+fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tmp = line.Split("|");
                    DateTime datetime = Convert.ToDateTime(tmp[0]);
                    ActivityHistory.Add(datetime, tmp[1]);
                }
            }
        }

        public static void ViewAllDoctors()
        {
            int i = 1;
            foreach (KeyValuePair<string,Doctor>entry in SystemFunctions.Doctors)
            {
                Doctor doctor = entry.Value;
                Console.WriteLine($"{i}.User name: {doctor.UserName} ; Name: {doctor.Name}; Last Name: {doctor.LastName}");
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


        private void CreateExamination()
        {
            Console.WriteLine("Enter the date of your Examination(e.g 22/10/2022):");
            DateTime date = OtherFunctions.AskForDate();
            if(date == null)
            {
                CreateExamination();
            }
            Console.WriteLine("Enter the time of your Examination (e.g. 14:30):");
            DateTime time = OtherFunctions.AskForTime();
            if(time == null)
            {
                CreateExamination();
            }

            DateTime dateTime = date.Date.Add(time.TimeOfDay);

            Console.WriteLine("Enter the username of doctor. Do you want to view the list of doctors? (y/n)");
            Console.WriteLine(">>");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                Console.WriteLine("view doctors implement");
            }
            Console.WriteLine("Enter the username:");
            string userName = Console.ReadLine();
            Doctor doctor = null;
            if(!SystemFunctions.Doctors.TryGetValue(userName, out doctor))
            {
                Console.WriteLine("Doctor with that username does not exist");
                return;
            }
            bool validateAppointment = doctor.CheckAppointment(dateTime);
            if (validateAppointment == false)
            {
                Console.WriteLine("Your doctor is unavailable.");
                return;
            }
            int id;
            try
            {
                string lastline = File.ReadLines(SystemFunctions.ExaminationsFilePath).Last();
                string[] tmp = lastline.Split("|");
                id = Convert.ToInt32(tmp[0]) + 1;
            }
            catch (System.InvalidOperationException)
            {
                id = 1;
            }
            Examination examination = new Examination(id, dateTime, doctor,this,false);
            ActivityHistory.Add(DateTime.Now, "CREATE");
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
                            ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                            return;
                        }
                        this.Examinations.Remove(examination);
                        Examination deletedExamination = new Examination(examination.ID, examination.DateTime,examination.Doctor,this,true);
                        SystemFunctions.AllExamtinations.Add(deletedExamination.ID, deletedExamination);
                        SystemFunctions.CurrentExamtinations.Remove(examination.ID);
                        examination.Doctor.Examinations.Remove(examination);
                        ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    }
                    break;
                }
            }
        }

        private void EditExamination()
        {
            bool quit = false;
            Console.WriteLine("Enter the ID of the examination you want to edit:");
            string id = Console.ReadLine();
            Examination examination = null;

            while (examination == null)
            {
                foreach (Examination tmp in this.Examinations)
                {
                    if (tmp.ID == Convert.ToDouble(id))
                    {
                        examination = tmp;
                        break;
                    }
                }
                if (examination == null)
                {
                    Console.WriteLine($"No examinations matches ID: {id}");
                    quit = OtherFunctions.AskQuit();
                    if (quit) return;
                }            
            }
            DateTime dayBefore = examination.DateTime - TimeSpan.FromDays(1);
            int actionValidation = DateTime.Compare(DateTime.Now, dayBefore);
            if (actionValidation>0)
            {
                Console.WriteLine("Sorry, you can not perform this action.");
                Console.WriteLine("You can change the appointment the day before the appointment at the latest/");
                return;
            }

            DateTime requestPeriod = examination.DateTime - TimeSpan.FromDays(2);
            int requestValidation = DateTime.Compare(DateTime.Now, requestPeriod);
            Console.WriteLine("Do you want to edit date or time or the doctor? (d/t/dr)");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "D")
            {
                Console.WriteLine("Enter the new date of your Examination (e.g 22/10/2022):");
                DateTime newDate = OtherFunctions.AskForDate();
                newDate += examination.DateTime.TimeOfDay;
                bool validation = examination.Doctor.CheckAppointment(newDate);
                if(validation == false)
                {
                    Console.WriteLine("Doctor is not available");
                    return;
                }
                //dodaje secretary request
                if (!(requestValidation < 0))
                {
                    Console.WriteLine("You can not perform this activity. Your request will be sent to secretary.");
                    string line = examination.ID.ToString() + "|" + "UPDATE" + "|" + examination.DateTime.ToString("dd/MM/yyyy") + "|" + examination.Doctor.UserName;
                    using (StreamWriter sw = File.AppendText(SystemFunctions.PatientRequestsFilePath))
                    {
                        sw.WriteLine(line);
                    }
                    ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                examination.DateTime = newDate;
                
            }
            else if (choice.ToUpper() == "T")
            {
                //proveri kada se radi izmena i proveri da li je dostupan doktor
                Console.WriteLine("Enter the new time of your Examination (e.g. 12:00)");
                DateTime newTime = OtherFunctions.AskForTime();
                DateTime oldTime = examination.DateTime;
                examination.DateTime.Date.Add(newTime.TimeOfDay);
                bool validation = examination.Doctor.CheckAppointment(examination.DateTime);
                if(validation == false)
                {
                    Console.WriteLine("Doctor is not available.");
                    examination.DateTime = oldTime;
                    return;
                }
                if (!(requestValidation < 0))
                {
                    Console.WriteLine("You can not perform this activity. Your request will be sent to secretary.");
                    string line = examination.ID.ToString() + "|" + "UPDATE" + "|" + examination.DateTime.ToString("dd/MM/yyyy") + "|" + examination.Doctor.UserName;
                    using (StreamWriter sw = File.AppendText(SystemFunctions.PatientRequestsFilePath))
                    {
                        sw.WriteLine(line);
                    }
                    ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                //secretary request

            }
            else if (choice.ToUpper() == "DR")
            {
                Console.WriteLine("Do you want to view list of doctors? (y/n)");
                Console.WriteLine(">>");
                string input = Console.ReadLine();
                if (input.ToUpper() == "Y")
                {
                    ViewAllDoctors();
                }
                Console.WriteLine("Write username of new doctor:");
                string inputUserName = Console.ReadLine();
                Doctor doctor = null;
                if(!SystemFunctions.Doctors.TryGetValue(UserName, out doctor))
                {
                    Console.WriteLine("Doctor with that user name does not eixst.");
                    return;
                }
                bool validate = doctor.CheckAppointment(examination.DateTime);
                if(validate == false)
                {
                    Console.WriteLine("Doctor is not available");
                    return;
                }
                //proveri kada se radi izmena
                
                examination.Doctor = doctor;
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        private void AntiTroll()
        {
            int update_delete = 0;
            int make = 0;
            DateTime today = DateTime.Now;
            DateTime monthBefore = DateTime.Now - TimeSpan.FromDays(30);
            foreach(KeyValuePair<DateTime,string> activity in ActivityHistory)
            {
                DateTime date = activity.Key;
                string activity_performed = activity.Value;
                int lower_limit = DateTime.Compare(date, monthBefore);
                int upper_limit = DateTime.Compare(date,today);
                if (lower_limit<0 || upper_limit>0)
                {
                    continue;
                }
                if(activity_performed == "MAKE")
                {
                    make += 1;
                }
                else
                {
                    update_delete += 1;
                }
                if(make >= 8 || update_delete >= 5)
                {
                    Console.WriteLine("You have been blocked by system.");
                    this.Blocked = Blocked.System;
                    //todo system exit
                }
            }
        }
    }
}
