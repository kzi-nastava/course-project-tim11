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
    //---------------SEARCH AND FILTERING-------------------------------------------------------------
    public static List<Equipment> Search(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        var results = new List<Equipment>();
        foreach(var item in EquipmentRepo.ClinicEquipmentList)
        {
            if(item.Name.ToLower().Contains(searchTerm) || item.Type.ToString().ToLower().Contains(searchTerm) || RoomService.Get(item.RoomId).Name.ToLower().Contains(searchTerm))
            {
                results.Add(item);
            }
        }
        return results;
    }
    public static List<Equipment> FilterByEqType(List<Equipment> inputList, EquipmentType type)
    {
        var results = new List<Equipment>();
        foreach(var item in inputList)
        {
            if(item.Type == type)
            {
                results.Add(item);
            }
        }
        return results;
    }
    public static List<Equipment> FilterByRoomType(List<Equipment> inputList, RoomType type)
    {
        var results = new List<Equipment>();
        foreach(var item in inputList)
        {
            if(RoomRepo.Get(item.RoomId).Type == type)
            {
                results.Add(item);
            }
        }
        return results;
    }
    public static List<Equipment> FilterByNumbers(List<Equipment> inputList, int lowerBound, int upperBound)
    {
        var results = new List<Equipment>();
        foreach(var item in inputList)
        {
            if(item.Amount >= lowerBound && item.Amount <= upperBound)
            {
                results.Add(item);
            }
        }
        return results;
    }   
}