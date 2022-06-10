using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.HelperClasses;

namespace ClinicApp.AdminFunctions
{
    public static class RoomRenovationService
    {
        public static void ListAllRenovations()
        {
            string newLine;
            foreach (RoomRenovation renovation in RoomRenovationRepo.GetAll())
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
            //get renovated room id
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = RoomService.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            if (room.Id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(room.Id);
            //create new room
            CLI.CLIWrite("Name of new room: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            CLI.CLIWrite("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = RoomService.ChooseRoomType();
            Room newRoom = new Room { Name = name, Type = roomType };
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = room.Id,
                Duration = duration,
                Type = RenovationType.ComplexSplit,
                Done = false,
                NewRoom = newRoom
            };
            RoomRenovationRepo.Add(renovation);
        }
        public static void CreateComplexJoinRenovation()
        {
            //get renovated room id
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = RoomService.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            if (room.Id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(room.Id);

            //get the joined room
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int otherId = RoomService.GetValidRoomId();
            Room otherRoom = RoomRepo.Get(otherId);
            if (room.Id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            if (otherRoom.Id == 0)
            {
                CLI.CLIWriteLine("You cannot delete Storage!");
                return;
            }
            if (OtherFunctions.CheckForExaminations(duration, otherRoom.Id))
            {
                CLI.CLIWriteLine("This room has an appointment at the given time, discarding");
                return;
            }
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = room.Id,
                Duration = duration,
                Type = RenovationType.ComplexJoin,
                Done = false,
                JoinedRoomId = otherRoom.Id,
            };
            RoomRenovationRepo.Add(renovation);
        }
        public static void SimpleRoomRenovation()
        {

            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = RoomService.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            if (room.Id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(room.Id);

            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = room.Id,
                Duration = duration,
                Type = RenovationType.Simple,
                Done = false
            };
            RoomRenovationRepo.Add(renovation);
        }
        public static void CommitComplexJoinRenovation(RoomRenovation renovation)
        {
            List<Equipment> equipmentInJoinedRoom = EquipmentService.GetEquipmentFromRoom(renovation.JoinedRoomId);
            foreach (var eq in equipmentInJoinedRoom)
            {
                EquipmentMovement movement = new EquipmentMovement { Amount = eq.Amount, EquipmentId = eq.Id, NewRoomId = renovation.RoomId, MovementDate = DateTime.Today };
                EquipmentMovementRepo.Add(movement);
                EquipmentMovementService.CheckForMovements();
                EquipmentRepo.Delete(eq.Id);
            }
            renovation.Done = true;
            RoomRepo.Delete(renovation.JoinedRoomId);
            RoomRenovationRepo.PersistChanges();
        }

        public static void CommitComplexSplitRenovation(RoomRenovation renovation)
        {

            RoomRepo.Add(renovation.NewRoom);
            renovation.Done = true;
            RoomRenovationRepo.PersistChanges();
        }

        public static void CheckForRenovations()
        {
            foreach (var renovation in RoomRenovationRepo.RoomRenovationList)
            {
                if (!renovation.Done)
                {
                    if (renovation.Duration.EndDate <= DateTime.Today)
                    {
                        switch (renovation.Type)
                        {
                            case RenovationType.Simple:
                                renovation.Done = true;
                                break;
                            case RenovationType.ComplexJoin:
                                CommitComplexJoinRenovation(renovation);
                                break;
                            case RenovationType.ComplexSplit:
                                CommitComplexSplitRenovation(renovation);
                                break;
                        }
                    }
                }
            }
            RoomRenovationRepo.PersistChanges();
        }
    }
}
