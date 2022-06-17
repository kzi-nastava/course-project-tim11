using System;
using System.IO;
using System.Linq;
using ClinicApp.Users;
using System.Collections.Generic;
using ClinicApp.HelperClasses;
using ClinicApp.Dialogs;

namespace ClinicApp.Clinic
{
    public class ExaminationService
    {
        public ExaminationService()
        {
        }

        private void ViewExaminations(Patient patient)
        {
            if (patient.Appointments.Count == 0)
            {
                Console.WriteLine("\nNo future examinations\n");
                return;
            }
            int i = 1;
            foreach (Examination examination in patient.Appointments)
            {
                Console.WriteLine($"\n\n{i}. Examination\n\nId: {examination.ID}; \nTime and Date: {examination.DateTime};\nDoctor last name: {examination.Doctor.LastName}; Doctor name: {examination.Doctor.Name}\n");
                i++;
            }
        }
        private void CreateExamination(Patient patient, Doctor doctor = null)
        {
            DateTime dateTime = DateTime.Now;
            Console.Write("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");
            DateTime date = OtherFunctions.GetGoodDate();
            Console.Write("\nEnter the time of your Examination (e.g. 14:30)\n>> ");

            DateTime time;
            //NEPOTREBAN DO WHILE!!!!
            do
            {
                time = CLI.CLIEnterTime();
                time = date.Date + time.TimeOfDay;
                if (time < DateTime.Now)
                {
                    Console.WriteLine("You can't enter that time, its in the past");
                }
            } while (time < DateTime.Now);

            dateTime = time;
            if (doctor == null)
            {
                // NAPRAVI FUNKCIJU KADA JE DOKTOR NULL
                Console.WriteLine("Enter the username of doctor. Do you want to view the list of doctors? (y/n)");
                Console.Write(">>");
                string choice = Console.ReadLine();
                Console.WriteLine();
                if (choice.ToUpper() == "Y")
                {
                    DoctorDialog.ViewAllDoctors();
                }
                Console.WriteLine("\nEnter the username:");
                string userName = Console.ReadLine();
                Doctor doctorDict = null;
                if (!UserRepository.Doctors.TryGetValue(userName, out doctorDict))
                {
                    Console.WriteLine("Doctor with that username does not exist");
                    return;
                }
                doctor = UserRepository.Doctors[userName];
                //TODO : types of appointment -> duration
                int duration = 15;
                bool validateAppointment = AppointmentService.CheckAppointment(dateTime, duration, ref doctor);
                if (validateAppointment == false)
                {
                    Console.WriteLine("Your doctor is unavailable at that time.");
                    return;
                }
            }
            int id;
            try
            {
                id = AppointmentRepo.AllAppointments.Values.Last().ID + 1;
            }
            catch
            {
                id = 1;
            }
            Examination examination = new Examination(id, dateTime, doctor, patient, false, 0, 0);
            PatientService.InsertAppointmentPatient(ref patient, examination) ;
            AppointmentService.InsertAppointment(examination, ref doctor);
            AppointmentRepo.AllAppointments.Add(id, examination);
            AppointmentRepo.CurrentAppointments.Add(id, examination);
            Console.WriteLine("\nNew examination successfully created\n");
            //ActivityHistory.Add(DateTime.Now, "CREATE");
        }

