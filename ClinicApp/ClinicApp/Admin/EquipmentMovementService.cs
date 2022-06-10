﻿using System;
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
            Equipment eq = EquipmentRepository.Get(id);
            CLI.CLIWriteLine("Enter amount to move");
            int amount = CLI.CLIEnterNumberWithLimit(1, eq.Amount);
            CLI.CLIWriteLine("Enter the Id of the room where the equipment is going to");
            id = RoomService.GetValidRoomId();
            Room room = RoomRepository.Get(id);
            CLI.CLIWriteLine("Enter date on which the equipment is being moved");
            DateTime date = CLI.CLIEnterDate();
            EquipmentMovement movement = new EquipmentMovement { EquipmentId = eq.Id, Amount = amount, NewRoomId = room.Id, MovementDate = date, Done = false };
            EquipmentMovementRepository.Add(movement);
        }

        public static void CommitChanges(EquipmentMovement item)   //actually moves the equipment
        {
            Dictionary<string,int> newRoomEqNames = new Dictionary<string, int>();
            foreach (Equipment eq in EquipmentRepository.EquipmentList)
            {
                if (eq.RoomId == item.NewRoomId)
                {
                    newRoomEqNames.Add(eq.Name, eq.Id);
                }
            }
            if (newRoomEqNames.ContainsKey(EquipmentRepository.Get(item.EquipmentId).Name)) 
            {
                int eqId = newRoomEqNames[EquipmentRepository.Get(item.EquipmentId).Name];
                EquipmentRepository.Update(eqId, EquipmentRepository.Get(eqId).Amount + item.Amount);
            }
            else
            {
                Equipment newEq = new Equipment
                {
                    Name = EquipmentRepository.Get(item.EquipmentId).Name,
                    Amount = item.Amount,
                    RoomId = item.NewRoomId,
                    Type = EquipmentRepository.Get(item.EquipmentId).Type,
                    Dynamic = EquipmentRepository.Get(item.EquipmentId).Dynamic
                };

                EquipmentRepository.Add(newEq);
            }
            int remaining = EquipmentRepository.Get(item.EquipmentId).Amount - item.Amount;
            EquipmentRepository.Update(item.EquipmentId, remaining);
            item.Done = true;

        }
        
        public static void CheckForMovements() 
        {
            foreach(var item in EquipmentMovementRepository.EquipmentMovementList)
            {
                if (item.MovementDate <= DateTime.Today && item.Done == false)
                {
                    CommitChanges(item);
                }
            }
            EquipmentMovementRepository.PersistChanges();
        }       
    }
}
