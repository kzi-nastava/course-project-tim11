﻿using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Users
{
    public class Admin : User
    {
        public Admin(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Admin;
            MessageBox = new MessageBox(this);
        }

        public Admin(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Admin;
            MessageBox = new MessageBox(this);
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        public override int MenuWrite()
        {
            EquipmentMovementService.CheckForMovements(); //load to check if there is any equipment to move today
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage Clinic Rooms");
            Console.WriteLine("4: Manage Clinic Equipment");
            Console.WriteLine("5: Manage Room Renovations");
            Console.WriteLine("6: Manage Medicines");
            Console.WriteLine("0: Exit");

            return 6;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    MessageBox.DisplayMessages();
                    break;
                case 3:
                    RoomManagmentMenu();
                    break;
                case 4:
                    EquipmentManagmentMenu();
                    break;
                case 5:
                    RoomRenovationMenu();
                    break;
                case 6:
                    MedicinesMenu();
                    break;
            }
        }
        //-------------------------------------MANAGE ROOMS----------------------------------------
        public static void RoomManagmentMenu()
        {
            while (true)
            {
                CLI.CLIWriteLine("Manage Rooms");
                CLI.CLIWriteLine("1. List all rooms");
                CLI.CLIWriteLine("2. Add new room");
                CLI.CLIWriteLine("3. Edit an existing room");
                CLI.CLIWriteLine("4. Delete a room by ID");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0,4);
                switch (choice)
                {
                    case 1:
                        RoomService.ListAllRooms();
                        break;
                    case 2:
                        RoomService.AddNewRoom();
                        break;
                    case 3:
                        RoomService.EditRoom();
                        break;
                    case 4:
                        RoomService.DeleteRoom();
                        break;
                    case 0:
                        return;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        //------------------------------------------------------MANAGE EQUIPMENT------------------------------------------
        public static void EquipmentManagmentMenu()
        {
            while (true)
            {
                EquipmentMovementService.CheckForMovements();
                CLI.CLIWriteLine("Manage Equipment");
                CLI.CLIWriteLine("1. List all");
                CLI.CLIWriteLine("2. Search");
                CLI.CLIWriteLine("3. Move equipment");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0,3);
                switch (choice)
                {
                    case 1:
                        EquipmentService.ListAllEquipment();
                        break;
                    case 2:
                        EquipmentSearchService.SearchEquipment();
                        break;
                    case 3:
                        MoveEquipmentMenu();
                        break;
                    case 0:
                        return;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void MoveEquipmentMenu()
        {
            while (true)
            {
                CLI.CLIWriteLine("1. Move Equipment");
                CLI.CLIWriteLine("2. Add new Equipment to Storage");
                CLI.CLIWriteLine("0 to return");
                int answer = CLI.CLIEnterNumberWithLimit(0, 2);
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        EquipmentMovementService.MoveEquipment();
                        break;
                    case 2:
                        AddEqToStorage();
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void AddEqToStorage()
        {
            Console.WriteLine("List Equipment in Storage? (y/n): ");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                EquipmentService.ListAllEquipmentInRoom(0); //zero for storage
            }

            while (true) //storage submenu
            {
                Console.WriteLine("1. Add new Equipment");
                Console.WriteLine("2. Edit amount of existing Equipment");
                answer = Console.ReadLine();
                if (answer == "1")
                {
                    string name;
                    EquipmentType type;
                    int amount = OtherFunctions.EnterNumber();
                    List<string> exsistingNames = new List<string>();
                    foreach (Equipment item in EquipmentRepo.ClinicEquipmentList)
                    {
                        if (item.RoomId == 0)
                        {
                            exsistingNames.Add(item.Name);
                        }
                    }
                    Console.Write("Name: ");
                    name = OtherFunctions.EnterStringWithoutDelimiter("|");
                    while(exsistingNames.Contains(name))
                    {
                        Console.Write("Name already in use! Enter name: ");
                        name = OtherFunctions.EnterStringWithoutDelimiter("|");
                    }
                    Console.WriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
                    type = OtherFunctions.ChooseEquipmentType();
                    Equipment eq = new Equipment { Amount = amount, Name = name, RoomId = 0, Type = type };
                    EquipmentRepo.Add(eq);
                    break;
                }
                else if (answer == "2")
                {
                    
                    Console.WriteLine("Enter ID of equipment to change:");
                    Equipment eq = EquipmentRepo.Get(OtherFunctions.GetValidEquipmentId());
                    if (eq.RoomId != 0)
                    {
                        Console.WriteLine("Equipment not in Storage cannot be edited directly, use the option 1. in the Manage Equipment menu");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter new amount: ");
                        int amount = OtherFunctions.EnterNumberWithLimit(1, 99999999);
                        EquipmentRepo.Update(eq.Id, amount);
                    }

                    break;
                }
                else if (answer.ToUpper() == "X")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                }
            }
        }
        
        //-------------------------------------------------RENOVATIONS---------------------------------------------
        public static void RoomRenovationMenu()
        {
            while (true)
            {
                RoomRenovationService.CheckForRenovations();
                CLI.CLIWriteLine("Room Renovation Menu");
                CLI.CLIWriteLine("1. Simple Renovation");
                CLI.CLIWriteLine("2. Complex Renovation");
                CLI.CLIWriteLine("3. List all Renovations");
                CLI.CLIWriteLine("0. Return");
                int choice = CLI.CLIEnterNumberWithLimit(0,3);
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        RoomRenovationService.SimpleRoomRenovation();
                        break;
                    case 2:
                        ComplexRoomRenovationMenu();
                        break;
                    case 3:
                        RoomRenovationService.ListAllRenovations();
                        break;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ComplexRoomRenovationMenu()
        {

            CLI.CLIWriteLine("1. Split room");
            CLI.CLIWriteLine("2. Join 2 rooms");
            CLI.CLIWriteLine("0. Return");
            int answer = CLI.CLIEnterNumberWithLimit(0,2);
            switch (answer)
            {
                case 0:
                    return;
                case 1:
                    RoomRenovationService.CreateComplexSplitRenovation();
                    return;
                case 2:
                    RoomRenovationService.CreateComplexJoinRenovation();
                    return;
                default:
                    CLI.CLIWriteLine("Invalid option, try again");
                    break;
            }
        }
        //MEDICINES MENU
        public static void MedicinesMenu()
        {
            Console.WriteLine("1. Create medicine");
            Console.WriteLine("2. CRUD ingrediants");
            Console.WriteLine("3. Reviewed medicine requests");
            Console.WriteLine("0. Return");
            int answer = OtherFunctions.EnterNumber();
            while (true)
            {
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        MedicineRequestService.CreateMedicineRequest();
                        return;
                    case 2:
                        CRUDIngrediants();
                        return;
                    case 3:
                        MedicineRequestService.ReviewedMedsMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void CRUDIngrediants()
        {
            CLI.CLIWriteLine("Ingrediants Menu");
            CLI.CLIWriteLine("1. Add new Ingrediant");
            CLI.CLIWriteLine("2. Update Ingrediant");
            CLI.CLIWriteLine("3. Delete Ingrediant");
            CLI.CLIWriteLine("0. to return");
            int answer = CLI.CLIEnterNumberWithLimit(0,3);
            switch (answer)
            {
                case 1:
                    IngrediantService.CreateIngrediant();
                    break;
                case 2:
                    IngrediantService.UpdateIngrediant();
                    break;
                case 3:
                    IngrediantService.DeleteIngrediant();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }
        }
    }
}