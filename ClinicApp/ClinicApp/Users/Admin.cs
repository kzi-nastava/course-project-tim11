using System;
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
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role;
        }

        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2. Manage Clinic Rooms");
            Console.WriteLine("3. Manage Clinic Equipment");
            Console.WriteLine("0: Exit");

            return 1;
        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    RoomManagmentMenu();
                    break;
                case 3:
                    EquipmentManagmentMenu();
                    break;
            }
        }
        public static void RoomManagmentMenu()
        {
            while (true)
            {
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
            foreach (ClinicRoom room in ClinicRoomManager.ClinicRooms)
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
            ClinicRoomManager.Add(room);
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
                int id = OtherFunctions.EnterNumber();
                room = ClinicRoomManager.Get(id);
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
            ClinicRoomManager.Update(room.Id, name, roomType);
        }
        public static void DeleteRoom()
        {
            ClinicRoom room;
            int id;
            while (true)
            {
                Console.WriteLine("Enter ID of the room you want to Delete");
                id = OtherFunctions.EnterNumber();
                room = ClinicRoomManager.Get(id);
                if (room is null)
                {
                    Console.WriteLine("Invalid option, try again");
                }
                else break;
            }
            if (id == 0)
            {
                Console.WriteLine("You cannot delete Storage!");
            }
            else ClinicRoomManager.Delete(id);

        }
        //------------------------------------------------------MANAGE EQUIPMENT------------------------------------------
        public static void EquipmentManagmentMenu()
        {
            while (true)
            {
                Console.WriteLine("Manage Equipment");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. Search");
                Console.WriteLine("3. Move equipment");
                Console.WriteLine("X to return");
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
                        MoveEquipmentMenu();
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
            foreach (ClinicEquipment eq in ClinicEquipmentManager.ClinicEquipmentList)
            {
                Console.WriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + ClinicRoomManager.Get(eq.RoomId).Name + " " + ClinicRoomManager.Get(eq.RoomId).Type + " " + eq.Type);
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

            Results = ClinicEquipmentManager.Search(STerms.SearchTerm);

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
                else if (answer.ToLower() == "n")
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
                Results = ClinicEquipmentManager.FilterByEqType(Results, STerms.FilterByEq);
            }
            if (STerms.FilterByRoomTypeBool == true)
            {
                Results = ClinicEquipmentManager.FilterByRoomType(Results, STerms.FilterByRoom);
            }
            if (STerms.FilterByAmountBool == true)
            {
                switch (STerms.STAmount)
                {
                    case 1:
                        Results = ClinicEquipmentManager.FilterByNumbers(Results, 0, 0);
                        break;
                    case 2:
                        Results = ClinicEquipmentManager.FilterByNumbers(Results, 1, 10);
                        break;
                    case 3:
                        Results = ClinicEquipmentManager.FilterByNumbers(Results, 11, 10000000);
                        break;
                }
            }
            Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
            foreach (ClinicEquipment eq in Results)
            {
                Console.WriteLine(eq.Id + " " + eq.Name + " " + eq.Amount + " " + ClinicRoomManager.Get(eq.RoomId).Name + " " + ClinicRoomManager.Get(eq.RoomId).Type + " " + eq.Type);
            }

        }
        public static void MoveEquipmentMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Move Equipment");
                Console.WriteLine("2. Add new Equipment to Storage");
                Console.WriteLine("X to return");
                string answer = Console.ReadLine();
                if (answer == "1")
                {
                    MoveEquipment();
                }
                else if (answer == "2")
                {
                    AddEqToStorage();
                }
                else if (answer.ToUpper() == "X")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                }

            }

        }
        public static void AddEqToStorage()
        {
            while (true)
            {
                Console.WriteLine("List Equipment in Storage? (y/n): ");
                string answer = Console.ReadLine();
                if (answer.ToLower() == "y")
                {
                    Console.WriteLine("ID | NAME | AMOUNT | ROOM NAME | ROOM TYPE | EQUIPMENT TYPE");
                    foreach (ClinicEquipment item in ClinicEquipmentManager.ClinicEquipmentList)
                    {
                        if (item.RoomId == 0)
                        {
                            Console.WriteLine(item.Id + " " + item.Name + " " + item.Amount + " " + ClinicRoomManager.Get(item.RoomId).Name + " " + ClinicRoomManager.Get(item.RoomId).Type + " " + item.Type);
                        }
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("1. Add new Equipment");
                Console.WriteLine("2. Edit amount of existing Equipment");
                string answer = Console.ReadLine();
                if (answer == "1")
                {
                    string name;
                    EquipmentType type;
                    int amount = OtherFunctions.EnterNumber();
                    List<string> exsistingNames = new List<string>();
                    foreach(ClinicEquipment item in ClinicEquipmentManager.ClinicEquipmentList)
                    {
                        if (item.RoomId == 0)
                        {
                            exsistingNames.Add(item.Name);
                        }
                    }

                    while (true)
                    {
                        Console.Write("Name: ");
                        name = Console.ReadLine();
                        if (name.Contains("|"))
                        {
                            Console.WriteLine("Invalid option, name cannot contain |, try again");
                        }
                        else if (exsistingNames.Contains(name))
                        {
                            Console.WriteLine("Invalid option, equipment with this name already exists");
                        }
                        else { break; }
                    }
                    while (true)
                    {
                        Console.Write("\nChoose Type (1 for Operations, 2 for RoomFurniture, 3 for Hallway, 4 for Examinations): ");
                        answer = Console.ReadLine();
                        if (answer == "1")
                        {
                            type = EquipmentType.Operations;
                            break;
                        }
                        else if (answer == "2")
                        {
                            type = EquipmentType.RoomFurniture; 
                            break;
                        }
                        else if (answer == "3")
                        {
                            type = EquipmentType.Hallway;
                            break;
                        }
                        else if (answer == "4")
                        {
                            type = EquipmentType.Examinations;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option, try again");
                        }

                    }
                    ClinicEquipment eq = new ClinicEquipment { Amount = amount, Name = name, RoomId = 0, Type = type };
                    ClinicEquipmentManager.Add(eq);
                    break;
                }
                else if (answer == "2")
                {
                    ClinicEquipment eq;
                    while (true)
                    {
                        Console.WriteLine("Enter ID of equipment to change:");
                        int id = OtherFunctions.EnterNumber();
                        eq = ClinicEquipmentManager.Get(id);
                        if (eq is null)
                        {
                            Console.WriteLine("Invalid option, try again");
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (eq.RoomId != 0)
                    {
                        Console.WriteLine("Equipment not in Storage cannot be edited directly, use the option 1. in Equipment Menu");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter new amount");
                        int amount = OtherFunctions.EnterNumber();
                        ClinicEquipmentManager.Update(eq.Id, amount);
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
        public static void MoveEquipment()
        {
            
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
}