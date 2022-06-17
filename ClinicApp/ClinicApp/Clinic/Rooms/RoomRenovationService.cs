using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.HelperClasses;

namespace ClinicApp.Clinic
{
    public static class RoomRenovationService
    {
        public static List<RoomRenovation> GetAll()
        {
            return RoomRenovationRepository.GetAll();
        }
        public static void CreateComplexSplit(string name, RoomType roomType, int id, DateRange duration)
        {
            Room newRoom = new Room { Name = name, Type = roomType };
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = id,
                Duration = duration,
                Type = RenovationType.ComplexSplit,
                Done = false,
                NewRoom = newRoom
            };
            RoomRenovationRepository.Add(renovation);
        }
        public static void CreateComplexJoin(int id, DateRange duration, int otherRoomId)
        {
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = id,
                Duration = duration,
                Type = RenovationType.ComplexJoin,
                Done = false,
                JoinedRoomId = otherRoomId,
            };
            RoomRenovationRepository.Add(renovation);
        }
        public static void CreateSimpleRenovation(int id, DateRange duration)
        {
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = id,
                Duration = duration,
                Type = RenovationType.Simple,
                Done = false
            };
            RoomRenovationRepository.Add(renovation);
        }
        public static void CommitComplexJoinRenovation(RoomRenovation renovation)
        {
            List<Equipment> equipmentInJoinedRoom = EquipmentService.GetEquipmentFromRoom(renovation.JoinedRoomId);
            foreach (var eq in equipmentInJoinedRoom)
            {
                EquipmentMovement movement = new EquipmentMovement { Amount = eq.Amount, EquipmentId = eq.Id, NewRoomId = renovation.RoomId, MovementDate = DateTime.Today };
                EquipmentMovementRepository.Add(movement);
                EquipmentMovementService.CheckForMovements();
                EquipmentRepository.Delete(eq.Id);
            }
            renovation.Done = true;
            RoomRepository.Delete(renovation.JoinedRoomId);
            RoomRenovationRepository.PersistChanges();
        }

        public static void CommitComplexSplitRenovation(RoomRenovation renovation)
        {

            RoomRepository.Add(renovation.NewRoom);
            renovation.Done = true;
            RoomRenovationRepository.PersistChanges();
        }

        public static void CheckForRenovations()
        {
            foreach (var renovation in RoomRenovationRepository.RoomRenovationList)
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
            RoomRenovationRepository.PersistChanges();
        }
    }
}
