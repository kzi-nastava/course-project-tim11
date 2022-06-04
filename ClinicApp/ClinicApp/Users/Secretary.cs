using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClinicApp.Users
{
    public class Secretary : User
    {
        public Secretary(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Secretary;
            MessageBox = new MessageBox(this);
        }

        public Secretary(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Secretary;
            MessageBox = new MessageBox(this);
        }

        //Compresses a Secretary object intu a string for easier upload.
        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        //Writes all the option a secretary has once he logs in.
        public override int MenuWrite()
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage patient accounts");
            Console.WriteLine("4: Block or unbolck patient accounts");
            Console.WriteLine("5: Manage examination requests");
            Console.WriteLine("6: Create examinations based upon referrals.");
            Console.WriteLine("7: Create an emergency examination.");
            Console.WriteLine("0: Exit");

            return 7;
        }

        //Executes the chosen command.
        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    MessageBox.DisplayMessages();
                    break;
                case 3:
                    PatientsCRUD();
                    break;
                case 4:
                    ManageBlockedPatients();
                    break;
                case 5:
                    ManageExaminationRequests();
                    break;
                case 6:
                    CreateExaminationsFromReferrals();
                    break;
                case 7:
                    CreateEmergencyExamination();
                    break;
            }
        }

        //Manages the Patient CRUD.
        private static void PatientsCRUD()
        {
            int option = 1, option2, numberOfOptions = 4;
            User tempUser;
            while (option != 0)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1: Create a patient account");
                Console.WriteLine("2: View all patient accounts");
                Console.WriteLine("3: Update a patient account");
                Console.WriteLine("4: Delete a patient account");
                Console.WriteLine("0: Back to menue");
                Console.Write(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                Console.WriteLine();
                switch(option)
                {
                    //Create
                    case 1:
                        User patient = OtherFunctions.Register(Roles.Patient);
                        SystemFunctions.Users.Add(patient.UserName, patient);
                        SystemFunctions.Patients.Add(patient.UserName, (Patient)patient);
                        break;
                    //Read
                    case 2:
                        OtherFunctions.PrintUsers(role : Roles.Patient);
                        break;
                    //Update
                    case 3:
                        option2 = 1;
                        while (option2 != 0)
                        {
                            Console.WriteLine("\nWrite the username of the patient who's account you want deleted:");
                            string userName = OtherFunctions.EnterString();
                            if (SystemFunctions.Users.TryGetValue(userName, out tempUser))
                            {
                                if (tempUser.Role == Roles.Patient)
                                {
                                    UpdatePatient((Patient)tempUser);
                                }
                                else
                                {
                                    Console.WriteLine("\nThis account doesn't belong to a patient. Want to try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThere is no account with this username. Want to try again?");
                                Console.WriteLine("1: Yes");
                                Console.WriteLine("0: No");
                                option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                            }
                        }
                        break;
                    //Delete
                    case 4:
                        option2 = 1;
                        while(option2 != 0)
                        {
                            Console.WriteLine("\nWrite the username of the patient who's account you want deleted:");
                            string userName = OtherFunctions.EnterString();
                            if (SystemFunctions.Users.TryGetValue(userName, out tempUser))
                            {
                                if (tempUser.Role == Roles.Patient)
                                {
                                    SystemFunctions.Users.Remove(userName);
                                    SystemFunctions.Patients.Remove(userName);
                                    option2 = 0;
                                }
                                else
                                {
                                    Console.WriteLine("\nThis account doesn't belong to a patient. Want to try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThere is no account with this username. Want to try again?");
                                Console.WriteLine("1: Yes");
                                Console.WriteLine("0: No");
                                option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                            }
                        }
                        break;
                }
            }
        }

        //Puts the U in CRUD.
        private static void UpdatePatient(Patient patient)
        {
            int option = 1;
            string temp;

            while(option != 0)
            {
                patient.Print();
                Console.WriteLine("\nWhat would you like to change?");
                Console.WriteLine("1: Username");
                Console.WriteLine("2: Password");
                Console.WriteLine("3: Name");
                Console.WriteLine("4: Last name");
                Console.WriteLine("5: Gender");
                Console.WriteLine("6: Date of birth");
                Console.WriteLine("0: Back to menu");
                option = OtherFunctions.EnterNumberWithLimit(0, 6);

                switch(option)
                {
                    //Username
                    case 1:
                        Console.Write("Username: ");
                        temp = OtherFunctions.EnterString();
                        while (SystemFunctions.Users.ContainsKey(temp))
                        {
                            Console.WriteLine("This username is taken. Please, try again.");
                            Console.Write("Username: ");
                            temp = OtherFunctions.EnterString();
                        }
                        patient.UserName = temp;
                        break;
                    //Password
                    case 2:
                        string password, passwordCheck;
                        Console.Write("Password: ");
                        password = OtherFunctions.MaskPassword();
                        Console.Write("\nRepeat password: ");
                        passwordCheck = OtherFunctions.MaskPassword();
                        while (password != passwordCheck)
                        {
                            Console.WriteLine("Passwords don't match. Please, try again.");
                            Console.Write("Password: ");
                            password = OtherFunctions.MaskPassword();
                            Console.Write("\nRepeat password: ");
                            passwordCheck = OtherFunctions.MaskPassword();
                        }
                        patient.Password = password;
                        break;
                    //Name
                    case 3:
                        Console.Write("\nName: ");
                        patient.Name = OtherFunctions.EnterString();
                        break;
                    //Last name
                    case 4:
                        Console.Write("\nLast name: ");
                        patient.LastName = OtherFunctions.EnterString();
                        break;
                    //Gender
                    case 5:
                        Console.Write("Gender (m/f/n): ");
                        temp = OtherFunctions.EnterString();
                        while (temp != "m" && temp != "f" && temp != "n")
                        {
                            Console.Write("You didn't enter a valid option. Please, try again (m/f/n): ");
                            temp = OtherFunctions.EnterString();
                        }
                        patient.Gender = temp[0];
                        break;
                    //Date of birth
                    case 6:
                        Console.Write("Date of birth (e.g. 02/05/1984): ");
                        patient.DateOfBirth = OtherFunctions.AskForDate();
                        break;
                }
            }
        }

        //Manages blocked and unblocked patients.
        private static void ManageBlockedPatients()
        {
            int option = 1, numberOfOptions = 4;
            string username;
            Patient patient;
            while (option != 0)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1: List patient accounts");
                Console.WriteLine("2: Block patient accounts");
                Console.WriteLine("3: Unblock patient accounts");
                Console.WriteLine("0: Back to menue");
                Console.Write(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                Console.WriteLine();
                switch (option)
                {
                    //List all
                    case 1:
                        Console.WriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        Console.WriteLine(OtherFunctions.TableHeader() + " Blocked by      |");
                        Console.WriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        foreach (KeyValuePair<string, Patient> pair in SystemFunctions.Patients)
                        {
                            Console.WriteLine(pair.Value.TextInTable() + " " + pair.Value.Blocked.ToString() + OtherFunctions.Space(15, pair.Value.Blocked.ToString()) + " |");
                            Console.WriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        }
                        Console.WriteLine();
                        break;
                    //Block
                    case 2:
                        Console.WriteLine("\nEnter the username of the account you want to block:");
                        username = OtherFunctions.EnterString();
                        if(SystemFunctions.Patients.TryGetValue(username, out patient))
                            if(patient.Blocked == Blocked.Unblocked)
                                patient.Blocked = Blocked.Secretary;
                            else
                                Console.WriteLine("This patient's account is already blocked.");
                        else
                            Console.WriteLine("There is no patient's account with such username.");
                        break;
                    //Unblock
                    case 3:
                        Console.WriteLine("\nEnter the username of the account you want to unblock:");
                        username = OtherFunctions.EnterString();
                        if(SystemFunctions.Patients.TryGetValue(username, out patient))
                            if(patient.Blocked != Blocked.Unblocked)
                                patient.Blocked = Blocked.Unblocked;
                            else
                                Console.WriteLine("This patient's account is already unblocked.");
                        else
                            Console.WriteLine("There is no patient's account with such username.");
                        break;
                }
            }
        }

        //Manages patient requests.
        private static void ManageExaminationRequests()
        {
            int id, option;
            Clinic.Appointment appointment;

            Console.WriteLine();
            using (StreamReader reader = new StreamReader(SystemFunctions.PatientRequestsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    Console.WriteLine("1: Approve");
                    Console.WriteLine("2: Deny");
                    option = OtherFunctions.EnterNumberWithLimit(1, 2);
                    if(option == 1)
                    {
                        if (!Int32.TryParse(line.Split("|")[0], out id))
                            id = -1;
                        if (line.Split("|")[1] == "DELETE")
                        {
                            appointment = SystemFunctions.AllAppointments[id];
                            appointment.Doctor.Appointments.Remove(appointment);
                            appointment.Patient.Appointments.Remove(appointment);
                            var last = SystemFunctions.AllAppointments.Values.Last();
                            Clinic.Examination deletedExamination = new Clinic.Examination(last.ID + 1, appointment.DateTime, appointment.Doctor, appointment.Patient, appointment.Finished, appointment.ID, appointment.Edited);
                            SystemFunctions.AllAppointments.Add(deletedExamination.ID, deletedExamination);
                            SystemFunctions.CurrentAppointments.Remove(appointment.ID);
                        }
                        else if (line.Split("|")[1] == "UPDATE" && SystemFunctions.AllAppointments.TryGetValue(id, out appointment))
                        {

                            appointment.DateTime = DateTime.Parse(line.Split("|")[2]);
                            Doctor doctor;
                            if (SystemFunctions.Doctors.TryGetValue(line.Split("|")[3], out doctor))
                            {
                                appointment.Doctor.Appointments.Remove(appointment);
                                appointment.Patient.Appointments.Remove(appointment);
                                var last = SystemFunctions.AllAppointments.Values.Last();
                                Clinic.Examination editedExamination = new Clinic.Examination(last.ID + 1, appointment.DateTime, doctor, appointment.Patient, appointment.Finished, 0, appointment.ID);
                                SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                                SystemFunctions.CurrentAppointments.Remove(appointment.ID);
                                SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                                appointment.Patient.InsertExamination(editedExamination);
                                editedExamination.Doctor.InsertAppointment(editedExamination);
                            }
                        }
                    }
                }
            }
            Console.WriteLine();
        }

        //Creates examinations based upon referrals.
        private static void CreateExaminationsFromReferrals()
        {
            int option = 1;
            string userName;
            Patient patient;
            while(option != 0)
            {
                Console.WriteLine("\nEnter the patients username:");
                userName = OtherFunctions.EnterString();
                if (SystemFunctions.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.
                    if(patient.Referrals.Count() == 0)
                    {
                        Console.WriteLine("\nThis patient has no referrals.");
                        return;
                    }
                    else
                    {
                        //Finds the doctor.
                        Clinic.Referral referral = patient.Referrals[0];
                        Doctor doctor = referral.DoctorSpecialist;

                        //Finds the id.
                        int id = 0;
                        foreach (int examinationID in SystemFunctions.AllAppointments.Keys)
                        {
                            if (examinationID > id)
                            {
                                id = examinationID;
                            }
                        }
                        id++;

                        //Finds the date and time for the examination.
                        bool hasTime = false;
                        int option2 = 1;
                        DateTime dateTime = DateTime.Now, date, time;
                        while(hasTime == false && option2 != 0)
                        {
                            Console.Write("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");
                            date = OtherFunctions.GetGoodDate();

                            Console.Write("\nEnter the time of your Examination (e.g. 14:30)\n>> ");
                            time = OtherFunctions.AskForTime();

                            dateTime = date.Date + time.TimeOfDay;
                            if (dateTime < DateTime.Now)
                            {
                                Console.WriteLine("You can't enter that time, it's in the past");
                            }
                            else
                            {
                                hasTime = true;
                                if (!doctor.CheckAppointment(dateTime, 15))
                                {
                                    hasTime = false;
                                    Console.WriteLine("The doctor is not availible at that time. Try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                                else if (!patient.CheckAppointment(dateTime, 15))
                                {
                                    hasTime = false;
                                    Console.WriteLine("The patient is not availible at that time. Try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                                else
                                {
                                    //Creates the examination.
                                    Clinic.Examination examination = new Clinic.Examination(id, dateTime, doctor, patient, false, 0, 0);
                                    doctor.InsertAppointment(examination);
                                    patient.InsertExamination(examination);
                                    SystemFunctions.AllAppointments.Add(id, examination);
                                    SystemFunctions.CurrentAppointments.Add(id, examination);
                                    patient.Referrals.RemoveAt(0);
                                    Console.WriteLine("\nNew examination successfully created\n");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nThere is no such patient. Try again?");
                    Console.WriteLine("1: Yes");
                    Console.WriteLine("0: No");
                    option = OtherFunctions.EnterNumberWithLimit(0, 1);
                }
            }
        }

        //Creates emergency examinations.
        private static void CreateEmergencyExamination()
        {
            string userName;
            Patient patient;
            int option = 1, id;
            while (option != 0)
            {
                //Finding the patient.
                Console.WriteLine("\nEnter the patients username:");
                userName = OtherFunctions.EnterString();
                if (SystemFunctions.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.

                    //Finding the doctors field of expertise.
                    int option2, numberOfOptions = 0;
                    Console.WriteLine("Which specialty does the doctor need to possess?");
                    List<Fields> fields = new List<Fields>();
                    foreach(Fields field in (Fields[])Enum.GetValues(typeof(Fields)))
                    {
                        numberOfOptions++;
                        Console.WriteLine(numberOfOptions + ": " + field);
                        fields.Add(field);
                    }
                    option2 = OtherFunctions.EnterNumberWithLimit(1, numberOfOptions);
                    Fields fieldOfDoctor = fields[option2 - 1]; //-1 because it starts from zero and options start from 1.

                    //Checking if there's a doctor with that specialty.
                    bool hasSpecialty = false;
                    foreach(KeyValuePair<string, Doctor> pair in SystemFunctions.Doctors)
                    {
                        if(pair.Value.Field == fieldOfDoctor)
                            hasSpecialty = true;
                    }
                    if(hasSpecialty == false)
                    {
                        Console.WriteLine("\nNo doctor has that specialty.");
                        return;
                    }

                    //Finding the available time for examination.
                    bool hasFoundTime = false;
                    DateTime dateTime = DateTime.Now.AddMinutes(5);
                    while(dateTime <= DateTime.Now.AddMinutes(120) && hasFoundTime == false)
                    {
                        DateRange dateRange = new DateRange(dateTime, dateTime.AddMinutes(15));
                        if(patient.CheckAppointment(dateTime, 15))
                        {
                            foreach(KeyValuePair<string, Doctor> doctor in SystemFunctions.Doctors)
                            {
                                if (doctor.Value.Field == fieldOfDoctor && !OtherFunctions.CheckForRenovations(dateRange, doctor.Value.RoomId) &&
                                    !OtherFunctions.CheckForExaminations(dateRange, doctor.Value.RoomId) &&
                                    doctor.Value.CheckAppointment(dateTime, 15))
                                {
                                    hasFoundTime = true;
                                    //Finds the id.
                                    id = 0;
                                    foreach (int examinationID in SystemFunctions.AllAppointments.Keys)
                                    {
                                        if (examinationID > id)
                                        {
                                            id = examinationID;
                                        }
                                    }
                                    id++;
                                    Clinic.Examination examination = new Clinic.Examination(id, dateTime, doctor.Value, patient, false, 0, 0);
                                    doctor.Value.InsertAppointment(examination);
                                    patient.InsertExamination(examination);
                                    SystemFunctions.AllAppointments.Add(id, examination);
                                    SystemFunctions.CurrentAppointments.Add(id, examination);
                                    doctor.Value.MessageBox.AddMessage("You have an emergency examination.");
                                    patient.MessageBox.AddMessage("You have an emergency examination.");
                                    Console.WriteLine("The examination has been created successfully.");
                                    return;
                                }
                            }
                        }
                        dateTime = dateTime.AddMinutes(1);
                    }

                    //If there is no time available, we search for another examiantion to delay.

                    //First, we make a list of all the examinations that can be delayed and by how much can they be delayed.
                    List<KeyValuePair<Clinic.Examination, DateTime>> examinationsForDelaying = new List<KeyValuePair<Clinic.Examination, DateTime>>();
                    foreach (KeyValuePair<int, Clinic.Appointment> examinationForDelay in SystemFunctions.AllAppointments)
                        if (examinationForDelay.Value.DateTime > DateTime.Now && examinationForDelay.Value.DateTime < DateTime.Now.AddMinutes(120) && examinationForDelay.Value.Doctor.Field == fieldOfDoctor && (examinationForDelay.Value.Patient == patient || patient.CheckAppointment(examinationForDelay.Value.DateTime, 15)))
                        {
                            examinationsForDelaying.Add(new KeyValuePair<Clinic.Examination, DateTime>((Clinic.Examination)examinationForDelay.Value, examinationForDelay.Value.NextAvailable()));
                            Console.WriteLine(examinationForDelay.Key);
                        }
                    if(examinationsForDelaying.Count == 0)
                    {
                        Console.WriteLine("\nThere are no examinations available for delaying.");
                        return;
                    }

                    //Then, we make a list of 5 options.
                    numberOfOptions = 0;
                    List<KeyValuePair<Clinic.Examination, DateTime>> examinationsForDelayingOptions = new List<KeyValuePair<Clinic.Examination, DateTime>>();
                    while(numberOfOptions < 5 && examinationsForDelaying.Count() > 0)
                    {
                        KeyValuePair<Clinic.Examination, DateTime> temp = examinationsForDelaying[0];
                        foreach (KeyValuePair<Clinic.Examination, DateTime> pair in examinationsForDelaying)
                            if (pair.Value < temp.Value)
                                temp = pair;
                        numberOfOptions++;
                        examinationsForDelayingOptions.Add(temp);
                        examinationsForDelaying.Remove(temp);
                    }

                    //The secretary chooses which one will be delayed.
                    Console.WriteLine("\nWhich examination should we delay?");
                    for(int i = 0; i < numberOfOptions; i++)
                    {
                        Console.WriteLine((i + 1) + ": " + examinationsForDelayingOptions[i].Key.ID + " will be delayed from " + examinationsForDelayingOptions[i].Key.DateTime + " to " + examinationsForDelayingOptions[i].Value);
                    }
                    Console.WriteLine("0: Back to menu");
                    if (numberOfOptions < 5)
                        Console.WriteLine("There are no more examinations left that can be delayed.");

                    //Finally, we create the examination and delay the other one.
                    option2 = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                    if (option2 == 0)
                        return;
                    KeyValuePair<Clinic.Examination, DateTime> examinationForDelaying = examinationsForDelayingOptions[option2 - 1]; //-1 because it starts from zero and options start from 1.
                    //Finds the id.
                    id = 0;
                    foreach (int examinationID in SystemFunctions.AllAppointments.Keys)
                    {
                        if (examinationID > id)
                        {
                            id = examinationID;
                        }
                    }
                    id++;

                    //Creates the examination.
                    Clinic.Examination examination2 = new Clinic.Examination(id, examinationForDelaying.Key.DateTime, examinationForDelaying.Key.Doctor, patient, false, 0, 0);
                    examinationForDelaying.Key.Doctor.InsertAppointment(examination2);
                    patient.InsertExamination(examination2);
                    SystemFunctions.AllAppointments.Add(id, examination2);
                    SystemFunctions.CurrentAppointments.Add(id, examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("You have an emergency examination.");
                    patient.MessageBox.AddMessage("You have an emergency examination.");
                    Console.WriteLine("The examination has been created successfully.");

                    //Delays the other examination.
                    examinationForDelaying.Key.Doctor.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.Patient.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.DateTime = examinationForDelaying.Value;
                    examinationForDelaying.Key.Edited++;
                    examinationForDelaying.Key.Doctor.InsertAppointment(examination2);
                    examinationForDelaying.Key.Patient.InsertExamination(examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("Your examination has been delayed.");
                    examinationForDelaying.Key.Patient.MessageBox.AddMessage("Your examination has been delayed.");
                    Console.WriteLine("The other examination has been delayed successfully.");
                }
                else
                {
                    Console.WriteLine("\nThere is no such patient. Try again?");
                    Console.WriteLine("1: Yes");
                    Console.WriteLine("0: No");
                    option = OtherFunctions.EnterNumberWithLimit(0, 1);
                }
            }
        }
    }
}
