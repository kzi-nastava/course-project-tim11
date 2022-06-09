using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClinicApp.AdminFunctions;

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
            CLI.CLIWriteLine("\nWhat would you like to do?");
            CLI.CLIWriteLine("1: Log out");
            CLI.CLIWriteLine("2: Display new messages (" + MessageBox.NumberOfMessages + ")");
            CLI.CLIWriteLine("3: Manage patient accounts");
            CLI.CLIWriteLine("4: Block or unbolck patient accounts");
            CLI.CLIWriteLine("5: Manage examination requests");
            CLI.CLIWriteLine("6: Create examinations based upon referrals");
            CLI.CLIWriteLine("7: Create an emergency examination");
            CLI.CLIWriteLine("8: Make an order of dynamic equipment");
            CLI.CLIWriteLine("9: Redistribute dynamic equipment");
            CLI.CLIWriteLine("0: Exit");

            return 9;
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
                case 8:
                    OrderDynamiicEquipment();
                    break;
                case 9:
                    RedistributeDynamiicEquipment();
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
                CLI.CLIWriteLine("\nWhat would you like to do?");
                CLI.CLIWriteLine("1: Create a patient account");
                CLI.CLIWriteLine("2: View all patient accounts");
                CLI.CLIWriteLine("3: Update a patient account");
                CLI.CLIWriteLine("4: Delete a patient account");
                CLI.CLIWriteLine("0: Back to menue");
                CLI.CLIWrite(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                CLI.CLIWriteLine();
                switch(option)
                {
                    //Create
                    case 1:
                        User patient = OtherFunctions.Register(Roles.Patient);
                        UserRepository.Users.Add(patient.UserName, patient);
                        UserRepository.Patients.Add(patient.UserName, (Patient)patient);
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
                            CLI.CLIWriteLine("\nWrite the username of the patient who's account you want updated:");
                            string userName = OtherFunctions.EnterString();
                            if (UserRepository.Users.TryGetValue(userName, out tempUser))
                            {
                                if (tempUser.Role == Roles.Patient)
                                {
                                    UpdatePatient((Patient)tempUser);
                                }
                                else
                                {
                                    CLI.CLIWriteLine("\nThis account doesn't belong to a patient. Want to try again?");
                                    CLI.CLIWriteLine("1: Yes");
                                    CLI.CLIWriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                            }
                            else
                            {
                                CLI.CLIWriteLine("\nThere is no account with this username. Want to try again?");
                                CLI.CLIWriteLine("1: Yes");
                                CLI.CLIWriteLine("0: No");
                                option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                            }
                        }
                        break;
                    //Delete
                    case 4:
                        option2 = 1;
                        while(option2 != 0)
                        {
                            CLI.CLIWriteLine("\nWrite the username of the patient who's account you want deleted:");
                            string userName = OtherFunctions.EnterString();
                            if (UserRepository.Users.TryGetValue(userName, out tempUser))
                            {
                                if (tempUser.Role == Roles.Patient)
                                {
                                    UserRepository.Users.Remove(userName);
                                    UserRepository.Patients.Remove(userName);
                                    option2 = 0;
                                }
                                else
                                {
                                    CLI.CLIWriteLine("\nThis account doesn't belong to a patient. Want to try again?");
                                    CLI.CLIWriteLine("1: Yes");
                                    CLI.CLIWriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                            }
                            else
                            {
                                CLI.CLIWriteLine("\nThere is no account with this username. Want to try again?");
                                CLI.CLIWriteLine("1: Yes");
                                CLI.CLIWriteLine("0: No");
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
                CLI.CLIWriteLine("\nWhat would you like to change?");
                CLI.CLIWriteLine("1: Username");
                CLI.CLIWriteLine("2: Password");
                CLI.CLIWriteLine("3: Name");
                CLI.CLIWriteLine("4: Last name");
                CLI.CLIWriteLine("5: Gender");
                CLI.CLIWriteLine("6: Date of birth");
                CLI.CLIWriteLine("0: Back to menu");
                option = OtherFunctions.EnterNumberWithLimit(0, 6);

                switch(option)
                {
                    //Username
                    case 1:
                        CLI.CLIWrite("Username: ");
                        temp = OtherFunctions.EnterString();
                        while (UserRepository.Users.ContainsKey(temp))
                        {
                            CLI.CLIWriteLine("This username is taken. Please, try again.");
                            CLI.CLIWrite("Username: ");
                            temp = OtherFunctions.EnterString();
                        }
                        patient.UserName = temp;
                        break;
                    //Password
                    case 2:
                        string password, passwordCheck;
                        CLI.CLIWrite("Password: ");
                        password = OtherFunctions.MaskPassword();
                        CLI.CLIWrite("\nRepeat password: ");
                        passwordCheck = OtherFunctions.MaskPassword();
                        while (password != passwordCheck)
                        {
                            CLI.CLIWriteLine("Passwords don't match. Please, try again.");
                            CLI.CLIWrite("Password: ");
                            password = OtherFunctions.MaskPassword();
                            CLI.CLIWrite("\nRepeat password: ");
                            passwordCheck = OtherFunctions.MaskPassword();
                        }
                        patient.Password = password;
                        break;
                    //Name
                    case 3:
                        CLI.CLIWrite("\nName: ");
                        patient.Name = OtherFunctions.EnterString();
                        break;
                    //Last name
                    case 4:
                        CLI.CLIWrite("\nLast name: ");
                        patient.LastName = OtherFunctions.EnterString();
                        break;
                    //Gender
                    case 5:
                        CLI.CLIWrite("Gender (m/f/n): ");
                        temp = OtherFunctions.EnterString();
                        while (temp != "m" && temp != "f" && temp != "n")
                        {
                            CLI.CLIWrite("You didn't enter a valid option. Please, try again (m/f/n): ");
                            temp = OtherFunctions.EnterString();
                        }
                        patient.Gender = temp[0];
                        break;
                    //Date of birth
                    case 6:
                        CLI.CLIWrite("Date of birth (e.g. 02/05/1984): ");
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
                CLI.CLIWriteLine("\nWhat would you like to do?");
                CLI.CLIWriteLine("1: List patient accounts");
                CLI.CLIWriteLine("2: Block patient accounts");
                CLI.CLIWriteLine("3: Unblock patient accounts");
                CLI.CLIWriteLine("0: Back to menue");
                CLI.CLIWrite(">> ");
                option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                CLI.CLIWriteLine();
                switch (option)
                {
                    //List all
                    case 1:
                        CLI.CLIWriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        CLI.CLIWriteLine(OtherFunctions.TableHeader() + " Blocked by      |");
                        CLI.CLIWriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        foreach (KeyValuePair<string, Patient> pair in UserRepository.Patients)
                        {
                            CLI.CLIWriteLine(pair.Value.TextInTable() + " " + pair.Value.Blocked.ToString() + OtherFunctions.Space(15, pair.Value.Blocked.ToString()) + " |");
                            CLI.CLIWriteLine(OtherFunctions.LineInTable() + "-----------------+");
                        }
                        CLI.CLIWriteLine();
                        break;
                    //Block
                    case 2:
                        CLI.CLIWriteLine("\nEnter the username of the account you want to block:");
                        username = OtherFunctions.EnterString();
                        if(UserRepository.Patients.TryGetValue(username, out patient))
                            if(patient.Blocked == Blocked.Unblocked)
                                patient.Blocked = Blocked.Secretary;
                            else
                                CLI.CLIWriteLine("This patient's account is already blocked.");
                        else
                            CLI.CLIWriteLine("There is no patient's account with such username.");
                        break;
                    //Unblock
                    case 3:
                        CLI.CLIWriteLine("\nEnter the username of the account you want to unblock:");
                        username = OtherFunctions.EnterString();
                        if(UserRepository.Patients.TryGetValue(username, out patient))
                            if(patient.Blocked != Blocked.Unblocked)
                                patient.Blocked = Blocked.Unblocked;
                            else
                                CLI.CLIWriteLine("This patient's account is already unblocked.");
                        else
                            CLI.CLIWriteLine("There is no patient's account with such username.");
                        break;
                }
            }
        }

        //Manages patient requests.
        private static void ManageExaminationRequests()
        {
            int id, option;
            Clinic.Appointment appointment;

            CLI.CLIWriteLine();
            using (StreamReader reader = new StreamReader(SystemFunctions.PatientRequestsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    CLI.CLIWriteLine(line);
                    CLI.CLIWriteLine("1: Approve");
                    CLI.CLIWriteLine("2: Deny");
                    option = OtherFunctions.EnterNumberWithLimit(1, 2);
                    if(option == 1)
                    {
                        if (!int.TryParse(line.Split("|")[0], out id))
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
                            if (UserRepository.Doctors.TryGetValue(line.Split("|")[3], out doctor))
                            {
                                appointment.Doctor.Appointments.Remove(appointment);
                                appointment.Patient.Appointments.Remove(appointment);
                                var last = SystemFunctions.AllAppointments.Values.Last();
                                Clinic.Examination editedExamination = new Clinic.Examination(last.ID + 1, appointment.DateTime, doctor, appointment.Patient, appointment.Finished, 0, appointment.ID);
                                SystemFunctions.AllAppointments.Add(editedExamination.ID, editedExamination);
                                SystemFunctions.CurrentAppointments.Remove(appointment.ID);
                                SystemFunctions.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                                PatientService.InsertAppointmentPatient(appointment.Patient, editedExamination);
                                editedExamination.Doctor.InsertAppointment(editedExamination);
                            }
                        }
                    }
                }
            }
            CLI.CLIWriteLine();
        }

        //Creates examinations based upon referrals.
        private static void CreateExaminationsFromReferrals()
        {
            int option = 1;
            string userName;
            Patient patient;
            while(option != 0)
            {
                CLI.CLIWriteLine("\nEnter the patients username:");
                userName = OtherFunctions.EnterString();
                if (UserRepository.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.
                    if(patient.Referrals.Count() == 0)
                    {
                        CLI.CLIWriteLine("\nThis patient has no referrals.");
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
                        DateTime dateTime, date, time;
                        while(hasTime == false && option2 != 0)
                        {
                            CLI.CLIWrite("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");
                            date = OtherFunctions.GetGoodDate();

                            CLI.CLIWrite("\nEnter the time of your Examination (e.g. 14:30)\n>> ");
                            time = OtherFunctions.AskForTime();

                            dateTime = date.Date + time.TimeOfDay;
                            if (dateTime < DateTime.Now)
                            {
                                CLI.CLIWriteLine("You can't enter that time, it's in the past");
                            }
                            else
                            {
                                hasTime = true;
                                if (!doctor.CheckAppointment(dateTime, 15))
                                {
                                    hasTime = false;
                                    CLI.CLIWriteLine("The doctor is not availible at that time. Try again?");
                                    CLI.CLIWriteLine("1: Yes");
                                    CLI.CLIWriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                                else if (!PatientService.CheckAppointment(patient, dateTime, 15))
                                {
                                    hasTime = false;
                                    CLI.CLIWriteLine("The patient is not availible at that time. Try again?");
                                    CLI.CLIWriteLine("1: Yes");
                                    CLI.CLIWriteLine("0: No");
                                    option2 = OtherFunctions.EnterNumberWithLimit(0, 1);
                                }
                                else
                                {
                                    //Creates the examination.
                                    Clinic.Examination examination = new Clinic.Examination(id, dateTime, doctor, patient, false, 0, 0);
                                    doctor.InsertAppointment(examination);
                                    PatientService.InsertAppointmentPatient(patient, examination);
                                    SystemFunctions.AllAppointments.Add(id, examination);
                                    SystemFunctions.CurrentAppointments.Add(id, examination);
                                    patient.Referrals.RemoveAt(0);
                                    CLI.CLIWriteLine("\nNew examination successfully created\n");
                                }
                            }
                        }
                    }
                }
                else
                {
                    CLI.CLIWriteLine("\nThere is no such patient. Try again?");
                    CLI.CLIWriteLine("1: Yes");
                    CLI.CLIWriteLine("0: No");
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
                CLI.CLIWriteLine("\nEnter the patients username:");
                userName = OtherFunctions.EnterString();
                if (UserRepository.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.

                    //Finding the doctors field of expertise.
                    int option2, numberOfOptions = 0;
                    CLI.CLIWriteLine("Which specialty does the doctor need to possess?");
                    List<Fields> fields = new List<Fields>();
                    foreach(Fields field in (Fields[])Enum.GetValues(typeof(Fields)))
                    {
                        numberOfOptions++;
                        CLI.CLIWriteLine(numberOfOptions + ": " + field);
                        fields.Add(field);
                    }
                    option2 = OtherFunctions.EnterNumberWithLimit(1, numberOfOptions);
                    Fields fieldOfDoctor = fields[option2 - 1]; //-1 because it starts from zero and options start from 1.

                    //Checking if there's a doctor with that specialty.
                    bool hasSpecialty = false;
                    foreach(KeyValuePair<string, Doctor> pair in UserRepository.Doctors)
                    {
                        if(pair.Value.Field == fieldOfDoctor)
                            hasSpecialty = true;
                    }
                    if(hasSpecialty == false)
                    {
                        CLI.CLIWriteLine("\nNo doctor has that specialty.");
                        return;
                    }

                    //Finding the available time for examination.
                    bool hasFoundTime = false;
                    DateTime dateTime = DateTime.Now.AddMinutes(5);
                    while(dateTime <= DateTime.Now.AddMinutes(120) && hasFoundTime == false)
                    {
                        DateRange dateRange = new DateRange(dateTime, dateTime.AddMinutes(15));
                        if(PatientService.CheckAppointment(patient, dateTime, 15))
                        {
                            foreach(KeyValuePair<string, Doctor> doctor in UserRepository.Doctors)
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
                                    PatientService.InsertAppointmentPatient(patient, examination);
                                    SystemFunctions.AllAppointments.Add(id, examination);
                                    SystemFunctions.CurrentAppointments.Add(id, examination);
                                    doctor.Value.MessageBox.AddMessage("You have an emergency examination.");
                                    patient.MessageBox.AddMessage("You have an emergency examination.");
                                    CLI.CLIWriteLine("The examination has been created successfully.");
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
                        if (examinationForDelay.Value.DateTime > DateTime.Now && examinationForDelay.Value.DateTime < DateTime.Now.AddMinutes(120) && examinationForDelay.Value.Doctor.Field == fieldOfDoctor && (examinationForDelay.Value.Patient == patient || PatientService.CheckAppointment(patient, examinationForDelay.Value.DateTime, 15)))
                        {
                            examinationsForDelaying.Add(new KeyValuePair<Clinic.Examination, DateTime>((Clinic.Examination)examinationForDelay.Value, examinationForDelay.Value.NextAvailable()));
                            CLI.CLIWriteLine(examinationForDelay.Key.ToString());
                        }
                    if(examinationsForDelaying.Count == 0)
                    {
                        CLI.CLIWriteLine("\nThere are no examinations available for delaying.");
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
                    CLI.CLIWriteLine("\nWhich examination should we delay?");
                    for(int i = 0; i < numberOfOptions; i++)
                    {
                        CLI.CLIWriteLine((i + 1) + ": " + examinationsForDelayingOptions[i].Key.ID + " will be delayed from " + examinationsForDelayingOptions[i].Key.DateTime + " to " + examinationsForDelayingOptions[i].Value);
                    }
                    CLI.CLIWriteLine("0: Back to menu");
                    if (numberOfOptions < 5)
                        CLI.CLIWriteLine("There are no more examinations left that can be delayed.");

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
                    PatientService.InsertAppointmentPatient(patient, examination2);
                    SystemFunctions.AllAppointments.Add(id, examination2);
                    SystemFunctions.CurrentAppointments.Add(id, examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("You have an emergency examination.");
                    patient.MessageBox.AddMessage("You have an emergency examination.");
                    CLI.CLIWriteLine("The examination has been created successfully.");

                    //Delays the other examination.
                    examinationForDelaying.Key.Doctor.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.Patient.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.DateTime = examinationForDelaying.Value;
                    examinationForDelaying.Key.Edited++;
                    examinationForDelaying.Key.Doctor.InsertAppointment(examination2);
                    PatientService.InsertAppointmentPatient(examinationForDelaying.Key.Patient, examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("Your examination has been delayed.");
                    examinationForDelaying.Key.Patient.MessageBox.AddMessage("Your examination has been delayed.");
                    CLI.CLIWriteLine("The other examination has been delayed successfully.");
                }
                else
                {
                    CLI.CLIWriteLine("\nThere is no such patient. Try again?");
                    CLI.CLIWriteLine("1: Yes");
                    CLI.CLIWriteLine("0: No");
                    option = OtherFunctions.EnterNumberWithLimit(0, 1);
                }
            }
        }

        //Makes an order for dynamic equipment.
        private static void OrderDynamiicEquipment()
        {
            bool gauzes = false, stiches = false, vaccines = false, bandages = false;
            foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
            {
                if (equipment.Amount > 0 && equipment.Type == EquipmentType.Gauzes && equipment.RoomId == 0)
                    gauzes = true;
                if (equipment.Amount > 0 && equipment.Type == EquipmentType.Stiches && equipment.RoomId == 0)
                    stiches = true;
                if (equipment.Amount > 0 && equipment.Type == EquipmentType.Vaccines && equipment.RoomId == 0)
                    vaccines = true;
                if (equipment.Amount > 0 && equipment.Type == EquipmentType.Bandages && equipment.RoomId == 0)
                    bandages = true;
            }
            if (gauzes == true && stiches == true && vaccines == true && bandages == true)
                CLI.CLIWriteLine("\nWe don't lack any equipment at the moment.");
            else
            {
                int numberOfOptions, option = 1;
                while(option != 0)
                {
                    numberOfOptions = 0;
                    CLI.CLIWriteLine("\nWhich of the following equipment would you like to order?");
                    if(gauzes == false)
                    {
                        numberOfOptions++;
                        CLI.CLIWriteLine(numberOfOptions + ": Gauzes");
                    }
                    if(stiches == false)
                    {
                        numberOfOptions++;
                        CLI.CLIWriteLine(numberOfOptions + ": Stiches");
                    }
                    if(vaccines == false)
                    {
                        numberOfOptions++;
                        CLI.CLIWriteLine(numberOfOptions + ": Vaccines");
                    }
                    if(bandages == false)
                    {
                        numberOfOptions++;
                        CLI.CLIWriteLine(numberOfOptions + ": Bandages");
                    }
                    CLI.CLIWriteLine("0: Back to menu");
                    option = OtherFunctions.EnterNumberWithLimit(0, numberOfOptions);
                    if(option != 0)
                    {
                        CLI.CLIWriteLine("");
                        if (gauzes == false)
                        {
                            option--;
                            if(option == 0)
                            {
                                CLI.CLIWriteLine("How many gauzes would you like to order?");
                                option = OtherFunctions.EnterNumberWithLimit(1, 1000);
                                EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Gauzes, option, DateTime.Now.Date);
                                SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                            }
                        }
                        if (stiches == false)
                        {
                            option--;
                            if (option == 0)
                            {
                                CLI.CLIWriteLine("How many stiches would you like to order?");
                                option = OtherFunctions.EnterNumberWithLimit(1, 1000);
                                EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Stiches, option, DateTime.Now.Date);
                                SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                            }
                        }
                        if (vaccines == false)
                        {
                            option--;
                            if (option == 0)
                            {
                                CLI.CLIWriteLine("How many vaccines would you like to order?");
                                option = OtherFunctions.EnterNumberWithLimit(1, 1000);
                                EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Vaccines, option, DateTime.Now.Date);
                                SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                            }
                        }
                        if (bandages == false)
                        {
                            option--;
                            if (option == 0)
                            {
                                CLI.CLIWriteLine("How many bandages would you like to order?");
                                option = OtherFunctions.EnterNumberWithLimit(1, 1000);
                                EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Bandages, option, DateTime.Now.Date);
                                SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                            }
                        }
                        //In the end, option will still be greater than 0
                    }
                }
            }
        }

        //Redistributes dynamic equipment
        private static void RedistributeDynamiicEquipment()
        {
            foreach(Room room in RoomRepo.ClinicRooms)
            {
                int gauzes = 0, stiches = 0, vaccines = 0, bandages = 0;
                foreach(Equipment equipment in EquipmentRepo.ClinicEquipmentList)
                {
                    if (equipment.Type == EquipmentType.Gauzes && equipment.RoomId == room.Id)
                        gauzes += equipment.Amount;
                    if (equipment.Type == EquipmentType.Stiches && equipment.RoomId == room.Id)
                        stiches += equipment.Amount;
                    if (equipment.Type == EquipmentType.Vaccines && equipment.RoomId == room.Id)
                        vaccines += equipment.Amount;
                    if (equipment.Type == EquipmentType.Bandages && equipment.RoomId == room.Id)
                        bandages += equipment.Amount;
                }
                if(gauzes < 5 || stiches < 5 || vaccines < 5 || bandages < 5)
                {
                    CLI.CLIWriteLine("\nRoom id: " + room.Id);
                    CLI.CLIWriteLine("Room name: " + room.Name);
                    if (gauzes == 0)
                        CLI.CLIWriteLine("-Gauzes: " + gauzes);
                    else if(gauzes < 5)
                        CLI.CLIWriteLine(" Gauzes: " + gauzes);
                    if (stiches == 0)
                        CLI.CLIWriteLine("-Stiches: " + stiches);
                    else if(stiches < 5)
                        CLI.CLIWriteLine(" Stiches: " + stiches);
                    if (vaccines == 0)
                        CLI.CLIWriteLine("-Vaccines: " + vaccines);
                    else if(vaccines < 5)
                        CLI.CLIWriteLine(" Vaccines: " + vaccines);
                    if (bandages == 0)
                        CLI.CLIWriteLine("-Bandages: " + bandages);
                    else if(bandages < 5)
                        CLI.CLIWriteLine(" Bandages: " + bandages);
                }
            }

            int option = 1;
            while(option != 0)
            {
                CLI.CLIWriteLine("\nDo you want to move equipment?");
                CLI.CLIWriteLine("1: Yes");
                CLI.CLIWriteLine("0: No");
                option = OtherFunctions.EnterNumberWithLimit(0, 1);
                if (option == 1)
                {
                    int idFrom, idTo, amount, totalEquipment = 0;
                    EquipmentType type;
                    Room roomFrom, roomTo;
                    CLI.CLIWriteLine("\nEnter the id of the room from which you want to move dynamic equipment:");
                    idFrom = OtherFunctions.EnterNumber();
                    idTo = OtherFunctions.EnterNumber();
                    amount = OtherFunctions.EnterNumber();
                    CLI.CLIWriteLine("\nWhich of the following equipment would you like to move?");
                    CLI.CLIWriteLine("1: Gauzes");
                    CLI.CLIWriteLine("2: Stiches");
                    CLI.CLIWriteLine("3: Vaccines");
                    CLI.CLIWriteLine("4: Bandages");
                    option = OtherFunctions.EnterNumberWithLimit(1, 4);
                    switch (option)
                    {
                        case 1:
                            type = EquipmentType.Gauzes;
                            break;
                        case 2:
                            type = EquipmentType.Stiches;
                            break;
                        case 3:
                            type = EquipmentType.Vaccines;
                            break;
                        default:
                            type = EquipmentType.Bandages;
                            break;
                    }
                    roomFrom = RoomRepo.Get(idFrom);
                    if(roomFrom == default)
                        roomFrom = RoomRepo.Get(0);
                    roomTo = RoomRepo.Get(idTo);
                    if (roomTo == default)
                        roomTo = RoomRepo.Get(0);
                    foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
                    {
                        if (equipment.Type == type && equipment.RoomId == roomFrom.Id)
                            totalEquipment += equipment.Amount;
                    }
                    if(amount > totalEquipment)
                        amount = totalEquipment;
                    Equipment equipmentNew = new Equipment
                    {
                        Id = 0,
                        Name = type.ToString(),
                        Amount = amount,
                        RoomId = roomTo.Id,
                        Type = type
                    };
                    EquipmentRepo.Add(equipmentNew);
                    foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
                        if (equipment.Type == type && equipment.RoomId == roomTo.Id && amount > 0)
                            if(amount < equipment.Amount)
                            {
                                equipment.Amount -= amount;
                                amount = 0;
                            }
                            else
                            {
                                amount -= equipment.Amount;
                                EquipmentRepo.ClinicEquipmentList.Remove(equipment);
                            }
                    EquipmentRepo.PersistChanges();
                }
            }
        }
    }
}
