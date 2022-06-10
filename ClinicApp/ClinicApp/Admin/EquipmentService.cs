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

    //Makes an order for dynamic equipment.
    public static void OrderDynamiicEquipment()
    {
        bool gauzes = false, stiches = false, vaccines = false, bandages = false;
        foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
        {
            if (equipment.Amount > 0 && equipment.Type == EquipmentType.Gauzes && equipment.RoomId == 0)
                gauzes = true;
            if (equipment.Amount > 0 && equipment.Type == EquipmentType.Stiches && equipment.RoomId == 0)
                stiches = true;
            if (equipment.Amount > 0 && equipment.Type == EquipmentType.Vaccines && equipment.RoomId == 0)
                vaccines = true;
            if (equipment.Amount > 0 && equipment.Type == EquipmentType.Bandages && equipment.RoomId == 0)
                bandages = true;
        }
        if (gauzes == true && stiches == true && vaccines == true && bandages == true)
            CLI.CLIWriteLine("\nWe don't lack any equipment at the moment.");
        else
        {
            int numberOfOptions, option = 1;
            while (option != 0)
            {
                numberOfOptions = 0;
                CLI.CLIWriteLine("\nWhich of the following equipment would you like to order?");
                if (gauzes == false)
                {
                    numberOfOptions++;
                    CLI.CLIWriteLine(numberOfOptions + ": Gauzes");
                }
                if (stiches == false)
                {
                    numberOfOptions++;
                    CLI.CLIWriteLine(numberOfOptions + ": Stiches");
                }
                if (vaccines == false)
                {
                    numberOfOptions++;
                    CLI.CLIWriteLine(numberOfOptions + ": Vaccines");
                }
                if (bandages == false)
                {
                    numberOfOptions++;
                    CLI.CLIWriteLine(numberOfOptions + ": Bandages");
                }
                CLI.CLIWriteLine("0: Back to menu");
                option = CLI.CLIEnterNumberWithLimit(0, numberOfOptions);
                if (option != 0)
                {
                    CLI.CLIWriteLine("");
                    if (gauzes == false)
                    {
                        option--;
                        if (option == 0)
                        {
                            CLI.CLIWriteLine("How many gauzes would you like to order?");
                            option = CLI.CLIEnterNumberWithLimit(1, 1000);
                            EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Gauzes, option, DateTime.Now.Date);
                            SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                        }
                    }
                    if (stiches == false)
                    {
                        option--;
                        if (option == 0)
                        {
                            CLI.CLIWriteLine("How many stiches would you like to order?");
                            option = CLI.CLIEnterNumberWithLimit(1, 1000);
                            EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Stiches, option, DateTime.Now.Date);
                            SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                        }
                    }
                    if (vaccines == false)
                    {
                        option--;
                        if (option == 0)
                        {
                            CLI.CLIWriteLine("How many vaccines would you like to order?");
                            option = CLI.CLIEnterNumberWithLimit(1, 1000);
                            EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Vaccines, option, DateTime.Now.Date);
                            SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                        }
                    }
                    if (bandages == false)
                    {
                        option--;
                        if (option == 0)
                        {
                            CLI.CLIWriteLine("How many bandages would you like to order?");
                            option = CLI.CLIEnterNumberWithLimit(1, 1000);
                            EquipmentRequest equipmentRequest = new EquipmentRequest(EquipmentType.Bandages, option, DateTime.Now.Date);
                            SystemFunctions.EquipmentRequests.Add(equipmentRequest);
                        }
                    }
                    //In the end, option will still be greater than 0
                }
            }
        }
    }

    //Redistributes dynamic equipment
    public static void RedistributeDynamiicEquipment()
    {
        foreach (Room room in RoomRepo.ClinicRooms)
        {
            int gauzes = 0, stiches = 0, vaccines = 0, bandages = 0;
            foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
            {
                if (equipment.Type == EquipmentType.Gauzes && equipment.RoomId == room.Id)
                    gauzes += equipment.Amount;
                if (equipment.Type == EquipmentType.Stiches && equipment.RoomId == room.Id)
                    stiches += equipment.Amount;
                if (equipment.Type == EquipmentType.Vaccines && equipment.RoomId == room.Id)
                    vaccines += equipment.Amount;
                if (equipment.Type == EquipmentType.Bandages && equipment.RoomId == room.Id)
                    bandages += equipment.Amount;
            }
            if (gauzes < 5 || stiches < 5 || vaccines < 5 || bandages < 5)
            {
                CLI.CLIWriteLine("\nRoom id: " + room.Id);
                CLI.CLIWriteLine("Room name: " + room.Name);
                if (gauzes == 0)
                    CLI.CLIWriteLine("-Gauzes: " + gauzes);
                else if (gauzes < 5)
                    CLI.CLIWriteLine(" Gauzes: " + gauzes);
                if (stiches == 0)
                    CLI.CLIWriteLine("-Stiches: " + stiches);
                else if (stiches < 5)
                    CLI.CLIWriteLine(" Stiches: " + stiches);
                if (vaccines == 0)
                    CLI.CLIWriteLine("-Vaccines: " + vaccines);
                else if (vaccines < 5)
                    CLI.CLIWriteLine(" Vaccines: " + vaccines);
                if (bandages == 0)
                    CLI.CLIWriteLine("-Bandages: " + bandages);
                else if (bandages < 5)
                    CLI.CLIWriteLine(" Bandages: " + bandages);
            }
        }

        int option = 1;
        while (option != 0)
        {
            CLI.CLIWriteLine("\nDo you want to move equipment?");
            CLI.CLIWriteLine("1: Yes");
            CLI.CLIWriteLine("0: No");
            option = CLI.CLIEnterNumberWithLimit(0, 1);
            if (option == 1)
            {
                int idFrom, idTo, amount, totalEquipment = 0;
                EquipmentType type;
                Room roomFrom, roomTo;
                CLI.CLIWriteLine("\nEnter the id of the room from which you want to move dynamic equipment:");
                idFrom = CLI.CLIEnterNumber();
                idTo = CLI.CLIEnterNumber();
                amount = CLI.CLIEnterNumber();
                CLI.CLIWriteLine("\nWhich of the following equipment would you like to move?");
                CLI.CLIWriteLine("1: Gauzes");
                CLI.CLIWriteLine("2: Stiches");
                CLI.CLIWriteLine("3: Vaccines");
                CLI.CLIWriteLine("4: Bandages");
                option = CLI.CLIEnterNumberWithLimit(1, 4);
                switch (option)
                {
                    case 1:
                        type = EquipmentType.Gauzes;
                        break;
                    case 2:
                        type = EquipmentType.Stiches;
                        break;
                    case 3:
                        type = EquipmentType.Vaccines;
                        break;
                    default:
                        type = EquipmentType.Bandages;
                        break;
                }
                roomFrom = RoomRepo.Get(idFrom);
                if (roomFrom == default)
                    roomFrom = RoomRepo.Get(0);
                roomTo = RoomRepo.Get(idTo);
                if (roomTo == default)
                    roomTo = RoomRepo.Get(0);
                foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
                {
                    if (equipment.Type == type && equipment.RoomId == roomFrom.Id)
                        totalEquipment += equipment.Amount;
                }
                if (amount > totalEquipment)
                    amount = totalEquipment;
                Equipment equipmentNew = new Equipment
                {
                    Id = 0,
                    Name = type.ToString(),
                    Amount = amount,
                    RoomId = roomTo.Id,
                    Type = type
                };
                EquipmentRepo.Add(equipmentNew);
                foreach (Equipment equipment in EquipmentRepo.ClinicEquipmentList)
                    if (equipment.Type == type && equipment.RoomId == roomTo.Id && amount > 0)
                        if (amount < equipment.Amount)
                        {
                            equipment.Amount -= amount;
                            amount = 0;
                        }
                        else
                        {
                            amount -= equipment.Amount;
                            EquipmentRepo.ClinicEquipmentList.Remove(equipment);
                        }
                EquipmentRepo.PersistChanges();
            }
        }
    }
}