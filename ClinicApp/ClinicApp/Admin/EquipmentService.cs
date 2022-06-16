using ClinicApp.AdminFunctions;
using System;
using System.Collections.Generic;
using ClinicApp;
using ClinicApp.Users;

public static class EquipmentService
{
    
    
    public static void AddNew(string name, int amount, EquipmentType type)
    {
        Equipment eq = new Equipment { Amount = amount, Name = name, RoomId = 0, Type = type };
        EquipmentRepository.Add(eq);
    }
    
    public static List<Equipment> GetEquipmentFromRoom(int id)
    {
        List<Equipment> movements = new List<Equipment>();
        foreach (var eq in EquipmentRepository.EquipmentList)
        {
            if (eq.RoomId == id)
            {
                movements.Add(eq);
            }
        }
        return movements;
    }
    public static Equipment Get(int id)
    {
        return EquipmentRepository.Get(id);
    }
    public static List<Equipment> GetAll()
    {
        return EquipmentRepository.GetAll();
    }
    public static void Update(int id, int amount)
    {
        EquipmentRepository.Update(id, amount);
    }
    //Makes an order for dynamic equipment.
    public static void OrderDynamiicEquipment()
    {
        bool gauzes = false, stiches = false, vaccines = false, bandages = false;
        foreach (Equipment equipment in EquipmentRepository.EquipmentList)
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
        foreach (Room room in RoomRepository.Rooms)
        {
            int gauzes = 0, stiches = 0, vaccines = 0, bandages = 0;
            foreach (Equipment equipment in EquipmentRepository.EquipmentList)
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
                roomFrom = RoomRepository.Get(idFrom);
                if (roomFrom == default)
                    roomFrom = RoomRepository.Get(0);
                roomTo = RoomRepository.Get(idTo);
                if (roomTo == default)
                    roomTo = RoomRepository.Get(0);
                foreach (Equipment equipment in EquipmentRepository.EquipmentList)
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
                EquipmentRepository.Add(equipmentNew);
                foreach (Equipment equipment in EquipmentRepository.EquipmentList)
                    if (equipment.Type == type && equipment.RoomId == roomTo.Id && amount > 0)
                        if (amount < equipment.Amount)
                        {
                            equipment.Amount -= amount;
                            amount = 0;
                        }
                        else
                        {
                            amount -= equipment.Amount;
                            EquipmentRepository.EquipmentList.Remove(equipment);
                        }
                EquipmentRepository.PersistChanges();
            }
        }

    }
    public static void UpdateRoomEquipment(Doctor doctor)
    {
        CLI.CLIWriteLine($"\nThe state of equipment in room {doctor.RoomId} before the appointment: ");
        CLI.CLIWriteLine();
        List<Equipment> equipmentList = GetEquipmentFromRoom(doctor.RoomId);
        foreach (Equipment equipment in equipmentList)
        {
            CLI.CLIWriteLine($"{equipment.Name} : {equipment.Amount}");
        }
        CLI.CLIWriteLine($"Please enter the quantity of the equipment that was used during the appointment: ");
        CLI.CLIWriteLine();
        foreach (Equipment equipment in equipmentList)
        {

            var clinicEquipment = EquipmentRepository.Get(equipment.Id);
            CLI.CLIWrite($"{equipment.Name} : ");
            int quantity = CLI.CLIEnterNumberWithLimit(-1, clinicEquipment.Amount + 1);
            int newQuantity = clinicEquipment.Amount - quantity;
            EquipmentRepository.Update(equipment.Id, newQuantity);
            CLI.CLIWriteLine();
        }
        CLI.CLIWriteLine("Succesfully updated equipment.");

    }
}