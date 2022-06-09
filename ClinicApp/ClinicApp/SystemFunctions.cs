using ClinicApp.Clinic;
using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.HelperClasses;

namespace ClinicApp
{
    public enum Roles
    {
        Nobody, Admin, Secretary, Doctor, Patient
    };

    public enum MedicineFoodIntake
    {
        Before, After, During, Irrelevant
    };

    public enum Fields
    {
        Cardiologist, Pneumologist, Gynecologist, Neurologist, Psychiatrist, Anesthesiologist, Pediatrician, Dermatologist, Endocrinologist, Gastroenterologist,
        Oncologist, Ophthalmologist, Urologist, PlasticSurgeon, Pathologist
    };

    public class SystemFunctions
    {
        //Dictionaries

        public static Dictionary<string, HealthRecord> HealthRecords { get; set; } = new Dictionary<string, HealthRecord>();
        public static Dictionary<int, Appointment> AllAppointments { get; set; } = new Dictionary<int, Appointment>();
        public static Dictionary<int, Appointment> CurrentAppointments { get; set; } = new Dictionary<int, Appointment>();

        public static Dictionary<string, Medicine> Medicine { get; set; } = new Dictionary<string, Medicine>();
        public static List<Referral> Referrals { get; set; } = new List<Referral>();
        static public List<EquipmentRequest> EquipmentRequests { get; set; } = new List<EquipmentRequest>();

        // File paths

        public static string AppointmentsFilePath = "../../../Data/appointments.txt";
        public static string HealthRecordsFilePath = "../../../Data/health_records.txt";
        public static string PatientRequestsFilePath = "../../../Data/patient_requests.txt";
        public static string ReferralsFilePath = "../../../Data/referrals.txt";
        public static string MedicineFilePath = "../../../Data/medicine.txt";
        public static string PrescriptionsFilePath = "../../../Data/prescriptions.txt";
        public static string EquipmentRequestsFilePath = "../../../Admin/Data/equipmentRequests.txt";



        // Loads the information from the database into objects and adds them to coresponding dictionaries
        public static void LoadData()
        {
            //Loads the users.
            UserRepository.Load();

            //Loads the messages.
            MessageBoxRepository.Load();

            //Loads the health records.
            using (StreamReader reader = new StreamReader(HealthRecordsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    HealthRecord healthRecord = new HealthRecord(line);
                    HealthRecords.Add(healthRecord.Patient.UserName, healthRecord);
                }
            }

            //Loads medicine.
            using (StreamReader reader = new StreamReader(MedicineFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Medicine medicine = new Medicine(line);
                    Medicine.Add(medicine.Name, medicine);
                }
            }

            //Loads perscriptions.
            using (StreamReader reader = new StreamReader(PrescriptionsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Prescription prescription = new Prescription(line);
                    prescription.Patient.Prescriptions.Add(prescription);
                    prescription.Patient.MessageBox.AddMessage(prescription.PresrciptionToMessage());

                }
            }

            //Loads referrals.
            using (StreamReader reader = new StreamReader(ReferralsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Referral referral = new Referral(line);
                    referral.Patient.Referrals.Add(referral);
                    Referrals.Add(referral);
                }
            }


            //Loads the examinations.
            using (StreamReader reader = new StreamReader(AppointmentsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Appointment appointment = ParseAppointment(line);
                    if (!AllAppointments.TryAdd(appointment.ID, appointment))
                    {
                        AllAppointments[appointment.ID] = appointment;
                    }
                    if (appointment.DateTime.AddMinutes(15) > DateTime.Now)
                    {
                        if (appointment.Tombstone != 0)
                        {
                            CurrentAppointments.Remove(appointment.Tombstone);
                        }
                        else if (appointment.Edited != 0)
                        {
                            CurrentAppointments.Remove(appointment.Edited);
                            if (!appointment.Finished)
                            {
                                CurrentAppointments.Add(appointment.ID, appointment);
                            }

                        }
                        else if (!appointment.Finished)
                        {
                            CurrentAppointments.Add(appointment.ID, appointment);
                        }
                    }

                }
                foreach (int ID in CurrentAppointments.Keys)
                {
                    Appointment currentAppointment = CurrentAppointments[ID];
                    currentAppointment.Doctor.Appointments.Add(currentAppointment);
                    currentAppointment.Patient.Appointments.Add(currentAppointment);
                }
            }

            //Loads the equipment requests.
            using (StreamReader reader = new StreamReader(EquipmentRequestsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    EquipmentRequest equipmentRequest = new EquipmentRequest(line);
                    EquipmentRequests.Add(equipmentRequest);

                }
            }
        }

        private static Appointment ParseAppointment(string line)
        {
            string[] data = line.Split('|');
            if (data[8] == "o")
            {
                return new Operation(line);
            }
            else
            {
                return new Examination(line);
            }
        }


        //Updates certain information that depends on date and time
        public static void Update()
        {
            UpdateEquipmentRequests();
        }

        private static void UpdateEquipmentRequests()
        {
            List<EquipmentRequest> listForRemoving = new List<EquipmentRequest>();
            foreach(EquipmentRequest equipmentRequest in EquipmentRequests)
            {
                if(equipmentRequest.DateCreated < DateTime.Now.Date)
                {
                    equipmentRequest.FulfillOrder();
                    listForRemoving.Add(equipmentRequest);
                }
            }
            foreach(EquipmentRequest equipmentRequest in listForRemoving)
            {
                EquipmentRequests.Remove(equipmentRequest);
            }
        }


        // Uploads the information from the objects into the database
        public static void UploadData()
        {
            string newLine;

            //Uploads the users.
            UserRepository.Upload();

            //Uploads the messages.
            MessageBoxRepository.Upload();

            //Uploads the examinations.
            using (StreamWriter sw = File.CreateText(AppointmentsFilePath))
            {
                foreach (KeyValuePair<int, Appointment> pair in AllAppointments)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
            //Uploads the health records.
            using (StreamWriter sw = File.CreateText(HealthRecordsFilePath))
            {
                foreach (KeyValuePair<string, HealthRecord> pair in HealthRecords)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
            //Uploads the examinations.
            using (StreamWriter sw = File.CreateText(AppointmentsFilePath))
            {
                foreach (KeyValuePair<int, Appointment> pair in AllAppointments)
                {
                    newLine = pair.Value.Compress();
                    sw.WriteLine(newLine);
                }
            }
            //Uploads the referrals.
            using (StreamWriter sw = File.CreateText(ReferralsFilePath))
            {
                foreach (Referral referral in Referrals)
                {
                    newLine = referral.Compress();
                    sw.WriteLine(newLine);
                }
            }
            //Uploads the equipment requests.
            using (StreamWriter sw = File.CreateText(EquipmentRequestsFilePath))
            {
                foreach (EquipmentRequest equipmentRequest in EquipmentRequests)
                {
                    newLine = equipmentRequest.Compress();
                    sw.WriteLine(newLine);
                }
            }
            using (StreamWriter sw = File.CreateText(MedicineFilePath))
            {
                foreach (KeyValuePair<string, Medicine> pair in Medicine)
                {
                    newLine = pair.Value.Compress();
                    if (newLine != null)
                        sw.WriteLine(newLine);
                }
            }
        }
    }
}
