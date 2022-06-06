using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;

public static class EquipmentService
{
    public static void AddToRoom(int eqId, int roomId)
    {
        var eq = EquipmentRepo.Get(eqId);
        if (eq is null)
            return;
        eq.RoomId = roomId;
        EquipmentRepo.PersistChanges();
    }
    public static void ListAllEquipment()
    {
        Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
        foreach (Equipment eq in EquipmentRepo.ClinicEquipmentList)
        {
            Console.WriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepo.Get(eq.RoomId).Name + " " + RoomRepo.Get(eq.RoomId).Type + " " + eq.Type);
        }
    }
    public static void ListAllEquipmentInRoom(int id)
    {
        Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
        foreach (Equipment eq in EquipmentRepo.ClinicEquipmentList)
        {
            if (eq.RoomId == id)
                Console.WriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepo.Get(eq.RoomId).Name + " " + RoomRepo.Get(eq.RoomId).Type + " " + eq.Type);
        }
    }

    public static List<Equipment> GetEquipmentFromRoom(int id)
    {
        List<Equipment> movements = new List<Equipment>();
        foreach (var eq in EquipmentRepo.ClinicEquipmentList)
        {
            if (eq.RoomId == id)
            {
                movements.Add(eq);
            }
        }
        return movements;
    }    
}