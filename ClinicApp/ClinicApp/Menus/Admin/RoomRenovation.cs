using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;
using ClinicApp.HelperClasses;

namespace ClinicApp.Menus.Admin
{
    class RoomRenovations
    {
        public static void Menu()
        {
            while (true)
            {
                RoomRenovationService.CheckForRenovations();
                CLI.CLIWriteLine("Room Renovation Menu");
                CLI.CLIWriteLine("1. Simple Renovation");
                CLI.CLIWriteLine("2. Complex Renovation");
                CLI.CLIWriteLine("3. List all Renovations");
                CLI.CLIWriteLine("0. Return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 3);
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        SimpleRoomRenovation();
                        break;
                    case 2:
                        ComplexRoomRenovationMenu();
                        break;
                    case 3:
                        ListAllRenovations();
                        break;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ComplexRoomRenovationMenu()
        {

            CLI.CLIWriteLine("1. Split room");
            CLI.CLIWriteLine("2. Join 2 rooms");
            CLI.CLIWriteLine("0. Return");
            int answer = CLI.CLIEnterNumberWithLimit(0, 2);
            switch (answer)
            {
                case 0:
                    return;
                case 1:
                    CreateComplexSplitRenovation();
                    return;
                case 2:
                    CreateComplexJoinRenovation();
                    return;
                default:
                    CLI.CLIWriteLine("Invalid option, try again");
                    break;
            }
        }
        public static void ListAllRenovations()
        {
            string newLine;
            foreach (RoomRenovation renovation in RoomRenovationRepository.GetAll())
            {
                switch (renovation.Type)
                {
                    case RenovationType.Simple:
                        newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type);
                        CLI.CLIWriteLine(newLine);
                        break;
                    case RenovationType.ComplexJoin:
                        newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type) + "| ID of joined room: " + Convert.ToString(renovation.JoinedRoomId);
                        CLI.CLIWriteLine(newLine);
                        break;
                    case RenovationType.ComplexSplit:
                        newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type) + "| new room will be: " + renovation.NewRoom.Name + "|" + Convert.ToString(renovation.NewRoom.Type);
                        CLI.CLIWriteLine(newLine);
                        break;
                }
            }
        }
        public static DateRange GetUninterruptedDateRange(int roomId)
        {
            while (true)
            {
                CLI.CLIWriteLine("Enter the start date of the renovation");
                DateTime start = CLI.CLIEnterDate();
                Console.WriteLine("Enter the end date of the renovation");
                DateTime end = CLI.CLIEnterDate();
                DateRange duration = new DateRange { StartDate = start, EndDate = end };
                duration.ValidateDates();
                if (OtherFunctions.CheckForExaminations(duration, roomId) == false)
                {
                    return duration;
                }
                CLI.CLIWriteLine("You have entered invalid dates, there is an examination during the date range!");
            }
        }
        public static void CreateComplexSplitRenovation()
        {
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = CLI.GetValidRoomId();
            if (id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(id);
            CLI.CLIWrite("Name of new room: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            CLI.CLIWrite("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = RoomService.ChooseRoomType();
            RoomRenovationService.CreateComplexSplit(name, roomType, id, duration);
        }
        public static void CreateComplexJoinRenovation()
        {
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = CLI.GetValidRoomId();
            if (id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(id);
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int otherRoomId = CLI.GetValidRoomId();
            if (otherRoomId == 0)
            {
                CLI.CLIWriteLine("You cannot delete Storage!");
                return;
            }
            if (OtherFunctions.CheckForExaminations(duration, otherRoomId))
            {
                CLI.CLIWriteLine("This room has an appointment at the given time, discarding");
                return;
            }

            RoomRenovationService.CreateComplexJoin(id, duration, otherRoomId);
        }
        public static void SimpleRoomRenovation()
        {

            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = CLI.GetValidRoomId();
            if (id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(id);
            RoomRenovationService.CreateSimpleRenovation(id, duration);
        }
    }
}
