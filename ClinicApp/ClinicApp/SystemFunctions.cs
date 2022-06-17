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

        
        
        
        static public List<EquipmentRequest> EquipmentRequests { get; set; } = new List<EquipmentRequest>();

        // File paths

        
        
        public static string PatientRequestsFilePath = "../../../Data/patient_requests.txt";
        
       
        public static string EquipmentRequestsFilePath = "../../../Admin/Data/equipmentRequests.txt";



        // Loads the information from the database into objects and adds them to coresponding dictionaries
        public static void LoadData()
        {
            //Loads the users.
            UserRepository.Load();

            //Loads the messages.
            MessageBoxRepository.Load();

            //Loads the health records.
            HealthRecordRepo.Load();

            //Loads medicine.
            MedicineRepo.Load();

            //Loads perscriptions.
            PrescriptionRepo.Load();

            //Loads referrals.
            ReferralRepo.Load();

            //Loads requests for free days.
            FreeDaysRequestRepo.Load();

            //Loads the examinations.
            AppointmentRepo.LoadAppointments();

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

        


        //Updates certain information that depends on date and time
        public static void Update()
        {
            AdminFunctions.EquipmentMovementService.CheckForMovements(); //load to check if there is any equipment to move today
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

            //Uploads the health records.
            HealthRecordRepo.PersistChanges();

            //Uploads the appointments.
            AppointmentRepo.PresistChanges();

            //Uploads the medicine.
            MedicineRepo.PersistChanges();

            //Uploads the referrals.
            ReferralRepo.PersistChanges();

            //Uploads the free days request.
            FreeDaysRequestRepo.PersistChanges();

            //Uploads the equipment requests.
            using (StreamWriter sw = File.CreateText(EquipmentRequestsFilePath))
            {
                foreach (EquipmentRequest equipmentRequest in EquipmentRequests)
                {
                    newLine = equipmentRequest.Compress();
                    sw.WriteLine(newLine);
                }
            }
            
        }
    }
}
