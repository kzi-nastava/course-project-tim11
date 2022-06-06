using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public static class EquipmentMovementService
    {
        public static void MoveEquipment() //menu for creating a new equipment movement 
        {
            CLI.CLIWriteLine("Enter ID of equipment to change:");
            int id = EquipmentService.GetValidEquipmentId();
            Equipment eq = EquipmentRepo.Get(id);
            CLI.CLIWriteLine("Enter amount to move");
            int amount = CLI.CLIEnterNumberWithLimit(1, eq.Amount);
            CLI.CLIWriteLine("Enter the Id of the room where the equipment is going to");
            id = RoomService.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            CLI.CLIWriteLine("Enter date on which the equipment is being moved");
            DateTime date = CLI.CLIEnterDate();
            EquipmentMovement movement = new EquipmentMovement { EquipmentId = eq.Id, Amount = amount, NewRoomId = room.Id, MovementDate = date, Done = false };
            EquipmentMovementRepo.Add(movement);
        }

        public static void CommitChanges(EquipmentMovement item)   //actually moves the equipment
        {
            Dictionary<string,int> newRoomEqNames = new Dictionary<string, int>();
            foreach (Equipment eq in EquipmentRepo.ClinicEquipmentList)
            {
                if (eq.RoomId == item.NewRoomId)
                {
                    newRoomEqNames.Add(eq.Name, eq.Id);
                }
            }
            if (newRoomEqNames.ContainsKey(EquipmentRepo.Get(item.EquipmentId).Name)) 
            {

                int eqId = newRoomEqNames[EquipmentRepo.Get(item.EquipmentId).Name];
                EquipmentRepo.Update(eqId, EquipmentRepo.Get(eqId).Amount + item.Amount);
            }
            else
            {
                Equipment newEq = new Equipment
                {
                    Name = EquipmentRepo.Get(item.EquipmentId).Name,
                    Amount = item.Amount,
                    RoomId = item.NewRoomId,
                    Type = EquipmentRepo.Get(item.EquipmentId).Type
                };

                EquipmentRepo.Add(newEq);
            }
            int remaining = EquipmentRepo.Get(item.EquipmentId).Amount - item.Amount;
            EquipmentRepo.Update(item.EquipmentId, remaining);
            item.Done = true;

        }
        
        public static void CheckForMovements() 
        {
            foreach(var item in EquipmentMovementRepo.EquipmentMovementList)
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
