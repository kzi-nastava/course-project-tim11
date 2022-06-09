using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClinicApp.AdminFunctions;
using ClinicApp.HelperClasses;

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

        //Compresses a Secretary object into a string for easier upload.
        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        //Writes all the options a secretary has once he logs in.
        public override int MenuWrite()
        {
            return Menu.SecretaryMenuWrite(this);
        }

        //Executes the chosen command.
        public override void MenuDo(int option)
        {
            Menu.SecretaryMenuDo(this, option);
        }

        //Manages blocked and unblocked patients.
        public static void ManageBlockedPatients()
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
                option = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
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
                        username = CLI.CLIEnterString();
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
                        username = CLI.CLIEnterString();
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
        public static void ManageExaminationRequests()
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
                    option = CLI.CLIEnterNumberWithLimit(1, 2);
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
        public static void CreateExaminationsFromReferrals()
        {
            int option = 1;
            string userName;
            Patient patient;
            while(option != 0)
            {
                CLI.CLIWriteLine("\nEnter the patients username:");
                userName = CLI.CLIEnterString();
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
                            time = CLI.CLIEnterTime();

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
                                    option2 = CLI.CLIEnterNumberWithLimit(0, 1);
                                }
                                else if (!PatientService.CheckAppointment(patient, dateTime, 15))
                                {
                                    hasTime = false;
                                    CLI.CLIWriteLine("The patient is not availible at that time. Try again?");
                                    CLI.CLIWriteLine("1: Yes");
                                    CLI.CLIWriteLine("0: No");
                                    option2 = CLI.CLIEnterNumberWithLimit(0, 1);
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
                    option = CLI.CLIEnterNumberWithLimit(0, 1);
                }
            }
        }

        //Creates emergency examinations.
        public static void CreateEmergencyExamination()
        {
            string userName;
            Patient patient;
            int option = 1, id;
            while (option != 0)
            {
                //Finding the patient.
                CLI.CLIWriteLine("\nEnter the patients username:");
                userName = CLI.CLIEnterString();
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
                    option2 = CLI.CLIEnterNumberWithLimit(1, numberOfOptions);
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
                    option2 = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
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
                    option = CLI.CLIEnterNumberWithLimit(0, 1);
                }
            }
        }
    }
}
