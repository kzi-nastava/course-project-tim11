using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ClinicApp.AdminFunctions;
using ClinicApp.Clinic;
using ClinicApp.HelperClasses;

namespace ClinicApp
{
    public class OtherFunctions
    {
        public static bool AskQuit()
        {
            CLI.CLIWriteLine("Do you wish to quit? (y/n)");
            string choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                return true;
            }
            else return false;
        }

        public static Fields AskField() {

            CLI.CLIWriteLine("\nChose specialization by number:\n");
            int i = 1;
            foreach (string field in Enum.GetNames(typeof(Fields)))
            {
                CLI.CLIWriteLine($"{i}. {field}");
                i++;
            }
            int choice = CLI.CLIEnterNumberWithLimit(1, Enum.GetNames(typeof(Fields)).Length);
            Fields specialization = (Fields)(choice - 1);
            return specialization;


        }      

        public static User FindUser(string text, Roles role = Roles.Nobody)
        {
            User user;
            int option = 1;

            while (option != 0)
            {
                CLI.CLIWriteLine(text);
                string userName = CLI.CLIEnterString();
                if (UserRepository.Users.TryGetValue(userName, out user))
                {
                    if (user.Role == role || role == Roles.Nobody)
                    {
                        return user;
                    }
                    else
                    {
                        CLI.CLIWriteLine("\nThis account doesn't belong to a " + role.ToString().ToLower() + ". Want to try again?");
                        CLI.CLIWriteLine("1: Yes");
                        CLI.CLIWriteLine("0: No");
                        option = CLI.CLIEnterNumberWithLimit(0, 1);
                    }
                }
                else
                {
                    CLI.CLIWriteLine("\nThere is no account with this username. Want to try again?");
                    CLI.CLIWriteLine("1: Yes");
                    CLI.CLIWriteLine("0: No");
                    option = CLI.CLIEnterNumberWithLimit(0, 1);
                }
            }

            return null;
        }

        public static User LogIn()
        {
            int option = 1;
            User user = new Nobody(), tempUser;
            while (option != 0)
            {
                CLI.CLIWrite("Username: ");
                string userName = CLI.CLIEnterString();
                CLI.CLIWrite("Password: ");
                string password = CLI.CLIEnterPassword();
                if (UserRepository.Users.TryGetValue(userName, out tempUser))
                {
                    if (tempUser.Password == password)
                    {
                        CLI.CLIWriteLine($"\nWelcome {tempUser.UserName}");
                        user = tempUser;
                        option = 0;
                    }
                    else
                    {
                        CLI.CLIWriteLine("\nIncorrect password. Want to try again?");
                        CLI.CLIWriteLine("1: Yes");
                        CLI.CLIWriteLine("0: No");
                        option = CLI.CLIEnterNumberWithLimit(0, 1);
                    }
                }
                else
                {
                    CLI.CLIWriteLine("\nUser does not exist. Want to try again?");
                    CLI.CLIWriteLine("1: Yes");
                    CLI.CLIWriteLine("0: No");
                    option = CLI.CLIEnterNumberWithLimit(0, 1);
                }
            }

            return user;
        }

        public static string Space(int length, string text)
        {
            string space = "";
            for (int i = 1; i <= length - text.Length; i++)
                space += " ";
            return space;
        }


        public static string LineInTable(bool withRole = false)
        {
            if (withRole)
                return "+----------------------+----------------------+----------------------+------------+-----------------+------------+";
            else
                return "+----------------------+----------------------+----------------------+------------+-----------------+";
        }

        public static string TableHeader(bool withRole = false)
        {
            if(withRole)
                return "| Username             | Name                 | Last Name            | Gender     | Date of Birth   | Role       |";
            else
                return "| Username             | Name                 | Last Name            | Gender     | Date of Birth   |";
        }

        public static void PrintUsers(bool withRole = false, Roles role = Roles.Nobody)
        {
            CLI.CLIWriteLine(LineInTable(withRole));
            CLI.CLIWriteLine(TableHeader(withRole));
            CLI.CLIWriteLine(LineInTable(withRole));
            foreach(KeyValuePair<string, User> pair in UserRepository.Users)
            {
                if(role == Roles.Nobody || pair.Value.Role == role)
                {
                    CLI.CLIWriteLine(pair.Value.TextInTable(withRole));
                    CLI.CLIWriteLine(LineInTable(withRole));
                }
            }
            CLI.CLIWriteLine();
        }

