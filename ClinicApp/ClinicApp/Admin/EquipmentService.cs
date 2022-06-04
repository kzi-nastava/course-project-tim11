using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class EquipmentService 
{
    static public List<Equipment> ClinicEquipmentList { get; set; }

    static EquipmentService()
    {
        ClinicEquipmentList = EquipmentRepo.Load();
    }
    public static List<Equipment> GetAll() => ClinicEquipmentList;

    public static Equipment? Get(int id) => ClinicEquipmentList.FirstOrDefault(p => p.Id == id);

    public static void Add(Equipment eq)
    {
        eq.Id = ClinicEquipmentList.Last().Id + 1; 
        ClinicEquipmentList.Add(eq);
        EquipmentRepo.PersistChanges();
    }
    public static void Delete(int id)
    {
        var heq = Get(id);
        if (heq is null)
            return;
        ClinicEquipmentList.Remove(heq);
        EquipmentRepo.PersistChanges();
    }
    public static void AddToRoom(int eqId, int roomId)
    {
        var eq = Get(eqId);
        if (eq is null)
            return;
        eq.RoomId = roomId;
        EquipmentRepo.PersistChanges();
    }
    public static void Update(int id, int newAmount)
    {
        var eqToUpdate = Get(id);
        if (eqToUpdate == null)
        {
            return;
        }
        eqToUpdate.Amount = newAmount;
        EquipmentRepo.PersistChanges();
    }
    
    public static List<Equipment> GetEquipmentFromRoom(int id)
    {
        List<Equipment> movements = new List<Equipment>();
        foreach (var eq in ClinicEquipmentList)
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
        foreach(var item in ClinicEquipmentList)
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
            if(RoomService.Get(item.RoomId).Type == type)
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