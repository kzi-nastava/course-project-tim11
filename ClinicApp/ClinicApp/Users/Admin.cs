 ﻿using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Admin;

namespace ClinicApp.Users
{
    public class Admin
    {
        public static void AdminMenu() 
        {
            while (true) { 
                Console.WriteLine("Admin menu, choose an option");
                Console.WriteLine("1. Manage Clinic Rooms");
                Console.WriteLine("2. Manage Clinic Equipment");
                Console.WriteLine("X to exit");
                string choice = Console.ReadLine();
                switch (choice.ToUpper())
                {
                    case "1":
                        RoomManagmentMenu();
                        break;
                    case "2":
                        EquipmentManagmentMenu();
                        break;
                    case "X":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void RoomManagmentMenu() 
        {
            while (true) { 
                Console.WriteLine("Manage Rooms");
                Console.WriteLine("1. List all rooms");
                Console.WriteLine("2. Add new room");
                Console.WriteLine("3. Edit an existing room");
                Console.WriteLine("4. Delete a room by ID");
                Console.WriteLine("X to return");
                string choice = Console.ReadLine();
                switch (choice.ToUpper())
                {
                    case "1":
                        ListAllRooms();
                        break;
                    case "2":
                        AddNewRoom();
                        break;
                    case "3":
                        EditRoom();
                        break;
                    case "4":
                        DeleteRoom();
                        break;
                    case "X":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        //-------------------------------------MANAGE ROOMS----------------------------------------
        public static void ListAllRooms() 
        {
            Console.WriteLine("ID | NAME | TYPE");
            foreach (ClinicRoom room in ClinicRoomService.ClinicRooms)
            {
                Console.WriteLine(room.Id + " " + room.Name + " " + room.Type);
            }
        }
        public static void AddNewRoom() 
        {
            string name;
            string type;
            RoomType roomType;
            while (true)
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
                if (name.Contains("|")) 
                {
                    Console.WriteLine("Invalid option, name cannot contain |, try again");
                }
                else { break; }
            }
            while (true) 
            { 
                Console.Write("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
                type = Console.ReadLine();
                if (type == "1")
                {
                    roomType = RoomType.Operations;
                    break;
                }
                else if (type == "2")
                {
                    roomType = RoomType.Examinations; break;
                }
                else if (type == "3")
                {
                    roomType = RoomType.Waiting;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                    type = Console.ReadLine();
                }
                        
            }
            ClinicRoom room = new ClinicRoom { Name = name, Type = roomType };
            ClinicRoomService.Add(room);
        }
        public static void EditRoom()
        {
            ClinicRoom room;
            string name;
            string type;
            RoomType roomType;
            while (true) 
            {
                Console.WriteLine("Enter ID of the room you want to Edit");
                int id = Convert.ToInt32(Console.ReadLine());
                room = ClinicRoomService.Get(id);
            if (room is null)
            {
                Console.WriteLine("Invalid option, try again");
            }
            else break;
            }
            if (room.Id == 0)
            {
                Console.WriteLine("You cannot edit Storage!");
                return;
            }
            while (true)
            {
                Console.WriteLine("Enter new name, leave empty for old");
                name = Console.ReadLine();
                if (name.Contains("|"))
                {
                    Console.WriteLine("Invalid option, name cannot contain |, try again");
                }
                else { break; }
            }
            if (name == "") 
            {
                name = room.Name;
            }
            while (true)
            {
                Console.Write("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting), leave empty for old: ");
                type = Console.ReadLine();
                if (type == "1")
                {
                    roomType = RoomType.Operations;
                    break;
                }
                else if (type == "2")
                {
                    roomType = RoomType.Examinations;
                    break;
                }
                else if (type == "3")
                {
                    roomType = RoomType.Waiting;
                    break;
                }
                else if (type == "")
                {
                    roomType = room.Type;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                }
            }
            ClinicRoomService.Update(room.Id, name, roomType);
        }
        public static void DeleteRoom()
        {
            ClinicRoom room;
            int id;
            while (true)
            {
                Console.WriteLine("Enter ID of the room you want to Delete");
                id = Convert.ToInt32(Console.ReadLine());
                room = ClinicRoomService.Get(id);
                if (room is null)
                {
                    Console.WriteLine("Invalid option, try again");
                }
                else break;
            }
            if(id == 0)
            {
                Console.WriteLine("You cannot delete Storage!");
            }
            else ClinicRoomService.Delete(id);

        }
        //------------------------------------------------------MANAGE EQUIPMENT------------------------------------------
        public static void EquipmentManagmentMenu() 
        {
            while(true)
            {
                Console.WriteLine("Manage Equipment");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. Search");
                Console.WriteLine("3. Move equipment");
                string choice = Console.ReadLine();
                switch (choice.ToUpper())
                {
                    case "1":
                        ListAllEquipment();
                        break;
                    case "2":
                        SearchEquipment();
                        break;
                    case "3":
                        MoveEquipment();
                        break;
                    case "X":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ListAllEquipment()
        {
            Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
            foreach (ClinicEquipment eq in ClinicEquipmentService.ClinicEquipmentList)
            {
                Console.WriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + ClinicRoomService.Get(eq.RoomId).Name + " " + ClinicRoomService.Get(eq.RoomId).Type + " " + eq.Type);
            }
        }
        public static void SearchEquipment()
        {
            Console.WriteLine("Search");
            SearchTerms STerms = new SearchTerms();
            List<ClinicEquipment> Results;
            while (true)
            {
                Console.Write("Enter search terms: ");
                STerms.SearchTerm = Console.ReadLine();
                if (STerms.SearchTerm is null)
                {
                    Console.WriteLine("Invalid option, try again");
                }
                else
                {
                    break;
                }
            }
            
            Results = ClinicEquipmentService.Search(STerms.SearchTerm);

            while (true)
            {
                Console.WriteLine("\nFilter by Equipment Type? (y/n): ");
                string eq = Console.ReadLine();
                if (eq.ToLower() == "y")
                {
                    STerms.FilterByEqTypeBool = true;
                    while (true)
                    {
                        Console.WriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
                        string eqType = Console.ReadLine();
                        if (eqType == "1")
                        {
                            STerms.FilterByEq = EquipmentType.Operations;
                            break;
                        }
                        else if (eqType == "2")
                        {
                            STerms.FilterByEq = EquipmentType.RoomFurniture;
                            break;
                        }
                        else if (eqType == "3")
                        {
                            STerms.FilterByEq = EquipmentType.Hallway;
                            break;
                        }
                        else if (eqType == "4")
                        {
                            STerms.FilterByEq = EquipmentType.Examinations;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option, try again");
                        }

                    }
                 
                    break;
                }
                else if (eq.ToLower() == "n")
                {
                    STerms.FilterByEqTypeBool = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                }
            }
            while (true)
            {
                Console.WriteLine("Filter by room type?(y/n): ");
                string room = Console.ReadLine();
                if (room.ToLower() == "y")
                {
                    STerms.FilterByRoomTypeBool = true;
                    while (true)
                    {
                        Console.WriteLine("Choose!\n1. Operations\n2. Waiting\n3. STORAGE\n4. Examinations");
                        string roomType = Console.ReadLine();
                        if (roomType == "1")
                        {
                            STerms.FilterByRoom = RoomType.Operations;
                            break;
                        }
                        else if (roomType == "2")
                        {
                            STerms.FilterByRoom = RoomType.Waiting;
                            break;
                        }
                        else if (roomType == "3")
                        {
                            STerms.FilterByRoom = RoomType.STORAGE;
                            break;
                        }
                        else if (roomType == "4")
                        {
                            STerms.FilterByRoom = RoomType.Examinations;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option, try again");
                        }

                    }
                    break;
                }
                else if (room.ToLower() == "n")
                {
                    STerms.FilterByRoomTypeBool = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                }
                
            }
            while (true)
            {
                Console.WriteLine("Filter by amount?(y/n): ");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    STerms.FilterByAmountBool = true;
                    while (true)
                    {
                        Console.WriteLine("Choose!\n1. 0\n2. 1-10\n3. 10+");
                        answer = Console.ReadLine();
                        if (answer == "1")
                        {
                            STerms.STAmount = 1;
                            break;
                        }
                        else if (answer == "2")
                        {
                            STerms.STAmount = 2;
                            break;
                        }
                        else if (answer == "3")
                        {
                            STerms.STAmount = 3;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option, try again");
                        }
                        
                    }
                    break;
                }
                else if( answer.ToLower() == "n")
                {
                    STerms.FilterByAmountBool = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                }
            }
            if (STerms.FilterByEqTypeBool == true)
            {
                Results = ClinicEquipmentService.FilterByEqType(Results, STerms.FilterByEq);
            }
            if (STerms.FilterByRoomTypeBool == true)
            {
                Results = ClinicEquipmentService.FilterByRoomType(Results, STerms.FilterByRoom);
            }
            if (STerms.FilterByAmountBool == true)
            {
                switch (STerms.STAmount)
                {
                    case 1:
                        Results = ClinicEquipmentService.FilterByNumbers(Results, 0, 0);
                        break;
                    case 2:
                        Results = ClinicEquipmentService.FilterByNumbers(Results, 1, 10);
                        break;
                    case 3:
                        Results = ClinicEquipmentService.FilterByNumbers(Results, 11, 10000000);
                        break;
                }
            }
            Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
            foreach (ClinicEquipment eq in Results)
            {
                Console.WriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + ClinicRoomService.Get(eq.RoomId).Name + " " + ClinicRoomService.Get(eq.RoomId).Type + " " + eq.Type);
            }

        }
        public static void MoveEquipment()
        {
            Console.WriteLine("Moving");
        }
    }
    public class SearchTerms
    {
        public string SearchTerm { get; set; }
        public bool FilterByEqTypeBool { get; set; }
        public EquipmentType FilterByEq { get; set; }
        public bool FilterByAmountBool { get; set; }
        public int STAmount { get; set; }
        public bool FilterByRoomTypeBool { get; set; }
        public RoomType FilterByRoom { get; set; }
    }
}