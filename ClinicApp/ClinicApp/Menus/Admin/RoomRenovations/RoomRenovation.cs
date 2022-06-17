using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;
using ClinicApp.Clinic.Rooms;
using ClinicApp.HelperClasses;

namespace ClinicApp.Menus.Admin.RoomRenovations
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
                        SimpleRoomRenovation.Dialog();
                        break;
                    case 2:
                        ComplexRoomRenovation.Menu();
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
        
        public static void ListAllRenovations()
        {
            string newLine;
            foreach (RoomRenovation renovation in RoomRenovationService.GetAll())
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
        
        
        
    }
}
