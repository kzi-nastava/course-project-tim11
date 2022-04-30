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
            Console.WriteLine("\nWhat would you like to do?");
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
            DateTime dateTime = DateTime.Now;

            Console.Write("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");

            DateTime date = GetGoodDate();
            
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
                else
                {
                    if (CheckAppointment(time)) dateTime = time;
                    else { Console.WriteLine("You are not availible at that time."); return; };
                }
            } while (time < DateTime.Now);

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
            SystemFunctions.AllExamtinations.Add(id, examination);
            SystemFunctions.CurrentExamtinations.Add(id, examination);
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
            DateTime now = DateTime.Now;
            foreach (Examination examination in this.Examinations)
            {
                Console.WriteLine($"\n\n{i}. Examination\n\nId: {examination.ID}; \nTime and Date: {examination.DateTime};\nPatient last name: {examination.Patient.LastName}; Patient name: {examination.Patient.Name}\n");

                i++;
            }
            
        }
        //=======================================================================================================================================================================
        // UPDATE
        private void EditExamination()
        {
            bool quit = false;
            Examination examination = null;
            while (examination == null)
            {
                Console.WriteLine("Enter the ID of the examination you wish to edit?");
                int id = OtherFunctions.EnterNumber();
                foreach (Examination tmp in this.Examinations)
                {
                    if (tmp.ID == id)
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
                DateTime newDate;
                do{
                    newDate = OtherFunctions.AskForDate();
                    if (newDate.Date < DateTime.Now.Date)
                    {
                        Console.WriteLine("You can't enter a date that's in the past");
                    }
                    else
                    {
                        newDate += examination.DateTime.TimeOfDay;
                        if (CheckAppointment(newDate)) examination.DateTime = newDate;
                        else
                        {
                            Console.WriteLine("You are not availible at that time.");
                            return;
                        }
                    }
                } while (newDate.Date < DateTime.Now.Date);
                
            }
            else if (choice.ToUpper() == "T")
            {
                Console.WriteLine("Enter the new time of your Examination (e.g. 12:00)");
                DateTime newTime;
                do
                {
                    newTime = OtherFunctions.AskForTime();
                    newTime = examination.DateTime.Date + newTime.TimeOfDay;
                    if (newTime < DateTime.Now)
                    {
                        Console.WriteLine("You can't enter that time, its in the past");
                    }
                    else
                    {
                        if (CheckAppointment(newTime)) examination.DateTime = newTime;
                        else
                        {
                            Console.WriteLine("You are not availible at that time.");
                            return;
                        }
                    }
                } while (newTime < DateTime.Now);
                               
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
            Console.WriteLine("Enter a date for which you wish to see your schedule: ");
            DateTime date = GetGoodDate();
            Console.WriteLine($"Examinations on date: {date.ToShortDateString()} and the next three days: \n");

            foreach (Examination examination in this.Examinations)
            {
                if (date.Date <= examination.DateTime.Date && examination.DateTime.Date <= date.Date.AddDays(3))
                {
                    examination.ViewExamination();
                    Console.WriteLine();

                }

            }
            string choice = "Y";
            while (choice.ToUpper() == "Y")
            {
                Console.Write("Do you wish to view additional detail for any examination?(y/n)\n>> ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    Console.Write("\n\nEnter the ID of the examination you wish to view\n>> ");
                    int id = OtherFunctions.EnterNumber();
                    Examination chosenExamination;
                    if (!SystemFunctions.CurrentExamtinations.TryGetValue(id, out chosenExamination))
                    {
                        Console.WriteLine("No examination with that ID found");
                        return;
                    }
                    Console.WriteLine("Searching for medical record");
                    HealthRecord healthRecord;
                    if (!SystemFunctions.HealthRecords.TryGetValue(chosenExamination.Patient.UserName, out healthRecord))
                    {
                        Console.WriteLine("No health record found, creating a new record");
                        healthRecord = new HealthRecord(chosenExamination.Patient);
                        SystemFunctions.HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
                    }
                    Console.WriteLine($"Information about patient:");
                    healthRecord.Patient.ViewPatient();
                    healthRecord.ShowHealthRecord();
                }
            }
            Console.WriteLine("Do you wish to perform an examination(y/n)?");
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                Console.Write("\n\nEnter the ID of the examination you wish to view\n>> ");
                int id = OtherFunctions.EnterNumber();

                Examination chosenExamination;
                if (!SystemFunctions.CurrentExamtinations.TryGetValue(id, out chosenExamination))
                {
                    Console.WriteLine("No examination with that ID found");
                    return;
                }
                PerformExamination(chosenExamination);

            }
        

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
                healthRecord = new HealthRecord(examination.Patient);
                SystemFunctions.HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
            }
            healthRecord.ShowHealthRecord();
            Console.WriteLine("\nWrite you Anamnesis: ");
            string anamnesisText = Console.ReadLine();
            Anamnesis anamnesis = new Anamnesis(anamnesisText, this);
            healthRecord.Anamneses.Add(anamnesis);
            Console.WriteLine("Anamnesis added\nDo you want to change medical record?(y/n)");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {

                Console.Write("Change weight?(y/n): ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    Console.Write("New weight: ");
                    double weight = OtherFunctions.EnterDouble();
                    healthRecord.Weight = weight;
                }
                Console.Write("Change height? (y/n): ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    Console.Write("New height: ");
                    double height = OtherFunctions.EnterDouble();
                    healthRecord.Height = height;
                }
                Console.Write("Add to medical history(y/n): ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    string illness = Console.ReadLine();
                    healthRecord.MedicalHistory.Add(illness);
                }
                Console.Write("Add to list of allergies(y/n): ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    string alergy = Console.ReadLine();
                    healthRecord.Alergies.Add(alergy);
                }
            }

            if (!SystemFunctions.HealthRecords.TryAdd(examination.Patient.UserName, healthRecord))
            {
                SystemFunctions.HealthRecords[examination.Patient.UserName] = healthRecord;
            }
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
                    if ((examination.DateTime <= dateTime && examination.DateTime.AddMinutes(15) > dateTime) || (dateTime < examination.DateTime && dateTime.AddMinutes(15) > examination.DateTime))
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
        public DateTime GetGoodDate() {
            DateTime date;
            do
            {
                date = OtherFunctions.AskForDate();
                if (date.Date < DateTime.Now.Date)
                {
                    Console.WriteLine("You can't enter a date that's in the past");
                }
            } while (date.Date < DateTime.Now.Date);
            return date;
        }
    }

}
