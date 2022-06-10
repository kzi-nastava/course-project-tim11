using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.Users
{
    public class DoctorService
    {
        public DoctorService()
        {
        }

        public static void ViewAllDoctors()
        {
            int i = 1;
            foreach (KeyValuePair<string, Doctor> entry in UserRepository.Doctors)
            {
                Doctor doctor = entry.Value;
                Console.WriteLine($"{i}.User name: {doctor.UserName} ; Name: {doctor.Name}; Last Name: {doctor.LastName}");
                i++;
            }
        }

        public static List<Doctor> SortDoctorsByFirstName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.Name.CompareTo(d2.Name);
            });
            return doctorsUnsorted;
        }

        public static List<Doctor> SortDoctorsByLastName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

        public static List<Doctor> SortDoctorsByField(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

        public static bool CheckAppointment(DateTime dateTime, int duration, ref Doctor doctor)
        {
            foreach (Appointment appointment in doctor.Appointments)
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

        public static void InsertAppointment(Appointment newAppointment, ref Doctor doctor)
        {
            if (doctor.Appointments.Count() == 0)
            {
                doctor.Appointments.Add(newAppointment);
                return;
            }
            for (int i = 0; i < doctor.Appointments.Count(); i++)
            {
                if (doctor.Appointments[i].DateTime < newAppointment.DateTime)
                {
                    doctor.Appointments.Insert(i, newAppointment);
                    return;
                }
            }
            doctor.Appointments.Add(newAppointment);

        }

        public static Appointment GetAppointmentByID(ref Doctor doctor)
        {
            bool quit = false;
            Appointment appointment = null;
            while (appointment == null)
            {
                CLI.CLIWriteLine("Enter the ID of the appointment you wish to edit?");
                int id = CLI.CLIEnterNumber();
                foreach (Appointment tmp in doctor.Appointments)
                {
                    if (tmp.ID == id){
                        appointment = tmp;
                        break;
                    }
                }
                if (appointment == null)
                {
                    CLI.CLIWriteLine($"No appointment matches ID: {id}");
                    quit = OtherFunctions.AskQuit();
                    if (quit) return null;
                }
            }
            return appointment;

        }

        //=======================================================================================================================================================================
        // VIEW SCHEDULE
        public static void ViewSchedule(ref Doctor doctor)
        {
            CLI.CLIWriteLine("Enter a date for which you wish to see your schedule (e.g. 22/10/1987): ");
            DateTime date = OtherFunctions.GetGoodDate();
            CLI.CLIWriteLine($"Appointments on date: {date.ToShortDateString()} and the next three days: \n");

            ShowAppointmentsByDate(date, ref doctor);

            string choice = "Y";
            while (choice.ToUpper() == "Y")
            {
                CLI.CLIWrite("Do you wish to view additional detail for any appointment?(y/n)\n>> ");
                choice = CLI.CLIEnterString();
                if (choice.ToUpper() == "Y")
                {
                    ViewAppointmentInfo();
                }
            }
            CLI.CLIWriteLine("Do you wish to perform an examination/operation(y/n)?");
            choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                AskPerform(ref doctor);
            }

        }
        private static void ShowAppointmentsByDate(DateTime date, ref Doctor doctor)
        {
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (date.Date <= appointment.DateTime.Date && appointment.DateTime.Date <= date.Date.AddDays(3))
                {
                    appointment.View();
                    CLI.CLIWriteLine();
                }
            }
        }
        private static void ViewAppointmentInfo()
        {
            
            CLI.CLIWrite("\n\nEnter the ID of the appointment you wish to view\n>> ");
            int id = CLI.CLIEnterNumber();
            Appointment chosenAppointment;
            if (!AppointmentRepo.CurrentAppointments.TryGetValue(id, out chosenAppointment))
            {
                CLI.CLIWriteLine("No appointment with that ID found");
                return;
            }
            CLI.CLIWriteLine("Searching for medical record");
            HealthRecord healthRecord = HealthRecordService.Search(chosenAppointment.Patient);
            CLI.CLIWriteLine("Information about patient:");
            PatientService.ViewPatient(healthRecord.Patient);
            HealthRecordService.ShowHealthRecord(healthRecord);

        }

        private static void AskPerform(ref Doctor doctor)
        {
            
            CLI.CLIWrite("\n\nEnter the ID of the appointment you wish to perform\n>> ");
            int id = CLI.CLIEnterNumber();

            Appointment chosenAppointment;
            if (!AppointmentRepo.CurrentAppointments.TryGetValue(id, out chosenAppointment))
            {
                Console.WriteLine("No appointment with that ID found");
                return;
            }
            Perform(chosenAppointment, ref doctor);

            
        }

        //=======================================================================================================================================================================
        // PERFORM EXAMINATION
        private static void Perform(Appointment appointment, ref Doctor doctor)
        {
            string type;
            if (appointment.Type == 'e') type = "Examination";
            else type = "Operation";

            CLI.CLIWriteLine($"{type} starting. Searching for medical record");
            HealthRecord healthRecord = HealthRecordService.Search(appointment.Patient);
            HealthRecordService.ShowHealthRecord(healthRecord);
            AnamnesisService.WriteAnamnesis(ref healthRecord, ref doctor);

            CLI.CLIWriteLine("\nDo you want to change medical record? (y/n)");
            string choice = CLI.CLIEnterString().ToUpper();
            if (choice == "Y")
            {
                HealthRecordService.ChangePatientRecord(ref healthRecord);
            }
            
            CLI.CLIWriteLine("Create referral for patient? (y/n)");
            choice = CLI.CLIEnterString().ToUpper();
            if (choice == "Y")
            {
                Patient patient = healthRecord.Patient;
                ReferralService.CreateReferral(ref patient, ref doctor);
            }

            do
            {
                CLI.CLIWriteLine("Write prescription for patient? (y/n)");
                choice = CLI.CLIEnterString().ToUpper();
                if (choice.ToUpper() == "Y")
                {
                    Prescription prescription = WritePrescription(healthRecord.Patient);
                    if (prescription != null)
                    {
                        PrescriptionRepo.Add(prescription, healthRecord.Patient);

                    }

                }
            } while (choice.ToUpper() == "Y");



            appointment.Finished = true;
            SystemFunctions.CurrentAppointments.Remove(appointment.ID);
            this.Appointments.Remove(appointment);

            appointment.Patient.Appointments.Remove(appointment);
            Console.WriteLine($"{type} ended.");
        }


        

        

        
        //=======================================================================================================================================================================
        // UPDATING EQUIPMENT AFTER AN APPOINTMENT

        public void UpdateEquipment()
        {
            Console.WriteLine($"The state of equipment in room {this.RoomId} before the appointment: ");
            Console.WriteLine();
            List<AdminFunctions.Equipment> equipmentList = EquipmentService.GetEquipmentFromRoom(this.RoomId);
            foreach (AdminFunctions.Equipment equipment in equipmentList)
            {
                Console.WriteLine($"{equipment.Name} : {equipment.Amount}");
            }
            Console.WriteLine($"Please enter the quantity of the equipment that was used during the appointment: ");
            Console.WriteLine();
            foreach (AdminFunctions.Equipment equipment in equipmentList)
            {

                var clinicEquipment = EquipmentRepo.Get(equipment.Id);
                Console.Write($"{equipment.Name} : ");
                int quantity = CLI.CLIEnterNumberWithLimit(-1, clinicEquipment.Amount + 1);
                int newQuantity = clinicEquipment.Amount - quantity;
                EquipmentRepo.Update(equipment.Id, newQuantity);
                Console.WriteLine();
            }
            Console.WriteLine("Succesfully updated equipment.");

        }
        //=======================================================================================================================================================================
        // MANAGING MEDICINE 

        public void ManageMedicine()
        {
            Console.WriteLine("Medicine requests: ");
            List<AdminFunctions.MedicineRequest> listRequests = AdminFunctions.MedicineRequestRepo.GetAll();
            foreach (AdminFunctions.MedicineRequest request in listRequests)
            {
                Console.WriteLine($"ID: {request.Id}");
                Console.WriteLine($"Name: {request.Medicine.Name}");
                Console.Write("Ingredients: ");
                foreach (string ingredient in request.Medicine.Ingredients)
                {
                    Console.Write(ingredient + ", ");
                }
                Console.WriteLine();

                Console.WriteLine("Do you want to approve this medicine(y/n)");
                string choice = Console.ReadLine();
                if (choice.ToUpper() == "Y")
                {
                    AdminFunctions.MedicineRequestService.Approve(request.Id);
                }
                else
                {
                    Console.WriteLine("Why do you want to reject this medicine? Write a short comment.");
                    string comment = Console.ReadLine();
                    AdminFunctions.MedicineRequestService.Reject(request.Id, comment);
                }

            }


        }


    }
}
