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
        public List<Appointment> Appointments { get; set; }
        public List<Referral> Referrals { get; set; }
        public static Dictionary<DateTime, string> ActivityHistory { get; set; } = new Dictionary<DateTime, string>();

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        public Patient(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender, Blocked blocked)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Patient;
            MessageBox = new MessageBox(this);
            Blocked = blocked;
            Appointments = new List<Appointment>();
            Referrals = new List<Referral>();
            ActivityHistory = new Dictionary<DateTime, string>();
            LoadActivityHistory();
            Prescriptions = new List<Prescription>();
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
            MessageBox = new MessageBox(this);
            Blocked temp;
            try
            {
                if (Blocked.TryParse(data[7], out temp))
                    this.Blocked = temp;
            }
            catch {
                this.Blocked = Blocked.Unblocked;
            }


            Appointments = new List<Appointment>();
            Referrals = new List<Referral>();
            ActivityHistory = new Dictionary<DateTime, string>();
            Prescriptions = new List<Prescription>();
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

            Console.WriteLine("2: Display new messages (" + MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Make appointment");
            Console.WriteLine("4: Edit appointment");
            Console.WriteLine("5: Cancel appointment");
            Console.WriteLine("6: View appointments");
            Console.WriteLine("7: Appointment suggestion");
            Console.WriteLine("8: View health record history");
            Console.WriteLine("9: Search doctors");
            Console.WriteLine("0: Exit");

            return 9;

        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    MessageBox.DisplayMessages();
                    break;
                case 3:
                    CreateExamination();
                    break;
                case 4:
                    EditExamination();
                    break;
                case 5:
                    DeleteExamination();
                    break;
                case 6:
                    ViewExaminations();
                    break;
                case 7:
                    SuggestAppointment();
                    break;
                case 8:
                    ViewAnamnesis();
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

        private void ViewExaminations()
        {

            if (this.Appointments.Count == 0)
            {
                Console.WriteLine("\nNo future examinations\n");
                return;
            }

            int i = 1;
            foreach (Examination examination in this.Appointments)
            {
                Console.WriteLine($"\n\n{i}. Examination\n\nId: {examination.ID}; \nTime and Date: {examination.DateTime};\nDoctor last name: {examination.Doctor.LastName}; Doctor name: {examination.Doctor.Name}\n");

                i++;
            }

        }

        public void LoadActivityHistory()
        {
            string fileName = this.UserName + "activity.txt";
            /*using (StreamReader reader = new StreamReader("../../../Data/"+fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tmp = line.Split("|");
                    DateTime datetime = Convert.ToDateTime(tmp[0]);
                    ActivityHistory.Add(datetime, tmp[1]);
                }
            }*/
        }

        public static void ViewAllDoctors()
        {
            int i = 1;
            foreach (KeyValuePair<string, Doctor> entry in SystemFunctions.Doctors)
            {
                Doctor doctor = entry.Value;
                Console.WriteLine($"{i}.User name: {doctor.UserName} ; Name: {doctor.Name}; Last Name: {doctor.LastName}");
                i++;
            }
        }

        public void InsertAppointment(Appointment newExamination)
        {
            if (this.Appointments.Count() == 0) {
                this.Appointments.Add(newExamination);
                return;
            }
            for (int i = 0; i < this.Appointments.Count(); i++)
            {
                if (this.Appointments[i].DateTime < newExamination.DateTime)
                {
                    Appointments.Insert(i, newExamination);
                    return;
                }
            }
            this.Appointments.Add(newExamination);

        }


        private void CreateExamination(Doctor doctor = null)
        {
            DateTime dateTime = DateTime.Now;

            Console.Write("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");

            DateTime date = OtherFunctions.GetGoodDate();

            Console.Write("\nEnter the time of your Examination (e.g. 14:30)\n>> ");

            DateTime time;
            do
            {
                time = OtherFunctions.AskForTime();
                time = date.Date + time.TimeOfDay;
                if (time < DateTime.Now)
                {
                    Console.WriteLine("You can't enter that time, its in the past");
                }
            } while (time < DateTime.Now);

            dateTime = time;
            if(doctor == null)
            {
                Console.WriteLine("Enter the username of doctor. Do you want to view the list of doctors? (y/n)");
                Console.Write(">>");
                string choice = Console.ReadLine();
                Console.WriteLine();
                if (choice.ToUpper() == "Y")
                {
                    ViewAllDoctors();
                }
                Console.WriteLine("\nEnter the username:");
                string userName = Console.ReadLine();
                Doctor doctorDict = null;
                if (!SystemFunctions.Doctors.TryGetValue(userName, out doctorDict))
                {
                    Console.WriteLine("Doctor with that username does not exist");
                    return;
                }
                doctor = SystemFunctions.Doctors[userName];
                //TODO : types of appointment -> duration
                int duration = 15;
                bool validateAppointment = doctor.CheckAppointment(dateTime, duration);
                if (validateAppointment == false)
                {
                    Console.WriteLine("Your doctor is unavailable at that time.");
                    return;
                }
            }

            /*Console.WriteLine("Enter the username of doctor. Do you want to view the list of doctors? (y/n)");
            Console.Write(">>");
            string choice = Console.ReadLine();
            Console.WriteLine();
            if (choice.ToUpper() == "Y")
            {
                ViewAllDoctors();
            }
            Console.WriteLine("\nEnter the username:");
            string userName = Console.ReadLine();
            Doctor doctorDict = null;
            if (!SystemFunctions.Doctors.TryGetValue(userName, out doctorDict))
            {
                Console.WriteLine("Doctor with that username does not exist");
                return;
            }
            doctor = SystemFunctions.Doctors[userName];
            //TODO : types of appointment -> duration
            int duration = 15;
            bool validateAppointment = doctor.CheckAppointment(dateTime, duration);
            if (validateAppointment == false)
            {
                Console.WriteLine("Your doctor is unavailable at that time.");
                return;
            }*/
            int id;
            try
            {
                id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
            }
            catch
            {
                id = 1;
            }
            Examination examination = new Examination(id, dateTime, doctor, this, false, 0, 0);
            InsertAppointment(examination);
            doctor.InsertAppointment(examination);
            SystemFunctions.AllAppointments.Add(id, examination);
            SystemFunctions.CurrentAppointments.Add(id, examination);
            Console.WriteLine("\nNew examination successfully created\n");
            ActivityHistory.Add(DateTime.Now, "CREATE");
        }

        private void DeleteExamination()
        {
            Console.WriteLine("Enter the ID of the examination you want to delete?");
            int id = OtherFunctions.EnterNumber();
            Examination examination = null;
            foreach (Examination tmp in this.Appointments)
            {
                if (tmp.ID == id)
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
                        if (!(dateValidation < 0))
                        {
                            Console.WriteLine("You can not perform this action.");
                            return;
                        }
                        else if (!(dateValidationSecretary < 0))
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
                        this.Appointments.Remove(examination);
                        var last = SystemFunctions.AllAppointments.Values.Last();
                        Examination deletedExamination = new Examination(last.ID + 1, examination.DateTime, examination.Doctor, this, examination.Finished, examination.ID, examination.Edited);
                        SystemFunctions.AllAppointments.Add(deletedExamination.ID, deletedExamination);
                        SystemFunctions.CurrentAppointments.Remove(examination.ID);
                        examination.Doctor.Appointments.Remove(examination);
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
            int id = OtherFunctions.EnterNumber();
            Examination examination = null;
            int duration = 15;

            while (examination == null)
            {
                foreach (Examination tmp in this.Appointments)
                {
                    if (tmp.ID == id)
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
            if (actionValidation > 0)
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
                //TODO : type of appointment -> duration
                bool validation = examination.Doctor.CheckAppointment(newDate, duration);
                if (validation == false)
                {
                    Console.WriteLine("Doctor is not available");
                    return;
                }
                //secretary request
                if (!(requestValidation < 0))
                {
                    Console.WriteLine("You can not perform this activity. Your request will be sent to secretary.");
                    string line = examination.ID.ToString() + "|" + "UPDATE" + "|" + newDate.ToString("dd/MM/yyyy") + "|" + examination.Doctor.UserName;
                    using (StreamWriter sw = File.AppendText(SystemFunctions.PatientRequestsFilePath))
                    {
                        sw.WriteLine(line);
                    }

                    ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                this.Appointments.Remove(examination);
                examination.Doctor.Appointments.Remove(examination);
                var last = SystemFunctions.AllAppointments.Values.Last();
                Examination editedExamination = new Examination(last.ID + 1, newDate, examination.Doctor, this, examination.Finished, 0, examination.ID);
                SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                SystemFunctions.CurrentAppointments.Remove(examination.ID);
                SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                this.Appointments.Add(editedExamination);
                editedExamination.Doctor.Appointments.Add(editedExamination);

            }
            else if (choice.ToUpper() == "T")
            {
                Console.WriteLine("Enter the new time of your Examination (e.g. 12:00)");
                DateTime newTime = OtherFunctions.AskForTime();
                DateTime oldTime = examination.DateTime;
                examination.DateTime.Date.Add(newTime.TimeOfDay);
                bool validation = examination.Doctor.CheckAppointment(examination.DateTime,duration);
                if (validation == false)
                {
                    Console.WriteLine("Doctor is not available.");
                    examination.DateTime = oldTime;
                    return;
                }
                //secretary request
                if (!(requestValidation < 0))
                {
                    Console.WriteLine("You can not perform this activity. Your request will be sent to secretary.");
                    string line = examination.ID.ToString() + "|" + "UPDATE" + "|" + newTime.ToString("dd/MM/yyyy") + "|" + examination.Doctor.UserName;
                    using (StreamWriter sw = File.AppendText(SystemFunctions.PatientRequestsFilePath))
                    {
                        sw.WriteLine(line);
                    }
                    ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                this.Appointments.Remove(examination);
                examination.Doctor.Appointments.Remove(examination);
                var last = SystemFunctions.AllAppointments.Values.Last();
                Examination editedExamination = new Examination(last.ID + 1, newTime, examination.Doctor, this, examination.Finished, 0, examination.ID);
                SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                SystemFunctions.CurrentAppointments.Remove(examination.ID);
                SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                this.Appointments.Add(editedExamination);
                editedExamination.Doctor.Appointments.Add(editedExamination);

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
                if (!SystemFunctions.Doctors.TryGetValue(UserName, out doctor))
                {
                    Console.WriteLine("Doctor with that user name does not eixst.");
                    return;
                }
                bool validate = doctor.CheckAppointment(examination.DateTime,duration);
                if (validate == false)
                {
                    Console.WriteLine("Doctor is not available");
                    return;
                }
                if (!(requestValidation < 0))
                {
                    Console.WriteLine("You can not perform this activity. Your request will be sent to secretary.");
                    string line = examination.ID.ToString() + "|" + "UPDATE" + "|" + examination.DateTime.ToString("dd/MM/yyyy") + "|" + doctor.UserName;
                    using (StreamWriter sw = File.AppendText(SystemFunctions.PatientRequestsFilePath))
                    {
                        sw.WriteLine(line);
                    }
                    ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                //proveri kada se radi izmena

                this.Appointments.Remove(examination);
                examination.Doctor.Appointments.Remove(examination);
                var last = SystemFunctions.AllAppointments.Values.Last();
                Examination editedExamination = new Examination(last.ID + 1, examination.DateTime, doctor, this, examination.Finished, 0, examination.ID);
                SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                SystemFunctions.CurrentAppointments.Remove(examination.ID);
                SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                this.Appointments.Add(editedExamination);
                editedExamination.Doctor.Appointments.Add(editedExamination);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        public void AntiTroll()
        {
            int update_delete = 0;
            int make = 0;
            DateTime today = DateTime.Now;
            DateTime monthBefore = DateTime.Now - TimeSpan.FromDays(30);
            foreach (KeyValuePair<DateTime, string> activity in ActivityHistory)
            {
                DateTime date = activity.Key;
                string activity_performed = activity.Value;
                int lower_limit = DateTime.Compare(date, monthBefore);
                int upper_limit = DateTime.Compare(date, today);
                if (lower_limit < 0 || upper_limit > 0)
                {
                    continue;
                }
                if (activity_performed == "MAKE")
                {
                    make += 1;
                }
                else
                {
                    update_delete += 1;
                }
                if (make >= 8 || update_delete >= 5)
                {
                    Console.WriteLine("You have been blocked by system.");
                    this.Blocked = Blocked.System;
                    return;
                }
            }
        }

        public void ViewPatient() {
            Console.WriteLine($"Patient {Name} {LastName};\nDate of birth {DateOfBirth.ToShortDateString()}; Gender:\n{Gender}");
        }

        public bool CheckAppointment(DateTime dateTime, int duration)
        {
            foreach (Examination examination in this.Appointments)
            {
                if (examination.DateTime.Date == dateTime.Date)
                {
                    if ((examination.DateTime <= dateTime && examination.DateTime.AddMinutes(duration) > dateTime) || (dateTime <= examination.DateTime && dateTime.AddMinutes(duration) > examination.DateTime))
                    {
                        return false;
                    }
                }
                if (examination.DateTime.Date > dateTime.Date)
                {
                    break;
                }

            }
            return true;
        }

        private void SuggestAppointment()
        {
            int duration = 15;
            //todo take user input for doctor and time for examination, also time period for appoinment to be done and priority(doctor or time of examination)
            Console.WriteLine("You are currently using the appointment suggestion system.");
            Console.WriteLine("System will suggest your appointment by priority, your priority can be doctor or time of appointment.");
            Console.WriteLine("Please enter the date and time by which the appointment must be made at the latest.");
            DateTime lastAppointment = DateTime.Now;

            Console.Write("\nPlease enter the date for the last possible Examination (e.g. 22/10/1987)\n>> ");

            DateTime date = OtherFunctions.GetGoodDate();

            Console.Write("\nenter the time for the last possible Examination (e.g. 14:30)\n>> ");

            DateTime time;
            do
            {
                time = OtherFunctions.AskForTime();
                time = date.Date + time.TimeOfDay;
                if (time < DateTime.Now)
                {
                    Console.WriteLine("You can't enter that time, its in the past");
                }
            } while (time < DateTime.Now);

            lastAppointment = time;

            //todo take time of appointment
            Console.WriteLine("Please enter the preferred time of your Examination in format [HH:mm]:");
            DateTime preferredTime = OtherFunctions.AskForTime();


            Console.WriteLine("Enter the username of your preferred doctor. Do you want to view the list of doctors? (y/n)");
            Console.Write(">>");
            string choice = Console.ReadLine();
            Console.WriteLine();
            if (choice.ToUpper() == "Y")
            {
                ViewAllDoctors();
            }
            Console.WriteLine("\nEnter the username:");
            string userName = Console.ReadLine();
            Doctor doctor = null;
            if (!SystemFunctions.Doctors.TryGetValue(userName, out doctor))
            {
                Console.WriteLine("Doctor with that username does not exist");
                return;
            }
            doctor = SystemFunctions.Doctors[userName];

            Console.WriteLine("Please enter the priority for your search. Enter 'd' if doctor is your priority, enter 'a' if appointment is your priority.");
            string priority = Console.ReadLine();
            //first check preferred doctor and preferred time
            DateTime initial_appointment = DateTime.Today + preferredTime.TimeOfDay;
            bool available = doctor.CheckAppointment(initial_appointment, duration);
            if (available)
            {
                Console.WriteLine("Your doctor is available, congrats you made appointment.");
                int id;
                try
                {
                    id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
                }
                catch
                {
                    id = 1;
                }
                Examination examination = new Examination(id, initial_appointment, doctor, this, false, 0, 0);
                InsertAppointment(examination);
                doctor.InsertAppointment(examination);
                SystemFunctions.AllAppointments.Add(id, examination);
                SystemFunctions.CurrentAppointments.Add(id, examination);
                //Console.WriteLine("\nNew examination successfully created\n");
                ActivityHistory.Add(DateTime.Now, "CREATE");
                return;
            }
            if (priority.ToUpper() == "D")
            {
                //todo doctor priority
                bool availableDoctor = SuggestDoctorPriority(ref doctor, lastAppointment, preferredTime);
                if (!availableDoctor)
                {
                    Console.WriteLine("Sorry your doctor is not available in this period of time.");
                    //todo give three appointments for patient
                }
            }
            else if (priority.ToUpper() == "A")
            {
                //todo appointment priority
                bool availableAppointment = SuggestAppointmentPriority(preferredTime, lastAppointment);
                if (!availableAppointment)
                {
                    Console.WriteLine("Sorry your prefered appointment is not available in requested time span.");
                    //todo give three appointments for patient
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
                return;
            }
        }


        //suggest appointment doctor priority
        private bool SuggestDoctorPriority(ref Doctor doctor, DateTime lastAppointment, DateTime preferredTime)
        {
            int duration = 15;
            bool appointmentFound = false;
            DateTime today = DateTime.Today + preferredTime.TimeOfDay;
            while (today < lastAppointment)
            {
                bool available = doctor.CheckAppointment(today, duration);
                today = today + TimeSpan.FromMinutes(15);
                if (available)
                {
                    Console.WriteLine("Your doctor is available. You just made appointment.");
                    Console.WriteLine("Date of your appointment is:" + today.ToString() + ".");
                    int id;
                    try
                    {
                        id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
                    }
                    catch
                    {
                        id = 1;
                    }
                    Examination examination = new Examination(id, today, doctor, this, false, 0, 0);
                    InsertAppointment(examination);
                    doctor.InsertAppointment(examination);
                    SystemFunctions.AllAppointments.Add(id, examination);
                    SystemFunctions.CurrentAppointments.Add(id, examination);
                    //Console.WriteLine("\nNew examination successfully created\n");
                    ActivityHistory.Add(DateTime.Now, "CREATE");
                    return true;
                }
            }
            return appointmentFound;
        }

        //suggest appointment appointment priority
        private bool SuggestAppointmentPriority(DateTime preferredTime, DateTime lastAppointment)
        {
            int duration = 15;
            bool appoinmentFound = false;
            DateTime preferredAppointment = DateTime.Today + preferredTime.TimeOfDay;
            while (preferredAppointment < lastAppointment)
            {
                foreach (Doctor doctor in SystemFunctions.Doctors.Values)
                {
                    bool check = doctor.CheckAppointment(preferredAppointment, duration);
                    if (check)
                    {
                        Console.WriteLine("Your preferred appointment is available in your requested timespan.");
                        int id;
                        try
                        {
                            id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
                        }
                        catch
                        {
                            id = 1;
                        }
                        Examination examination = new Examination(id, preferredAppointment, doctor, this, false, 0, 0);
                        InsertAppointment(examination);
                        doctor.InsertAppointment(examination);
                        SystemFunctions.AllAppointments.Add(id, examination);
                        SystemFunctions.CurrentAppointments.Add(id, examination);
                        //Console.WriteLine("\nNew examination successfully created\n");
                        ActivityHistory.Add(DateTime.Now, "CREATE");
                        return true;
                    }
                }
            }
            return appoinmentFound;
        }

        private void ViewAnamnesis()
        {
            HealthRecord healthRecord = null;
            if (!SystemFunctions.HealthRecords.TryGetValue(this.UserName, out healthRecord))
            {
                Console.WriteLine("Health record with that username does not exist");
                return;
            }
            healthRecord = SystemFunctions.HealthRecords[this.UserName];

            Console.WriteLine("Patient health record.");
            healthRecord.ShowHealthRecord();
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("1.Sort amnesis list by date.");
            Console.WriteLine("2.Sort amnesis list by doctor.");
            Console.WriteLine("3.Find amnesis that contains specific word.");
            Console.WriteLine("4.Return to home page.");
            Console.WriteLine();
            Console.WriteLine();
            string user_input = Console.ReadLine();
            if (user_input == "1")
            {
                //sort by date
                healthRecord.AnamnesisSortedDate();
            }
            else if (user_input == "2")
            {
                //sort by doctor
                healthRecord.AnamnesisSortedDoctor();
            }
            else if (user_input == "3")
            {
                Console.WriteLine("Enter the specific word for amnesis search:");
                string specific_word = Console.ReadLine();
                healthRecord.SearchAnamnesis(specific_word);
            }
            else if (user_input == "4")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return;
            }
        }


        //TODO kontrolna tacka broj tri
        private void SearchDoctors()
        {
            //pretrazi po imenu ili prezimenu ili uzoj oblasti
            //sortiraj po oceni ili po parametrima pretrage
            Console.WriteLine("You can search doctors by first name, last name and their field of work.");
            Console.WriteLine("Please enter the criteria for doctor search, seperate parameters of search with ',':");
            Console.WriteLine(">>");
            string userInput = Console.ReadLine();
            string[] parametersOfSearch = userInput.Split(',');
            if(parametersOfSearch.Length == 1)
            {
                SearchDoctorsOneParameter(parametersOfSearch);
            }
            else if(parametersOfSearch.Length == 2)
            {
                SearchDoctorTwoParameters(parametersOfSearch);
            }
            else if(parametersOfSearch.Length == 3)
            {
                //SearchDoctorAllParameters(parametersOfSearch);
                Console.WriteLine("Method is implemented.");
                return;
            }
            else
            {
                Console.WriteLine("Invalid number of search parameters.");
                return;
            }

        }

        private void SearchDoctorsOneParameter(string[] parameters)
        {
            //validate parameters
            if(parameters[0] == "first name")
            {
                SearchDoctorByFirstName();
            }
            else if(parameters[0] == "last name")
            {
                SearchDoctorByLastName();
            }
            else if(parameters[0] == "field")
            {
                SearchDoctorByField();
            }
        }

        private void SearchDoctorByFirstName()
        {
            Console.WriteLine("Please enter the first name of your doctor.");
            Console.WriteLine(">>");
            string name = Console.ReadLine();
            //todo find doctors by name
        }

        private void SearchDoctorByLastName()
        {
            Console.WriteLine("Please enter the last name of your doctor.");
            Console.WriteLine(">>");
            string lastName = Console.ReadLine();
            //todo find doctors by last name
        }

        private void SearchDoctorByField()
        {
            Console.WriteLine("Please enter your doctors field of work.");
            Console.WriteLine(">>");
            string field = Console.ReadLine();
            //todo find doctors by field of work
        }

        private void SearchDoctorTwoParameters(string[] parameters)
        {
            string firstName="";
            string lastName="";
            string field="";
            bool parameter0 = false;
            bool parameter1 = false;
            bool parameter2 = false;
            if (ValidateParameter(parameters[0]) && ValidateParameter(parameters[1]))
            {
                if(parameters.Contains<string>("first name"))
                {
                    Console.WriteLine("Input the first name you want to search.");
                    Console.WriteLine(">>");
                    firstName = Console.ReadLine();
                    parameter0 = true;
                }
                if(parameters.Contains<string>("last name"))
                {
                    Console.WriteLine("Input the last name you want to search.");
                    Console.WriteLine(">>");
                    lastName = Console.ReadLine();
                    parameter1 = true;
                }
                if (parameters.Contains<string>("field"))
                {
                    Console.WriteLine("Enter the field you want to search.");
                    Console.WriteLine(">>");
                    field = Console.ReadLine();
                    parameter2 = true;
                }
            }
            Console.WriteLine("Invalid search parameters.");
            return;
        }

        /*private void SearchDoctorAllParameters(string[] parameters)
        {
            //RAZMISLI DA LI CES OVO DA IMPLEMENTIRAS
            if (ValidateParameter(parameters[0]) && ValidateParameter(parameters[1]) && ValidateParameter(parameters[2]))
            {
                //TODO search by all parameters
                Console.WriteLine("Please input first name you want to search.");
                Console.WriteLine(">>");
                string firstName = Console.ReadLine();
                Console.WriteLine("Please input last name you want to search");

                
            }
            Console.WriteLine("Invalid parameters.");
            return;
        }*/

        private bool ValidateParameter(string parameter)
        {
            //TODO validate parameters
            if (parameter=="first name" || parameter=="last name" || parameter == "field")
            {
                return true;
            }
                return false;
        }


        private List<Doctor> SortDoctorsByFirstName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate(Doctor d1, Doctor d2)
            {
                return d1.Name.CompareTo(d2.Name);
	        });
            return doctorsUnsorted;
        }

        private List<Doctor> SortDoctorsByLastName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

        private List<Doctor> SortDoctorsByField(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

    }
}
