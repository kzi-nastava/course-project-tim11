using System;
using System.IO;
using System.Linq;
using ClinicApp.Users;
using System.Collections.Generic;
using ClinicApp.HelperClasses;

namespace ClinicApp.Clinic
{
    public class ExaminationService
    {
        public ExaminationService()
        {
        }

        public static void ViewExaminations(Patient patient)
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


        //Creates examinations based upon referrals.
        public static void CreateExaminationsFromReferrals()
        {
            int option = 1;
            string userName;
            Patient patient;
            while (option != 0)
            {
                CLI.CLIWriteLine("\nEnter the patients username:");
                userName = CLI.CLIEnterString();
                if (UserRepository.Patients.TryGetValue(userName, out patient))
                {
                    option = 0; //We found the patient. No need to search for him again.
                    if (patient.Referrals.Count() == 0)
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
                                if (!DoctorService.CheckAppointment(dateTime, 15, ref doctor))
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
                                    DoctorService.InsertAppointment(examination, ref doctor);
                                    PatientService.InsertAppointmentPatient(ref patient, examination);
                                    AppointmentRepo.AllAppointments.Add(id, examination);
                                    AppointmentRepo.CurrentAppointments.Add(id, examination);
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
                    foreach (Fields field in (Fields[])Enum.GetValues(typeof(Fields)))
                    {
                        numberOfOptions++;
                        CLI.CLIWriteLine(numberOfOptions + ": " + field);
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
                        CLI.CLIWriteLine("\nNo doctor has that specialty.");
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

                                    DoctorService.CheckAppointment(dateTime, 15, ref doctorTmp))
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
                                    DoctorService.InsertAppointment(examination, ref doctorTmp);
                                    PatientService.InsertAppointmentPatient(ref patient, examination);
                                    AppointmentRepo.AllAppointments.Add(id, examination);
                                    AppointmentRepo.CurrentAppointments.Add(id, examination);
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
                    foreach (KeyValuePair<int, Clinic.Appointment> examinationForDelay in AppointmentRepo.AllAppointments)
                        if (examinationForDelay.Value.DateTime > DateTime.Now && examinationForDelay.Value.DateTime < DateTime.Now.AddMinutes(120) && examinationForDelay.Value.Doctor.Field == fieldOfDoctor && (examinationForDelay.Value.Patient == patient || PatientService.CheckAppointment(patient, examinationForDelay.Value.DateTime, 15)))
                        {
                            examinationsForDelaying.Add(new KeyValuePair<Clinic.Examination, DateTime>((Clinic.Examination)examinationForDelay.Value, examinationForDelay.Value.NextAvailable()));
                            CLI.CLIWriteLine(examinationForDelay.Key.ToString());
                        }
                    if (examinationsForDelaying.Count == 0)
                    {
                        CLI.CLIWriteLine("\nThere are no examinations available for delaying.");
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
                    CLI.CLIWriteLine("\nWhich examination should we delay?");
                    for (int i = 0; i < numberOfOptions; i++)
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
                    DoctorService.InsertAppointment(examination2, ref doctorTmp2);
                    PatientService.InsertAppointmentPatient(ref patient, examination2);
                    AppointmentRepo.AllAppointments.Add(id, examination2);
                    AppointmentRepo.CurrentAppointments.Add(id, examination2);
                    examinationForDelaying.Key.Doctor.MessageBox.AddMessage("You have an emergency examination.");
                    patient.MessageBox.AddMessage("You have an emergency examination.");
                    CLI.CLIWriteLine("The examination has been created successfully.");

                    //Delays the other examination.
                    examinationForDelaying.Key.Doctor.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.Patient.Appointments.Remove(examinationForDelaying.Key);
                    examinationForDelaying.Key.DateTime = examinationForDelaying.Value;
                    examinationForDelaying.Key.Edited++;
                    DoctorService.InsertAppointment(examination2, ref doctorTmp2);
                    Patient patientTmp = examinationForDelaying.Key.Patient;
                    PatientService.InsertAppointmentPatient(ref patientTmp, examination2);
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
