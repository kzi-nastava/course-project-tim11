using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;
using System.Linq;

public static class RoomService {

    public static List<Room> ClinicRooms { get; set;}

    static RoomService()
    {
        ClinicRooms = RoomRepo.Load();        
    }
    public static List<Room> GetAll() => ClinicRooms;

    public static Room? Get(int id) => ClinicRooms.FirstOrDefault(p => p.Id == id);

    public static void Add(Room room)
    {
        room.Id = ClinicRooms.Last().Id + 1;
        ClinicRooms.Add(room);
        RoomRepo.PersistChanges();
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
        RoomRepo.PersistChanges();
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
        RoomRepo.PersistChanges();
    }
}