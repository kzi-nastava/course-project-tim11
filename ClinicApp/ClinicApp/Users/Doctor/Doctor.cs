using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.Users.Doctors
{
    public class Doctor : User
    {
        public List<Appointment> Appointments;
        public Fields Field;
        public int RoomId { get; set; } // id of the room in which the doctor works

        public Doctor(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender, Fields field)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Doctor;
            MessageBox = new MessageBox(this);
            Appointments = new List<Appointment>();
            Field = field;
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
            Enum.TryParse(data[7], out Field);
            RoomId = Convert.ToInt32(data[8]);
            MessageBox = new MessageBox(this);
            Appointments = new List<Appointment>();
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role + "|" + Field.ToString() + "|" + Convert.ToString(RoomId);
        }


        //=======================================================================================================================================================================
        // MENU

        public override int MenuWrite()
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage examinations");
            Console.WriteLine("4: View schedule");
            Console.WriteLine("0: Exit");

            return 4;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    MessageBox.DisplayMessages();
                    break;
                case 3:
                    ManageAppointments();
                    break;
                case 4:
                    ViewSchedule();
                    break;
                default:
                    break;
            }
        }
        private void ManageAppointments()
        {
            Console.WriteLine("Chose how you wish to manage your examinations: ");
            string options = "\n1. Create\n2. View\n3. Edit(by ID)\n4. Delete(by ID)\n";
            Console.Write($"{options}Write the number of your choice\n>> ");
            int choice = OtherFunctions.EnterNumberWithLimit(1, 4);
            switch (choice)
            {
                case 1:
                    CreateAppointment();
                    break;
                case 2:
                    ViewAppointments();
                    break;
                case 3:
                    EditAppointment();
                    break;
                case 4:
                    DeleteExamination();
                    break;
            }
        }

        //=======================================================================================================================================================================
        // CREATE
        private void CreateAppointment()
        {
            DateTime dateTime = DateTime.Now;

            Console.Write("\nEnter the date of your Appointment (e.g. 22/10/1987)\n>> ");

            DateTime date = OtherFunctions.GetGoodDate();
            
            Console.Write("\nEnter the time of your Appointment (e.g. 14:30)\n>> ");

            DateTime time;
            int type = 0;
            int duration = 0;
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
                    Console.Write("\nDo you want to create an (1)EXAMINATION or an (2)OPERATION?\n>> ");
                    type = OtherFunctions.EnterNumberWithLimit(0, 3);
                    
                    if (type == 2)
                    {
                        Console.Write("\nEnter the duration of your Operation in minutes (e.g. 60)\n>> ");
                        duration = OtherFunctions.EnterNumberWithLimit(15, 1000);
                    }
                    else duration = 15;
                    if (CheckAppointment(time, duration)) dateTime = time;
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
            int id = 0;
            foreach (int appointmentID in SystemFunctions.AllAppointments.Keys) {
                if (appointmentID > id) {
                    id = appointmentID;
                }
            }
            id++;
            Appointment appointment;
            if (type == 1)
            {
                appointment = new Examination(id, dateTime, this, patient, false, 0, 0);
            }
            else {
                appointment = new Operation(id, dateTime, this, patient, false, 0, 0, duration);
            };

            InsertAppointment(appointment);
            patient.InsertAppointment(appointment);
            SystemFunctions.AllAppointments.Add(id, appointment);
            SystemFunctions.CurrentAppointments.Add(id, appointment);
            Console.WriteLine("\nNew appointment successfully created\n");

        }
        //=======================================================================================================================================================================
        // READ

        private void ViewAppointments()
        {

            if (this.Appointments.Count == 0)
            {
                Console.WriteLine("\nNo future appointments\n");
                return;
            }

            int i = 1;
            DateTime now = DateTime.Now;
            string type;
            foreach (Appointment appointment in this.Appointments)
            {
                if (appointment.Type == 'e') type = "Examination";
                else type = "Operation";
                Console.WriteLine($"\n\n{i}. {type}\n\nId: {appointment.ID};\nTime and Date: {appointment.DateTime};\nDuration: {appointment.Duration};\nPatient last name: {appointment.Patient.LastName}; Patient name: {appointment.Patient.Name}\n");

                i++;
            }
            
        }
        //=======================================================================================================================================================================
        // UPDATE
        private void EditAppointment()
        {
            bool quit = false;
            Appointment appointment = null;
            while (appointment == null)
            {
                Console.WriteLine("Enter the ID of the appointment you wish to edit?");
                int id = OtherFunctions.EnterNumber();
                foreach (Appointment tmp in this.Appointments)
                {
                    if (tmp.ID == id)
                    {
                        appointment = tmp;
                        break;
                    }
                }
                if (appointment == null)
                {
                    Console.WriteLine($"No appointment matches ID: {id}");
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
                        newDate += appointment.DateTime.TimeOfDay;
                        if (CheckAppointment(newDate, appointment.Duration)) {
                            
                            this.Appointments.Remove(appointment);
                            appointment.Patient.Appointments.Remove(appointment);
                            var last = SystemFunctions.AllAppointments.Values.Last();
                            Examination editedExamination = new Examination(last.ID + 1, newDate, this, appointment.Patient, appointment.Finished, 0, appointment.ID); ;
                            SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                            SystemFunctions.CurrentAppointments.Remove(appointment.ID);
                            SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                            this.Appointments.Add(editedExamination);
                            editedExamination.Patient.Appointments.Add(editedExamination);
                        }
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
                    newTime = appointment.DateTime.Date + newTime.TimeOfDay;
                    if (newTime < DateTime.Now)
                    {
                        Console.WriteLine("You can't enter that time, its in the past");
                    }
                    else
                    {
                        if (CheckAppointment(newTime, appointment.Duration)) { 
                            this.Appointments.Remove(appointment);
                            var last = SystemFunctions.AllAppointments.Values.Last();
                            appointment.Patient.Appointments.Remove(appointment);
                            Examination editedExamination = new Examination(last.ID + 1, newTime, this, appointment.Patient, appointment.Finished, 0, appointment.ID);
                            SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                            SystemFunctions.AllAppointments.Remove(appointment.ID);
                            SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                            this.Appointments.Add(editedExamination);
                            editedExamination.Patient.Appointments.Add(editedExamination);
                        }
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
            Console.WriteLine("Enter the ID of the appointment you wish to delete.");
            int id = OtherFunctions.EnterNumber();
            Appointment appointment = null;
            foreach (Appointment tmp in this.Appointments)
            {
                if (tmp.ID == id)
                {
                    appointment = tmp;
                    Console.WriteLine("Are you sure? (y/n)");
                    string choice = Console.ReadLine();
                    if (choice.ToUpper() == "Y") {
                        this.Appointments.Remove(appointment);
                        appointment.Patient.Appointments.Remove(appointment);
                        var last = SystemFunctions.AllAppointments.Values.Last();
                        Examination deletedExamination = new Examination(last.ID + 1, appointment.DateTime, this, appointment.Patient, appointment.Finished, appointment.ID, appointment.Edited);
                        SystemFunctions.AllAppointments.Add(deletedExamination.ID, deletedExamination);
                        SystemFunctions.CurrentAppointments.Remove(appointment.ID);
                    }
                    break;
                }
            }

        }

        //=======================================================================================================================================================================
        // VIEW SCHEDULE
        private void ViewSchedule()
        {
            Console.WriteLine("Enter a date for which you wish to see your schedule (e.g. 22/10/1987): ");
            DateTime date = OtherFunctions.GetGoodDate();
            Console.WriteLine($"Appointments on date: {date.ToShortDateString()} and the next three days: \n");

            foreach (Appointment appointment in this.Appointments)
            {
                if (date.Date <= appointment.DateTime.Date && appointment.DateTime.Date <= date.Date.AddDays(3))
                {
                    appointment.View();
                    Console.WriteLine();

                }

            }
            string choice = "Y";
            while (choice.ToUpper() == "Y")
            {
                Console.Write("Do you wish to view additional detail for any appointment?(y/n)\n>> ");
                choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    Console.Write("\n\nEnter the ID of the appointment you wish to view\n>> ");
                    int id = OtherFunctions.EnterNumber();
                    Appointment chosenAppointment;
                    if (!SystemFunctions.CurrentAppointments.TryGetValue(id, out chosenAppointment))
                    {
                        Console.WriteLine("No appointment with that ID found");
                        return;
                    }
                    Console.WriteLine("Searching for medical record");
                    HealthRecord healthRecord;
                    if (!SystemFunctions.HealthRecords.TryGetValue(chosenAppointment.Patient.UserName, out healthRecord))
                    {
                        Console.WriteLine("No health record found, creating a new record");
                        healthRecord = new HealthRecord(chosenAppointment.Patient);
                        SystemFunctions.HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
                    }
                    Console.WriteLine("Information about patient:");
                    healthRecord.Patient.ViewPatient();
                    healthRecord.ShowHealthRecord();
                }
            }
            Console.WriteLine("Do you wish to perform an examination/operation(y/n)?");
            choice = Console.ReadLine();
            if (choice.ToUpper() == "Y")
            {
                Console.Write("\n\nEnter the ID of the appointment you wish to perform\n>> ");
                int id = OtherFunctions.EnterNumber();

                Appointment chosenAppointment;
                if (!SystemFunctions.CurrentAppointments.TryGetValue(id, out chosenAppointment))
                {
                    Console.WriteLine("No appointment with that ID found");
                    return;
                }
                Perform(chosenAppointment);

            }
        

    }

        //=======================================================================================================================================================================
        // PERFORM EXAMINATION
        private void Perform(Appointment appointment)
        {
            string type;
            if (appointment.Type == 'e') type = "Examination";
            else type = "Operation";
            Console.WriteLine($"{type} starting. Searching for medical record");
            HealthRecord healthRecord;
            if (!SystemFunctions.HealthRecords.TryGetValue(appointment.Patient.UserName, out healthRecord))
            {
                Console.WriteLine("No health record found, creating a new record");
                healthRecord = new HealthRecord(appointment.Patient);
                SystemFunctions.HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
            }
            healthRecord.ShowHealthRecord();
            Console.WriteLine("\nWrite you Anamnesis: ");
            string anamnesisText = Console.ReadLine();
            Anamnesis anamnesis = new Anamnesis(anamnesisText, this);
            healthRecord.Anamneses.Add(anamnesis);
            Console.WriteLine("Anamnesis added\nDo you want to change medical record? (y/n)");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                ChangePatientRecord(ref healthRecord);
            }
            if (!SystemFunctions.HealthRecords.TryAdd(appointment.Patient.UserName, healthRecord))
            {
                SystemFunctions.HealthRecords[appointment.Patient.UserName] = healthRecord;
            }
            Console.WriteLine("Create referral for patient? (y/n)");
            choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                Patient patient = healthRecord.Patient;
                CreateReferral(ref patient);
            }

            do{ Console.WriteLine("Write prescription for patient? (y/n)");
                choice = Console.ReadLine().ToUpper(); 
                if(choice.ToUpper() == "Y")
                {
                    Prescription prescription = WritePrecription(healthRecord.Patient);
                    if (prescription != null) {
                        healthRecord.Patient.Prescriptions.Add(prescription);
                        using (StreamWriter sw = File.AppendText(SystemFunctions.PrescriptionsFilePath))
                        {
                            sw.WriteLine(prescription.Compress());
                        }
                    }
                    
                }
            }while (choice.ToUpper() == "Y") ;
            


            appointment.Finished = true;
            SystemFunctions.CurrentAppointments.Remove(appointment.ID);
            this.Appointments.Remove(appointment);

            appointment.Patient.Appointments.Remove(appointment);
            Console.WriteLine("Examination ended");
        }


        //=======================================================================================================================================================================
        // CHANGE PATIENT HEALTH RECORD


        private void ChangePatientRecord( ref HealthRecord healthRecord) { 
        Console.Write("Change weight?(y/n): ");
            string choice;
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

        //=======================================================================================================================================================================
        // CREATE REFERRAL FOR PATIENT

        void CreateReferral(ref Patient patient) {
            Console.WriteLine("Create referral for (1) specific doctor or (2) specific field");
            int i = OtherFunctions.EnterNumberWithLimit(0, 3);
            if (i == 1)
            {
                Patient.ViewAllDoctors();
                Console.WriteLine("\n Enter doctor username: ");
                Console.Write(">> ");
                string userName = Console.ReadLine();
                Doctor doctor = null;
                if (!SystemFunctions.Doctors.TryGetValue(userName, out doctor))
                {
                    Console.WriteLine("Doctor with that username does not exist.");
                    return;
                }
                Referral referral = new Referral(this, patient, doctor, doctor.Field);
                patient.Referrals.Add(referral);
                SystemFunctions.Referrals.Add(referral);
            }
            else {
                Fields field = OtherFunctions.AskField();
                Referral referral = new Referral(this, patient, null, field);
                patient.Referrals.Add(referral);
                SystemFunctions.Referrals.Add(referral);

            }
            Console.WriteLine("Referral created successfully!");
            
        }
        //=======================================================================================================================================================================
        // WRITE UP A PRESCRIPTION FOR A PATIENT

        private Prescription WritePrecription(Patient patient) {
            Console.Write("Insert the name of Medicine: ");
            string medicineName = Console.ReadLine();
            Console.WriteLine();
            Medicine medicine;
            if (!SystemFunctions.Medicine.TryGetValue(medicineName, out medicine))
            {
                Console.WriteLine("Medicine with that name does not exist.");
                return null;
            }
            bool alergic = CheckAlergy(medicine, patient.UserName);
            if (alergic) return null;
            int[] frequency = {0, 0, 0};

            Console.WriteLine("Enter the number of pills to take in 1) the morning 2) at noon 3) the afternoon:");
            for(int i = 0; i < 3; i++)
            {
                Console.Write((i+1) + ") >> ");
                frequency[i] = OtherFunctions.EnterNumber();
                Console.WriteLine();
            }
            Console.WriteLine("Should the patient take the medicine: \n(1) Before a meal\n(2) After a meal\n(3) During a meal\n(4) Doesn't matter\n\nChose by number");
            int medicineMealInfo = OtherFunctions.EnterNumberWithLimit(0, 5);
            MedicineFoodIntake medicineFoodIntake = (MedicineFoodIntake)(medicineMealInfo - 1);
            Prescription prescription = new Prescription(patient, this, DateTime.Now, medicine, frequency, medicineFoodIntake);
            prescription.ShowPrescription();
            Console.Write("Prescription created\n");
            return prescription;

        }





        //=======================================================================================================================================================================
        // OTHER FUNCTIONALITIES
        public bool CheckAppointment(DateTime dateTime, int duration)
        {
            foreach (Appointment appointment in this.Appointments)
            {
                if (appointment.DateTime.Date == dateTime.Date)
                {
                    if ((appointment.DateTime <= dateTime && appointment.DateTime.AddMinutes(duration) > dateTime) || (dateTime <= appointment.DateTime && dateTime.AddMinutes(duration) > appointment.DateTime))
                    {
                        return false;
                    }
                }
                if (appointment.DateTime.Date > dateTime.Date)
                {
                    break;
                }

            }
            return true;
        }

        public void InsertAppointment(Appointment newAppointment)
        {
            if (this.Appointments.Count() == 0)
            {
                this.Appointments.Add(newAppointment);
                return;
            }
            for (int i = 0; i < this.Appointments.Count(); i++)
            {
                if (this.Appointments[i].DateTime < newAppointment.DateTime)
                {
                    Appointments.Insert(i, newAppointment);
                    return;
                }
            }
            this.Appointments.Add(newAppointment);

        }

        private bool CheckAlergy(Medicine medicine, string userName)
        {
            HealthRecord healthRecord = SystemFunctions.HealthRecords[userName];
            foreach(string alergy in healthRecord.Alergies) {
                foreach(string alergen in medicine.Ingredients)
                {
                    if (alergen.ToUpper() == alergy.ToUpper()) {
                        Console.WriteLine($"Error: Patient alergic to medicine. Alergen: {alergen}");
                        return true;
                    }
                }
            }
            return false;
        }

    }


}

