using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;
using ClinicApp;

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
            CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepo.Get(eq.RoomId).Name + " " + RoomRepo.Get(eq.RoomId).Type + " " + eq.Type);
        }
    }
    public static void ListAllEquipmentInRoom(int id)
    {
        Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
        foreach (Equipment eq in EquipmentRepo.ClinicEquipmentList)
        {
            if (eq.RoomId == id)
                CLI.CLIWriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + RoomRepo.Get(eq.RoomId).Name + " " + RoomRepo.Get(eq.RoomId).Type + " " + eq.Type);
        }
    }
    public static int GetValidEquipmentId()
    {
        Equipment eq;
        int id = CLI.CLIEnterNumber();
        eq = EquipmentRepo.Get(id);
        while (eq is null)
        {
            CLI.CLIWriteLine("Invalid ID");
            id = CLI.CLIEnterNumber();
            eq = EquipmentRepo.Get(id);
        }
        return id;
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