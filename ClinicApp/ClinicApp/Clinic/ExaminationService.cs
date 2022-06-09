using System;
using System.IO;
using System.Linq;
using ClinicApp.Users;

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

        private void CreateExamination(Doctor doctor = null)
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
                    DoctorService.ViewAllDoctors();
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
                bool validateAppointment = doctor.CheckAppointment(dateTime, duration);
                if (validateAppointment == false)
                {
                    Console.WriteLine("Your doctor is unavailable at that time.");
                    return;
                }
            }
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

        //razbij na vise manjih funkcija!!!
        private void EditExamination()
        {
            bool quit = false;
            Console.WriteLine("Enter the ID of the examination you want to edit:");
            int id = CLI.CLIEnterNumber();
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
                DateTime newDate = CLI.CLIEnterDate();
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
                DateTime newTime = CLI.CLIEnterTime();
                DateTime oldTime = examination.DateTime;
                examination.DateTime.Date.Add(newTime.TimeOfDay);
                bool validation = examination.Doctor.CheckAppointment(examination.DateTime, duration);
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
                if (!UserRepository.Doctors.TryGetValue(UserName, out doctor))
                {
                    Console.WriteLine("Doctor with that user name does not eixst.");
                    return;
                }
                bool validate = doctor.CheckAppointment(examination.DateTime, duration);
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
    }
}
