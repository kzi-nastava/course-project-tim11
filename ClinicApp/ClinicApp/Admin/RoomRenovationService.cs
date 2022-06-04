using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.AdminFunctions
{
    public static class RoomRenovationService
    {
        

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