        private void DeleteExamination(Patient patient)
        {
            Console.WriteLine("Enter the ID of the examination you want to delete?");
            int id = CLI.CLIEnterNumber();
            Examination examination = null;
            foreach (Examination tmp in patient.Appointments)
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
                            //ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                            return;
                        }
                        patient.Appointments.Remove(examination);
                        var last = AppointmentRepo.AllAppointments.Values.Last();
                        Examination deletedExamination = new Examination(last.ID + 1, examination.DateTime, examination.Doctor, patient, examination.Finished, examination.ID, examination.Edited);
                        AppointmentRepo.AllAppointments.Add(deletedExamination.ID, deletedExamination);
                        AppointmentRepo.CurrentAppointments.Remove(examination.ID);
                        examination.Doctor.Appointments.Remove(examination);
                        //ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    }
                    break;
                }
            }
        }

        //razbij na vise manjih funkcija!!!
        private void EditExamination(Patient patient)
        {
            bool quit = false;
            Console.WriteLine("Enter the ID of the examination you want to edit:");
            int id = CLI.CLIEnterNumber();
            Examination examination = null;
            int duration = 15;

            while (examination == null)
            {
                foreach (Examination tmp in patient.Appointments)
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
                DateTime newDate = CLI.CLIEnterDate();
                newDate += examination.DateTime.TimeOfDay;
                //TODO : type of appointment -> duration
                Doctor doctortmp = examination.Doctor;
                bool validation = AppointmentService.CheckAppointment(newDate, duration, ref doctortmp);
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

                    //ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                patient.Appointments.Remove(examination);
                examination.Doctor.Appointments.Remove(examination);
                var last = AppointmentRepo.AllAppointments.Values.Last();
                Examination editedExamination = new Examination(last.ID + 1, newDate, examination.Doctor, patient, examination.Finished, 0, examination.ID);
                AppointmentRepo.AllAppointments.Add(editedExamination.ID, editedExamination);
                AppointmentRepo.CurrentAppointments.Remove(examination.ID);
                AppointmentRepo.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                patient.Appointments.Add(editedExamination);
                editedExamination.Doctor.Appointments.Add(editedExamination);

            }
            else if (choice.ToUpper() == "T")
            {
                Console.WriteLine("Enter the new time of your Examination (e.g. 12:00)");
                DateTime newTime = CLI.CLIEnterTime();
                DateTime oldTime = examination.DateTime;
                examination.DateTime.Date.Add(newTime.TimeOfDay);
                Doctor doctorTmp = examination.Doctor;
                bool validation = AppointmentService.CheckAppointment(examination.DateTime, duration, ref doctorTmp);
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
                    //ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                patient.Appointments.Remove(examination);
                examination.Doctor.Appointments.Remove(examination);
                var last = AppointmentRepo.AllAppointments.Values.Last();
                Examination editedExamination = new Examination(last.ID + 1, newTime, examination.Doctor, patient, examination.Finished, 0, examination.ID);
                AppointmentRepo.AllAppointments.Add(editedExamination.ID, editedExamination);
                AppointmentRepo.CurrentAppointments.Remove(examination.ID);
                AppointmentRepo.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                patient.Appointments.Add(editedExamination);
                editedExamination.Doctor.Appointments.Add(editedExamination);

            }
            else if (choice.ToUpper() == "DR")
            {
                Console.WriteLine("Do you want to view list of doctors? (y/n)");
                Console.WriteLine(">>");
                string input = Console.ReadLine();
                if (input.ToUpper() == "Y")
                {
                    DoctorDialog.ViewAllDoctors();
                }
                Console.WriteLine("Write username of new doctor:");
                string inputUserName = Console.ReadLine();
                Doctor doctor = null;
                if (!UserRepository.Doctors.TryGetValue(patient.UserName, out doctor))
                {
                    Console.WriteLine("Doctor with that user name does not eixst.");
                    return;
                }
                bool validate = AppointmentService.CheckAppointment(examination.DateTime, duration, ref doctor);
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
                    //ActivityHistory.Add(DateTime.Now, "DELETE/UPDATE");
                    return;
                }
                //proveri kada se radi izmena

                patient.Appointments.Remove(examination);
                examination.Doctor.Appointments.Remove(examination);
                var last = AppointmentRepo.AllAppointments.Values.Last();
                Examination editedExamination = new Examination(last.ID + 1, examination.DateTime, doctor, patient, examination.Finished, 0, examination.ID);
                AppointmentRepo.AllAppointments.Add(editedExamination.ID, editedExamination);
                AppointmentRepo.CurrentAppointments.Remove(examination.ID);
                AppointmentRepo.CurrentAppointments.Add(editedExamination.ID, editedExamination);
                patient.Appointments.Add(editedExamination);
                editedExamination.Doctor.Appointments.Add(editedExamination);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        //Creates examinations based upon referrals.
        public static void CreateExaminationsFromReferrals()
        {
            int option = 1;
            string userName;
            Patient patient;
            while (option != 0)
            {
                Console.WriteLine("\nEnter the patients username:");
                userName = Console.ReadLine();
                if (UserRepository.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.
                    if (patient.Referrals.Count() == 0)
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
                        foreach (int examinationID in AppointmentRepo.AllAppointments.Keys)
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
                        while (hasTime == false && option2 != 0)
                        {
                            Console.Write("\nEnter the date of your Examination (e.g. 22/10/1987)\n>> ");
                            date = OtherFunctions.GetGoodDate();

                            Console.Write("\nEnter the time of your Examination (e.g. 14:30)\n>> ");
                            time = CLI.CLIEnterTime();

                            dateTime = date.Date + time.TimeOfDay;
                            if (dateTime < DateTime.Now)
                            {
                                Console.WriteLine("You can't enter that time, it's in the past");
                            }
                            else
                            {
                                hasTime = true;
                                if (!AppointmentService.CheckAppointment(dateTime, 15, ref doctor))
                                {
                                    hasTime = false;
                                    Console.WriteLine("The doctor is not availible at that time. Try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = CLI.CLIEnterNumberWithLimit(0, 1);
                                }
                                else if (!PatientService.CheckAppointment(patient, dateTime, 15))
                                {
                                    hasTime = false;
                                    Console.WriteLine("The patient is not availible at that time. Try again?");
                                    Console.WriteLine("1: Yes");
                                    Console.WriteLine("0: No");
                                    option2 = CLI.CLIEnterNumberWithLimit(0, 1);
                                }
                                else
                                {
                                    //Creates the examination.
                                    Clinic.Examination examination = new Clinic.Examination(id, dateTime, doctor, patient, false, 0, 0);
                                    AppointmentService.InsertAppointment(examination, ref doctor);
                                    PatientService.InsertAppointmentPatient(ref patient, examination);
                                    AppointmentRepo.AllAppointments.Add(id, examination);
                                    AppointmentRepo.CurrentAppointments.Add(id, examination);
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
                Console.WriteLine("\nEnter the patients username:");
                userName = Console.ReadLine();
                if (UserRepository.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.

                    //Finding the doctors field of expertise.
                    int option2, numberOfOptions = 0;
                    Console.WriteLine("Which specialty does the doctor need to possess?");
                    List<Fields> fields = new List<Fields>();
                    foreach (Fields field in (Fields[])Enum.GetValues(typeof(Fields)))
                    {
                        numberOfOptions++;
                        Console.WriteLine(numberOfOptions + ": " + field);
                        fields.Add(field);
                    }
                    option2 = CLI.CLIEnterNumberWithLimit(1, numberOfOptions);
                    Fields fieldOfDoctor = fields[option2 - 1]; //-1 because it starts from zero and options start from 1.

                    //Checking if there's a doctor with that specialty.
                    bool hasSpecialty = false;
                    foreach (KeyValuePair<string, Doctor> pair in UserRepository.Doctors)
                    {
                        if (pair.Value.Field == fieldOfDoctor)
                            hasSpecialty = true;
                    }
                    if (hasSpecialty == false)
                    {
                        Console.WriteLine("\nNo doctor has that specialty.");
                        return;
                    }

                    //Finding the available time for examination.
                    bool hasFoundTime = false;
                    DateTime dateTime = DateTime.Now.AddMinutes(5);
                    while (dateTime <= DateTime.Now.AddMinutes(120) && hasFoundTime == false)
                    {
                        DateRange dateRange = new DateRange(dateTime, dateTime.AddMinutes(15));
                        if (PatientService.CheckAppointment(patient, dateTime, 15))
                        {
                            foreach (KeyValuePair<string, Doctor> doctor in UserRepository.Doctors)
                            {
                                Doctor doctorTmp = doctor.Value;
                                if (doctor.Value.Field == fieldOfDoctor && !OtherFunctions.CheckForRenovations(dateRange, doctor.Value.RoomId) &&
                                    !OtherFunctions.CheckForExaminations(dateRange, doctor.Value.RoomId) &&

                                    AppointmentService.CheckAppointment(dateTime, 15, ref doctorTmp))
                                {
                                    hasFoundTime = true;
                                    //Finds the id.
                                    id = 0;
                                    foreach (int examinationID in AppointmentRepo.AllAppointments.Keys)
                                    {
                                        if (examinationID > id)
                                        {
                                            id = examinationID;
                                        }
                                    }
                                    id++;
                                    Clinic.Examination examination = new Clinic.Examination(id, dateTime, doctor.Value, patient, false, 0, 0);
                                    AppointmentService.InsertAppointment(examination, ref doctorTmp);
                                    PatientService.InsertAppointmentPatient(ref patient, examination);
                                    AppointmentRepo.AllAppointments.Add(id, examination);
                                    AppointmentRepo.CurrentAppointments.Add(id, examination);
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
                    foreach (KeyValuePair<int, Clinic.Appointment> examinationForDelay in AppointmentRepo.AllAppointments)
                        if (examinationForDelay.Value.DateTime > DateTime.Now && examinationForDelay.Value.DateTime < DateTime.Now.AddMinutes(120) && examinationForDelay.Value.Doctor.Field == fieldOfDoctor && (examinationForDelay.Value.Patient == patient || PatientService.CheckAppointment(patient, examinationForDelay.Value.DateTime, 15)))
                        {
                            examinationsForDelaying.Add(new KeyValuePair<Clinic.Examination, DateTime>((Clinic.Examination)examinationForDelay.Value, examinationForDelay.Value.NextAvailable()));
                            Console.WriteLine(examinationForDelay.Key.ToString());
                        }
                    if (examinationsForDelaying.Count == 0)
                    {
                        Console.WriteLine("\nThere are no examinations available for delaying.");
                        return;
                    }

                    //Then, we make a list of 5 options.
                    numberOfOptions = 0;
                    List<KeyValuePair<Clinic.Examination, DateTime>> examinationsForDelayingOptions = new List<KeyValuePair<Clinic.Examination, DateTime>>();
                    while (numberOfOptions < 5 && examinationsForDelaying.Count() > 0)
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
                    for (int i = 0; i < numberOfOptions; i++)
                    {
                        Console.WriteLine((i + 1) + ": " + examinationsForDelayingOptions[i].Key.ID + " will be delayed from " + examinationsForDelayingOptions[i].Key.DateTime + " to " + examinationsForDelayingOptions[i].Value);
                    }
                    Console.WriteLine("0: Back to menu");
                    if (numberOfOptions < 5)
                        Console.WriteLine("There are no more examinations left that can be delayed.");

                    //Finally, we create the examination and delay the other one.
                    option2 = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
                    if (option2 == 0)
                        return;
                    KeyValuePair<Clinic.Examination, DateTime> examinationForDelaying = examinationsForDelayingOptions[option2 - 1]; //-1 because it starts from zero and options start from 1.
                    //Finds the id.
                    id = 0;
                    foreach (int examinationID in AppointmentRepo.AllAppointments.Keys)
                    {
                        if (examinationID > id)
                        {
                            id = examinationID;
                        }
                    }
                    id++;

                    //Creates the examination.
                    Clinic.Examination examination2 = new Clinic.Examination(id, examinationForDelaying.Key.DateTime, examinationForDelaying.Key.Doctor, patient, false, 0, 0);
                    Doctor doctorTmp2 = examinationForDelaying.Key.Doctor;
                    AppointmentService.InsertAppointment(examination2, ref doctorTmp2);
                    PatientService.InsertAppointmentPatient(ref patient, examination2);
                    AppointmentRepo.AllAppointments.Add(id, examination2);
                    AppointmentRepo.CurrentAppointments.Add(id, examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("You have an emergency examination.");
                    patient.MessageBox.AddMessage("You have an emergency examination.");
                    Console.WriteLine("The examination has been created successfully.");

                    //Delays the other examination.
                    examinationForDelaying.Key.Doctor.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.Patient.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.DateTime = examinationForDelaying.Value;
                    examinationForDelaying.Key.Edited++;
                    AppointmentService.InsertAppointment(examination2, ref doctorTmp2);
                    Patient patientTmp = examinationForDelaying.Key.Patient;
                    PatientService.InsertAppointmentPatient(ref patientTmp, examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("Your examination has been delayed.");
                    examinationForDelaying.Key.Patient.MessageBox.AddMessage("Your examination has been delayed.");
                    Console.WriteLine("The other examination has been delayed successfully.");
                }
                else
                {
                    Console.WriteLine("\nThere is no such patient. Try again?");
                    Console.WriteLine("1: Yes");
                    Console.WriteLine("0: No");
                    option = CLI.CLIEnterNumberWithLimit(0, 1);
                }
            }
        }
    }
}
