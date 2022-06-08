using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;
using ClinicApp;

public static class EquipmentService
{
    public static void StorageSubmenu()
    {
        CLI.CLIWriteLine("1. Add new Equipment");
        CLI.CLIWriteLine("2. Edit amount of existing Equipment");
        int answer = CLI.CLIEnterNumberWithLimit(0, 2);
        switch (answer)
        {
            case 1:
                AddNewToStorage();
                break;
            case 2:
                EditExisting();
                break;
            case 0:
                return;
        }
    }
    public static void AddEqToStorage()
    {
        CLI.CLIWriteLine("List Equipment in Storage? (y/n): ");
        string answer = CLI.CLIEnterString();
        if (answer.ToLower() == "y")
        {
            ListAllEquipmentInRoom(0); //zero for storage
        }
        StorageSubmenu();
    }
    public static void EditExisting()
    {
        CLI.CLIWriteLine("Enter ID of equipment to change:");
        int id = GetValidEquipmentId();
        Equipment eq = EquipmentRepo.Get(id);
        if (eq.RoomId != 0)
        {
            CLI.CLIWriteLine("Equipment not in Storage cannot be edited directly, use the option 1. in the Manage Equipment menu");
            return;
        }
        else
        {
            CLI.CLIWriteLine("Enter new amount: ");
            int amount = CLI.CLIEnterNumberWithLimit(1, 99999999);
            EquipmentRepo.Update(eq.Id, amount);
        }
    }
    public static void AddNewToStorage()
    {
        string name;
        EquipmentType type;
        int amount = CLI.CLIEnterNumber();
        List<string> exsistingNames = new List<string>();
        foreach (Equipment item in EquipmentRepo.ClinicEquipmentList)
        {
            if (item.RoomId == 0)
            {
                exsistingNames.Add(item.Name);
            }
        }
        CLI.CLIWrite("Name: ");
        name = CLI.CLIEnterStringWithoutDelimiter("|");
        while (exsistingNames.Contains(name))
        {
            CLI.CLIWrite("Name already in use! Enter name: ");
            name = CLI.CLIEnterStringWithoutDelimiter("|");
        }
        CLI.CLIWriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
        type = ChooseEquipmentType();
        Equipment eq = new Equipment { Amount = amount, Name = name, RoomId = 0, Type = type };
        EquipmentRepo.Add(eq);
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
    public static EquipmentType ChooseEquipmentType()
    {
        EquipmentType type;
        int input = CLI.CLIEnterNumberWithLimit(1, 4);
        switch (input)
        {
            case 1:
                type = EquipmentType.Operations;
                break;
            case 2:
                type = EquipmentType.RoomFurniture;
                break;
            case 3:
                type = EquipmentType.Hallway;
                break;
            default:
                type = EquipmentType.Examinations;
                break;
        }
        return type;
    }
}