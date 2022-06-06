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
                Console.WriteLine("Manage Equipment");
                Console.WriteLine("1. List all");
                Console.WriteLine("2. Search");
                Console.WriteLine("3. Move equipment");
                Console.WriteLine("X to return");
                string choice = Console.ReadLine();
                switch (choice.ToUpper())
                {
                    case "1":
                        EquipmentService.ListAllEquipment();
                        break;
                    case "2":
                        EquipmentSearchService.SearchEquipment();
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
        public static void MoveEquipmentMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Move Equipment");
                Console.WriteLine("2. Add new Equipment to Storage");
                Console.WriteLine("0 to return");
                int answer = OtherFunctions.EnterNumberWithLimit(0, 2);
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        MoveEquipment();
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
        public static void MoveEquipment() //menu for creating a new equipment movement 
        {
            Equipment eq;
            Console.WriteLine("Enter ID of equipment to change:");
            int id = OtherFunctions.GetValidEquipmentId();
            eq = EquipmentRepo.Get(id);
            Console.WriteLine("Enter amount to move");
            int amount = OtherFunctions.EnterNumberWithLimit(1, eq.Amount);
            Console.WriteLine("Enter the Id of the room where the equipment is going to");
            id = OtherFunctions.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            Console.WriteLine("Enter date on which the equipment is being moved");
            DateTime date = OtherFunctions.EnterDate();
            EquipmentMovement movement = new EquipmentMovement { EquipmentId = eq.Id, Amount = amount, NewRoomId = room.Id, MovementDate = date, Done = false };
            EquipmentMovementRepo.Add(movement);
        }
        //-------------------------------------------------RENOVATIONS---------------------------------------------
        public static void RoomRenovationMenu()
        {
            while (true)
            {
                RoomRenovationService.CheckForRenovations();
                Console.WriteLine("Room Renovation Menu");
                Console.WriteLine("1. Simple Renovation");
                Console.WriteLine("2. Complex Renovation");
                Console.WriteLine("3. List all Renovations");
                Console.WriteLine("0. Return");
                int choice = OtherFunctions.EnterNumber();
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        SimpleRoomRenovationMenu();
                        break;
                    case 2:
                        ComplexRoomRenovationMenu();
                        break;
                    case 3:
                        ListAllRenovations();
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }


        }
        public static void SimpleRoomRenovationMenu()
        {
                      
            Console.WriteLine("Enter ID of the room you want to Renovate");
            int id = OtherFunctions.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            if (room.Id == 0)
            {
                Console.WriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(room.Id);

            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = room.Id,
                Duration = duration,
                Type = RenovationType.Simple,
                Done = false
            };
            RoomRenovationRepo.Add(renovation);

        }
        public static void CreateComplexSplitRenovation()
        {
            //get renovated room id
            Console.WriteLine("Enter ID of the room you want to Renovate");
            int id = OtherFunctions.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            if (room.Id == 0)
            {
                Console.WriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(room.Id);
            //create new room
            Console.Write("Name of new room: ");
            string name = OtherFunctions.EnterStringWithoutDelimiter("|");
            Console.Write("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = OtherFunctions.ChooseRoomType();
            Room newRoom = new Room { Name = name, Type = roomType };
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = room.Id,
                Duration = duration,
                Type = RenovationType.ComplexSplit,
                Done = false,
                NewRoom = newRoom
            };
            RoomRenovationRepo.Add(renovation);
        }
        public static void CreateComplexJoinRenovation()
        {

            //get renovated room id
            Console.WriteLine("Enter ID of the room you want to Renovate");
            int id = OtherFunctions.GetValidRoomId();
            Room room = RoomRepo.Get(id);
            if (room.Id == 0)
            {
                Console.WriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = GetUninterruptedDateRange(room.Id);

            //get the joined room
            Console.WriteLine("Enter ID of the room you want to Renovate");
            int otherId = OtherFunctions.GetValidRoomId();
            Room otherRoom = RoomRepo.Get(otherId);
            if (room.Id == 0)
            {
                Console.WriteLine("You cannot renovate Storage!");
                return;
            }
            if (otherRoom.Id == 0)
            {
                Console.WriteLine("You cannot delete Storage!");
                return;
            }
            if (OtherFunctions.CheckForExaminations(duration, otherRoom.Id))
            {
                Console.WriteLine("This room has an appointment at the given time, discarding");
                return;
            }
            RoomRenovation renovation = new RoomRenovation
            {
                RoomId = room.Id,
                Duration = duration,
                Type = RenovationType.ComplexJoin,
                Done = false,
                JoinedRoomId = otherRoom.Id,
            };
            RoomRenovationRepo.Add(renovation);
        }
        public static void ComplexRoomRenovationMenu()
        {

            Console.WriteLine("1. Split room");
            Console.WriteLine("2. Join 2 rooms");
            Console.WriteLine("0. Return");
            int answer = OtherFunctions.EnterNumber();
            while (true)
            {
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        CreateComplexSplitRenovation();
                        return;
                    case 2:
                        CreateComplexJoinRenovation();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void ListAllRenovations()
        {
            string newLine;
            foreach (RoomRenovation renovation in RoomRenovationRepo.GetAll())
            {
                switch (renovation.Type)
                {
                    case RenovationType.Simple:
                        newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type);
                        Console.WriteLine(newLine);
                        break;
                    case RenovationType.ComplexJoin:
                        newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type) + "| ID of joined room: " + Convert.ToString(renovation.JoinedRoomId);
                        Console.WriteLine(newLine);
                        break;
                    case RenovationType.ComplexSplit:
                        newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type) + "| new room will be: " + renovation.NewRoom.Name + "|" + Convert.ToString(renovation.NewRoom.Type);
                        Console.WriteLine(newLine);
                        break;
                }
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
                        CreateMedicine();
                        return;
                    case 2:
                        CRUDIngrediants();
                        return;
                    case 3:
                        ReviewedMedsMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again");
                        break;
                }
            }
        }
        public static void CreateMedicine()
        {
            string name;
            Console.WriteLine("Enter medicine name");
            name = OtherFunctions.EnterStringWithoutDelimiter("|");
            while (SystemFunctions.Medicine.ContainsKey(name))
            {
                Console.WriteLine("Name already taken, enter another name");
                name = OtherFunctions.EnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = new List<string>();
            List<string> offeredIngrediants = IngrediantService.GetAll();
            Console.WriteLine("Choose ingrediants, 0 to finish choosing");
            while (true)
            {
                foreach(var ingrediant in offeredIngrediants)
                {
                    Console.WriteLine(offeredIngrediants.IndexOf(ingrediant)+1 + ". " + ingrediant);
                }
                var choice = OtherFunctions.EnterNumberWithLimit(0,offeredIngrediants.Count);
                if (choice == 0) 
                { 
                    break; 
                }
                chosenIngrediants.Add(offeredIngrediants[choice - 1]);
                offeredIngrediants.Remove(offeredIngrediants[choice - 1]);
            }
            Clinic.Medicine medicine = new Clinic.Medicine(name,chosenIngrediants);
            MedicineRequest mr = new MedicineRequest { Medicine = medicine, Comment = "" };
            MedicineRequestService.Add(mr);
        }
        public static void CRUDIngrediants()
        {
            Console.WriteLine("Ingrediants Menu");
            Console.WriteLine("1. Add new Ingrediant");
            Console.WriteLine("2. Update Ingrediant");
            Console.WriteLine("3. Delete Ingrediant");
            Console.WriteLine("0. to return");
            int answer = OtherFunctions.EnterNumberWithLimit(0,3);
            switch (answer)
            {
                case 1:
                    CreateIngrediant();
                    break;
                case 2:
                    UpdateIngrediant();
                    break;
                case 3:
                    DeleteIngrediant();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }
        }
        public static void CreateIngrediant()
        {
            Console.WriteLine("Enter the new ingrediant");
            string ingrediant = OtherFunctions.EnterStringWithoutDelimiter("|");
            IngrediantService.Add(ingrediant);
        }
        public static void UpdateIngrediant()
        {
            Console.WriteLine("Select the ingrediant to update");
            List<string> offeredIngrediants = IngrediantService.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                Console.WriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = OtherFunctions.EnterNumberWithLimit(1, offeredIngrediants.Count);
            if (indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            Console.WriteLine("Enter the new ingrediant");
            string newIngr = OtherFunctions.EnterStringWithoutDelimiter("|");
            IngrediantService.Update(selected, newIngr);
        }
        public static void DeleteIngrediant()
        {
            Console.WriteLine("Select the ingrediant to delete");
            List<string> offeredIngrediants = IngrediantService.GetAll();
            foreach (var ingrediant in offeredIngrediants)
            {
                Console.WriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
            }
            int indexOfSelected = OtherFunctions.EnterNumberWithLimit(1, offeredIngrediants.Count);
            if(indexOfSelected == 0)
            {
                return;
            }
            string selected = offeredIngrediants[indexOfSelected - 1];
            IngrediantService.Delete(selected);
        }
        public static void ReviewedMedsMenu()
        {
            Console.WriteLine("These requests have been reviewed by a doctor and should be fixed up");
            foreach(var request in MedicineRequestService.GetAll())
            {
                Console.WriteLine("----------------------------------------------------------");
                if (request.Comment != "")
                {
                    Console.WriteLine("Request ID: " + request.Id + 
                        "\nMedicine name: " + request.Medicine.Name + 
                        "\nMedicine ingrediants: " + request.Medicine.Ingredients + 
                        "\nDoctor's comment: " + request.Comment);
                    Console.WriteLine("----------------------------------------------------------");
                }
            }
            int id = OtherFunctions.EnterNumber();
            MedicineRequest selected = MedicineRequestService.Get(id);
            string name;
            Console.WriteLine("Enter medicine name");
            name = OtherFunctions.EnterStringWithoutDelimiter("|");
            while (SystemFunctions.Medicine.ContainsKey(name))
            {
                Console.WriteLine("Name already taken, enter another name");
                name = OtherFunctions.EnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = new List<string>();
            List<string> offeredIngrediants = IngrediantService.GetAll();
            Console.WriteLine("Choose ingrediants, 0 to finish choosing");
            while (true)
            {
                foreach (var ingrediant in offeredIngrediants)
                {
                    Console.WriteLine(offeredIngrediants.IndexOf(ingrediant) + 1 + ". " + ingrediant);
                }
                var choice = OtherFunctions.EnterNumberWithLimit(0, offeredIngrediants.Count);
                if (choice == 0)
                {
                    break;
                }
                chosenIngrediants.Add(offeredIngrediants[choice - 1]);
                offeredIngrediants.Remove(offeredIngrediants[choice - 1]);
            }
            Clinic.Medicine medicine = new Clinic.Medicine(name, chosenIngrediants);
            MedicineRequest newRequest = new MedicineRequest{  Medicine = medicine, Comment = ""};
            MedicineRequestService.Update(id, newRequest);

        }
        public static DateRange GetUninterruptedDateRange(int roomId)
        {
            while (true)
            {
                Console.WriteLine("Enter the start date of the renovation");
                DateTime start = OtherFunctions.EnterDate();
                Console.WriteLine("Enter the end date of the renovation");
                DateTime end = OtherFunctions.EnterDate();
                DateRange duration = new DateRange { StartDate = start, EndDate = end };
                duration.ValidateDates();
                if (OtherFunctions.CheckForExaminations(duration, roomId) == false)
                {
                    return duration;
                }
                Console.WriteLine("You have entered invalid dates, there is an examination during the date range!");
            }
        }
    }
}