        public static DateTime GetGoodDate()
        {
            DateTime date;
            do
            {
                date = CLI.CLIEnterDate();
                if (date.Date < DateTime.Now.Date)
                {
                    CLI.CLIWriteLine("You can't enter a date that's in the past");
                }
            } while (date.Date < DateTime.Now.Date);
            return date;
        }

        public static bool CheckForRenovations(DateRange examinationTime, int roomId)
        {
            foreach(var renovation in RoomRenovationRepository.RoomRenovationList)
            {
                if (roomId == renovation.RoomId && renovation.Duration.IsOverlaping(examinationTime))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckForExaminations(DateRange dateRange, int roomId)
        {
            foreach (int examId in AppointmentRepo.AllAppointments.Keys )
            {
                Clinic.Appointment exam = AppointmentRepo.AllAppointments[examId];
                if(exam.Doctor.RoomId == roomId && dateRange.IsOverlaping(new DateRange(exam.DateTime, exam.DateTime.AddMinutes(15))))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ValidateDateTime(DateTime date) {
            if(date < DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public static Patient AskUsernamePatient()
        {
            CLI.CLIWriteLine("Enter the username of the patient. Do you want to view the list of all patients first (y/n)");
            CLI.CLIWrite(">> ");
            string choice = CLI.CLIEnterString();
            if (choice.ToUpper() == "Y")
            {
                PatientService.ViewAllPatients();
            }
            CLI.CLIWrite("\nEnter the username: ");
            string userName = CLI.CLIEnterString();
            Patient patient = null;
            if (!UserRepository.Patients.TryGetValue(userName, out patient))
            {
                CLI.CLIWriteLine("Patient with that username does not exist.");
            }
            return patient;
        }

        public static Doctor AskUsernameDoctor()
        {
            DoctorService.ViewAllDoctors();
            CLI.CLIWriteLine("\n Enter doctor username: ");
            CLI.CLIWrite(">> ");
            string userName = CLI.CLIEnterString();
            Doctor doctor = null;
            if (!UserRepository.Doctors.TryGetValue(userName, out doctor))
            {
                CLI.CLIWriteLine("Doctor with that username does not exist.");
            }
            return doctor;
        }
        public static void ListMedicineRequests()
        {
            {
                CLI.CLIWriteLine("These requests have been reviewed by a doctor and should be fixed up");
                foreach (var request in MedicineRequestRepository.GetAll())
                {
                    if (request.Comment != "")
                    {
                        CLI.CLIWriteLine("----------------------------------------------------------");
                        CLI.CLIWriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + WriteMedicineIngrediants(request.Medicine.Ingredients) +
                            "\nDoctor's comment: " + request.Comment);
                        CLI.CLIWriteLine("----------------------------------------------------------");
                    }
                }
            }
        }
        public static string WriteMedicineIngrediants(List<string> ingrediants)
        {
            string output = "";
            foreach (var ingr in ingrediants)
            {
                output += ingr + ", ";
            }
            return output;
        }
        public static RoomType ChooseRoomType()
        {
            RoomType type;
            int input = CLI.CLIEnterNumberWithLimit(1, 3);
            switch (input)
            {
                case 1:
                    type = RoomType.Operations;
                    break;
                case 2:
                    type = RoomType.Examinations;
                    break;
                case 3:
                    type = RoomType.Waiting;
                    break;
                default:
                    type = RoomType.STORAGE;
                    break;
            }
            return type;
        }
        public static EquipmentType ChooseEquipmentType()
        {
            EquipmentType type;
            int input = CLI.CLIEnterNumberWithLimit(1, 4);
            switch (input)
            {
                case 1:
                    type = EquipmentType.Operations;
                    break;
                case 2:
                    type = EquipmentType.RoomFurniture;
                    break;
                case 3:
                    type = EquipmentType.Hallway;
                    break;
                default:
                    type = EquipmentType.Examinations;
                    break;
            }
            return type;
        }
        public static List<string> ChooseIngrediants()
        {
            List<string> chosenIngrediants = new List<string>();
            List<string> offeredIngrediants = new List<string>(IngrediantRepository.GetAll());
            CLI.CLIWriteLine("Choose ingrediants, 0 to finish choosing");
            while (true)
            {
                foreach (var ingrediant in offeredIngrediants)
                {
                    CLI.CLIWriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
                }
                var choice = CLI.CLIEnterNumberWithLimit(0, offeredIngrediants.Count);
                if (choice == 0 && chosenIngrediants.Count > 0)
                {
                    break;
                }
                chosenIngrediants.Add(offeredIngrediants[choice - 1]);
                offeredIngrediants.Remove(offeredIngrediants[choice - 1]);
            }
            return chosenIngrediants;
        }
    }
}
