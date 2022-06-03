using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class RoomService {

    public static List<Room> ClinicRooms { get; set;}

    static RoomService()
    {
        ClinicRooms = LoadRooms();        
    }
    public static List<Room> GetAll() => ClinicRooms;

    public static Room? Get(int id) => ClinicRooms.FirstOrDefault(p => p.Id == id);

    public static void Add(Room room)
    {
        room.Id = ClinicRooms.Last().Id + 1;
        ClinicRooms.Add(room);
        PersistRooms();
    }
    public static void Delete(int id)
    {
        var room = Get(id);
        if (room is null)
            return;
        if (id == 0)
        {
            return;
        }
        ClinicRooms.Remove(room);
        PersistRooms();
    }
    public static void Update(int id, string newName, RoomType newType)
    {
        var roomToUpdate = Get(id);
        if(roomToUpdate==null){
            return;
        }
        if (roomToUpdate.Type == RoomType.STORAGE)
        {
            return;
        }
        if(id == 0){
            return;
        }
        if(newType == RoomType.STORAGE){
            return;
        }
        roomToUpdate.Id = id;
        roomToUpdate.Name = newName;
        roomToUpdate.Type = newType;
        PersistRooms();
    }
    //-------------FILES STUFF----------------------------------------------
    public static void PersistRooms()
    {
        File.Delete("../../../Admin/Data/rooms.txt");
        foreach (Room room in ClinicRooms)
        {
            string newLine = Convert.ToString(room.Id) + "|" + room.Name + "|" + Convert.ToString(room.Type);
            using (StreamWriter sw = File.AppendText("../../../Admin/Data/rooms.txt"))
            {
                sw.WriteLine(newLine);
            }
        }

    }
    public static List<Room> LoadRooms()
    {
        List<Room> rooms = new List<Room>();
        using (StreamReader reader = new StreamReader("../../../Admin/Data/rooms.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Room room = ParseRoom(line);
                rooms.Add(room);
            }
        }
        return rooms;
    }
    static Room ParseRoom(string line)
    {
        string[] parameteres = line.Split("|");
        RoomType type = RoomType.STORAGE;
        switch (parameteres[2])
        {
            case "Operations":
                type = RoomType.Operations;
                break;
            case "Waiting":
                type = RoomType.Waiting;
                break;
            case "Examinations":
                type = RoomType.Examinations;
                break;
        }
        Room room = new Room { Id = Convert.ToInt32(parameteres[0]), Name = parameteres[1], Type = type };

        return room;
    }
}