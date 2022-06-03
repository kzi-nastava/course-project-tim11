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
        ClinicEquipmentList = LoadEquipment();
                
    }
    public static List<Equipment> GetAll() => ClinicEquipmentList;

    public static Equipment? Get(int id) => ClinicEquipmentList.FirstOrDefault(p => p.Id == id);

    public static void Add(Equipment eq)
    {
        eq.Id = ClinicEquipmentList.Last().Id + 1; 
        ClinicEquipmentList.Add(eq);
        PersistEquipment();
    }
    public static void Delete(int id)
    {
        var heq = Get(id);
        if (heq is null)
            return;
        ClinicEquipmentList.Remove(heq);
        PersistEquipment();
    }
    public static void AddToRoom(int eqId, int roomId)
    {
        var eq = Get(eqId);
        if (eq is null)
            return;
        eq.RoomId = roomId;
        PersistEquipment();
    }
    public static void Update(int id, int newAmount)
    {
        var eqToUpdate = Get(id);
        if (eqToUpdate == null)
        {
            return;
        }
        eqToUpdate.Amount = newAmount;
        PersistEquipment();
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
    //--------------FILES STUFF-----------------
    public static void PersistEquipment()
    {
        File.Delete("../../../Admin/Data/equipment.txt");
        foreach (Equipment eq in ClinicEquipmentList)
        {
            string newLine = Convert.ToString(eq.Id) + "|" + eq.Name + "|" + Convert.ToString(eq.Amount) + "|" + Convert.ToString(eq.RoomId) + "|" + Convert.ToString(eq.Type);
            using (StreamWriter sw = File.AppendText("../../../Admin/Data/equipment.txt"))
            {
                sw.WriteLine(newLine);
            }
        }

    }
    public static List<Equipment> LoadEquipment()
    {
        List<Equipment> eqList = new List<Equipment>();
        using (StreamReader reader = new StreamReader("../../../Admin/Data/equipment.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Equipment eq = ParseEquipment(line);
                eqList.Add(eq);
            }
        }
        return eqList;
    }
    static Equipment ParseEquipment(string line)
    {
        string[] parameteres = line.Split("|");
        EquipmentType type = EquipmentType.Examinations;
        switch (parameteres[4])
        {
            case "Operations":
                type = EquipmentType.Operations;
                break;
            case "Hallway":
                type = EquipmentType.Hallway;
                break;
            case "RoomFurniture":
                type = EquipmentType.RoomFurniture;
                break;
            case "Examinations":
                type = EquipmentType.Examinations;
                break;
        }
        Equipment eq = new Equipment { 
            Id = Convert.ToInt32(parameteres[0]), 
            Name = parameteres[1], 
            Amount = Convert.ToInt32(parameteres[2]), 
            RoomId = Convert.ToInt32(parameteres[3]), 
            Type = type };

        return eq;
    }
}