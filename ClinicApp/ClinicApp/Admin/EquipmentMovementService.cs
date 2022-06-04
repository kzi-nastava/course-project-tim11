using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public static class EquipmentMovementService
    {
        static public List<EquipmentMovement> EquipmentMovementList { get; set; }

        static EquipmentMovementService()
        {
            EquipmentMovementList = EquipmentMovementRepo.Load();
        }
        public static List<EquipmentMovement> GetAll() => EquipmentMovementList;

        public static EquipmentMovement? Get(int id) => EquipmentMovementList.FirstOrDefault(p => p.Id == id);

        public static void Add(EquipmentMovement item)
        {
            if (EquipmentMovementList.Count == 0)
            {
                item.Id = 1;
            }
            else
            {
                item.Id = EquipmentMovementList.Last().Id + 1;
            }
            EquipmentMovementList.Add(item);
            EquipmentMovementRepo.PersistChanges();
        }
        public static void CommitChanges(EquipmentMovement item)   //actually moves the equipment
        {
            Dictionary<string,int> newRoomEqNames = new Dictionary<string, int>();
            foreach (Equipment eq in EquipmentService.ClinicEquipmentList)
            {
                if (eq.RoomId == item.NewRoomId)
                {
                    newRoomEqNames.Add(eq.Name, eq.Id);
                }
            }
            if (newRoomEqNames.ContainsKey(EquipmentService.Get(item.EquipmentId).Name)) 
            {

                int eqId = newRoomEqNames[EquipmentService.Get(item.EquipmentId).Name];
                EquipmentService.Update(eqId, EquipmentService.Get(eqId).Amount + item.Amount);
            }
            else
            {
                Equipment newEq = new Equipment
                {
                    Name = EquipmentService.Get(item.EquipmentId).Name,
                    Amount = item.Amount,
                    RoomId = item.NewRoomId,
                    Type = EquipmentService.Get(item.EquipmentId).Type
                };
                
                EquipmentService.Add(newEq);
            }
            int remaining = EquipmentService.Get(item.EquipmentId).Amount - item.Amount;
            EquipmentService.Update(item.EquipmentId, remaining);
            item.Done = true;

        }
        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            EquipmentMovementList.Remove(item);
            EquipmentMovementRepo.PersistChanges();
        }
        public static void CheckForMovements() 
        {
            foreach(var item in EquipmentMovementList)
            {
                if (item.MovementDate <= DateTime.Today && item.Done == false)
                {
                    CommitChanges(item);
                }
            }
            EquipmentMovementRepo.PersistChanges();
        }       
    }
}
