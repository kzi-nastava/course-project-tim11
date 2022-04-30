using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.Users
{
    public class Doctor : User
    {
        public List<Examination> Examinations;
        public Doctor(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Doctor;
            Examinations = new List<Examination>();
        }

        public Doctor(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Doctor;
            Examinations = new List<Examination>();
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }


        //=======================================================================================================================================================================
        // MENU

        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2. Manage examinations");
            Console.WriteLine("3. View schedule");
            Console.WriteLine("0: Exit");

            return 3;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    ManageExaminations();
                    break;
                case 3:
                    ViewSchedule();
                    break;
                default:
                    break;
            }
        }
        private void ManageExaminations()
        {
            Console.WriteLine("Chose how you wish to manage your examinations: ");
            string options = "\n1. Create\n2. View\n3. Edit(by ID)\n4. Delete(by ID)\n";
            Console.Write($"{options}Write the number of your choice\n>> ");
            int choice = OtherFunctions.EnterNumberWithLimit(1, 4);
            switch (choice)
            {
                case 1:
                    CreateExamination();
                    break;
                case 2:
                    ViewExaminations();
                    break;
                case 3:
                    EditExamination();
                    break;
                case 4:
                    DeleteExamination();
                    break;
            }
        }

        //=======================================================================================================================================================================
        // CREATE
        private void CreateExamination()
        {
            Console.Write("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");
            DateTime date = OtherFunctions.AskForDate();
            if (date == null)
            {
                CreateExamination();
            }
            Console.Write("\nEnter the time of your Examination (e.g. 14:30)\n>> ");
            DateTime time = OtherFunctions.AskForTime();
            if (time == null)
            {
                CreateExamination();
            }

            DateTime dateTime = date.Date.Add(time.TimeOfDay);


            bool availible = CheckAppointment(dateTime);
            if (!availible)
            {
                Console.WriteLine("You are not availible at that time");
                return;
            }

            Console.WriteLine("Enter the username of the patient. Do you want to view the list of all patients first (y/n)");
            Console.Write(">> ");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                Patient.ViewAllPatients();
            }
            Console.Write("\nEnter the username: ");
            string userName = Console.ReadLine();
            Patient patient = null;
            if (!SystemFunctions.Patients.TryGetValue(userName, out patient))
            {
                Console.WriteLine("Patient with that username does not exist.");
                return;
            }
            int id;
            try
            {
                string lastLine = File.ReadLines(SystemFunctions.ExaminationsFilePath).Last();
                string[] tmp = lastLine.Split('|');
                id = Convert.ToInt32(tmp[0]) + 1;
            }
            catch (System.InvalidOperationException)
            {
                id = 1;
            }
            Examination examination = new Examination(id, dateTime, this, patient, false);
            InsertExamination(examination);
            patient.InsertExamination(examination);
            examination.ToFile();
            Console.WriteLine("\nNew examination successfully created\n");

        }
        //=======================================================================================================================================================================
        // READ

        private void ViewExaminations()
        {

            if (this.Examinations.Count == 0)
            {
                Console.WriteLine("\nNo future examinations\n");
                return;
            }

            int i = 1;
            Examination currentExamination = null;
            DateTime now = DateTime.Now;
            foreach (Examination examination in this.Examinations)
            {
                if (!CheckAppointment(now))
                {
                    currentExamination = examination;
                }
                Console.WriteLine($"\n\n{i}. Examination\n\nId: {examination.ID}; \nTime and Date: {examination.DateTime};\nPatient last name: {examination.Patient.LastName}; Patient name: {examination.Patient.Name}\n");

                i++;
            }
            if (currentExamination != null)
            {
                Console.WriteLine("\n\nDo you want to start this Examination?");
                Console.WriteLine($"\nId: {currentExamination.ID}; \nTime and Date: {currentExamination.DateTime};\nPatient last name: {currentExamination.Patient.LastName}; Patient name: {currentExamination.Patient.Name}]n");
                Console.Write("\ny/n >> ");
                string start = Console.ReadLine();
                if (start.ToUpper() == "Y")
                {
                    PerformExamination(currentExamination);
                }
            }
        }
        //=======================================================================================================================================================================
        // UPDATE
        private void EditExamination()
        {
            bool quit = false;
            Console.WriteLine("Enter the ID of the examination you wish to edit?");
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
                    Console.WriteLine($"No examination matches ID: {id}");
                    quit = OtherFunctions.AskQuit();
                    if (quit) return;
                }

            }


            Console.WriteLine("Do you want to edit the date or the time? (d/t)");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "D")
            {
                Console.WriteLine("Enter the new date of your Examination (e.g. 22/10/1987)");
                DateTime newDate = OtherFunctions.AskForDate();
                newDate += examination.DateTime.TimeOfDay;
                examination.DateTime = newDate;
            }
            else if (choice.ToUpper() == "T")
            {
                Console.WriteLine("Enter the new time of your Examination (e.g. 12:00)");
                DateTime newTime = OtherFunctions.AskForTime();
                examination.DateTime.Date.Add(newTime.TimeOfDay);
            }
            else
            {
                Console.WriteLine("Not a valid choice");
            }
        }
        //=======================================================================================================================================================================
        // DELETE
        private void DeleteExamination()
        {
            Console.WriteLine("Enter the ID of the examination you wish to delete?");
            string id = Console.ReadLine();
            Examination examination = null;
            foreach (Examination tmp in this.Examinations)
            {
                if (tmp.ID == Convert.ToInt32(id))
                {
                    examination = tmp;
                    Console.WriteLine("Are you sure? (y/n)");
                    string choice = Console.ReadLine();
                    if (choice.ToUpper() == "Y") {
                        this.Examinations.Remove(examination);
                        Examination deletedExamination = new Examination(examination.ID, examination.DateTime, this, examination.Patient, true);
                        SystemFunctions.AllExamtinations.Add(deletedExamination.ID, deletedExamination);
                        SystemFunctions.CurrentExamtinations.Remove(examination.ID);
                        deletedExamination.ToFile();
                    }
                    break;
                }
            }

        }

        //=======================================================================================================================================================================
        // VIEW SCHEDULE
        private void ViewSchedule()
        {

        }

        //=======================================================================================================================================================================
        // PERFORM EXAMINATION
        private void PerformExamination(Examination examination)
        {
            Console.WriteLine("Examination starting. Searching for medical record");
            HealthRecord healthRecord;
            if (!SystemFunctions.HealthRecords.TryGetValue(examination.Patient.UserName, out healthRecord))
            {
                Console.WriteLine("No health record found, creating a new record");
                //healthRecord = new HealthRecord();
            }
            healthRecord.ShowHealthRecord();
            Console.WriteLine("Write you Anamnesis: ");
            string anamnesisText = Console.ReadLine();
            //Anamnesis anamnesis = new Anamnesis(anamnesisText, this);
            healthRecord.Anamneses.Add(anamnesis);
            Console.WriteLine("Anamnesis added\nDo you want to change medical record?(y/n)");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {

                Console.WriteLine("Change weight?(y/n)");
                if (choice.ToUpper() == "Y")
                {
                    Console.Write("New weight: ");
                    double weight = Convert.ToDouble(Console.ReadLine());
                    healthRecord.Weight = weight;
                    Console.Write("New height: ");
                    double height = Convert.ToDouble(Console.ReadLine());
                    healthRecord.Height = height;
                    Console.Write("Add to medical history: ");
                    string illness = Console.ReadLine();
                    healthRecord.MedicalHistory.Add(illness);
                    Console.Write("Add to list of allergies: ");
                    string alergy = Console.ReadLine();
                    healthRecord.Alergies.Add(alergy);

                }
            }

            SystemFunctions.HealthRecords.Add(examination.Patient.UserName, healthRecord);
            this.Examinations.Remove(examination);
            examination.Patient.Examinations.Remove(examination);
            Console.WriteLine("Examination ended");
        }


        //=======================================================================================================================================================================
        // OTHER FUNCTIONALITIES
        public bool CheckAppointment(DateTime dateTime)
        {
            foreach (Examination examination in this.Examinations)
            {
                if (examination.DateTime.Date == dateTime.Date)
                {
                    if (examination.DateTime <= dateTime && examination.DateTime.AddMinutes(15) > dateTime)
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

        public void InsertExamination(Examination newExamination)
        {
            if (this.Examinations.Count() == 0)
            {
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
