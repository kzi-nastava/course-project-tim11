using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.AdminFunctions
{
    public static class RoomRenovationService
    {
        static public List<RoomRenovation> RoomRenovationList { get; set; }

        static RoomRenovationService()
        {
            RoomRenovationList = RoomRenovationRepo.Load();
        }

        public static List<RoomRenovation> GetAll() => RoomRenovationList;

        public static RoomRenovation? Get(int id) => RoomRenovationList.FirstOrDefault(p => p.Id == id);

        public static void Add(RoomRenovation item)
        {
            if (RoomRenovationList.Count == 0)
            {
                item.Id = 1;
            }
            else
            {
                item.Id = RoomRenovationList.Last().Id + 1;
            }
            RoomRenovationList.Add(item);
            RoomRenovationRepo.PersistChanges();
        }
        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            RoomRenovationList.Remove(item);
            RoomRenovationRepo.PersistChanges();
        }

        public static void CommitComplexJoinRenovation(RoomRenovation renovation)
        {
            List<Equipment> equipmentInJoinedRoom = EquipmentService.GetEquipmentFromRoom(renovation.JoinedRoomId);
            foreach (var eq in equipmentInJoinedRoom)
            {
                EquipmentMovement movement = new EquipmentMovement { Amount = eq.Amount, EquipmentId = eq.Id, NewRoomId = renovation.RoomId, MovementDate = DateTime.Today };
                EquipmentMovementService.Add(movement);
                EquipmentMovementService.CheckForMovements();
                EquipmentService.Delete(eq.Id);
            }
            renovation.Done = true;
            RoomService.Delete(renovation.JoinedRoomId);
            RoomRenovationRepo.PersistChanges();
        }

        public static void CommitComplexSplitRenovation(RoomRenovation renovation)
        {

            RoomService.Add(renovation.NewRoom);
            renovation.Done = true;
            RoomRenovationRepo.PersistChanges();
        }

        public static void CheckForRenovations()
        {
            foreach (var renovation in RoomRenovationList)
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
