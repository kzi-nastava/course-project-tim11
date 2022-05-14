using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public static class RoomRenovationManager
    {
        static public List<RoomRenovation> RoomRenovationList { get; set; }

        static RoomRenovationManager()
        {
            RoomRenovationList = LoadRoomRenovations();
        }

        public static List<RoomRenovation> GetAll() => RoomRenovationList;

        public static RoomRenovation? Get(int id) => RoomRenovationList.FirstOrDefault(p => p.Id == id);

        public static void Add(RoomRenovation item)
        {
            if (RoomRenovationList.Count == 0)
            {
                item.Id = 1;
            }
            else
            {
                item.Id = RoomRenovationList.Last().Id + 1;
            }
            RoomRenovationList.Add(item);
            PersistChanges();
        }
        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            RoomRenovationList.Remove(item);
            PersistChanges();
        }

        public static void CommitComplexJoinRenovation(RoomRenovation renovation) 
        {
            List<ClinicEquipment> equipmentInJoinedRoom = ClinicEquipmentManager.GetEquipmentFromRoom(renovation.JoinedRoomId);
            foreach(var eq in equipmentInJoinedRoom)
            {
                EquipmentMovement movement = new EquipmentMovement { Amount = eq.Amount, EquipmentId = eq.Id, NewRoomId = renovation.RoomId, MovementDate = DateTime.Today };
                EquipmentMovementManager.CommitChanges(movement);
                ClinicEquipmentManager.Delete(eq.Id);
            }
            renovation.Done = true;
            PersistChanges();
        }

        public static void CommitComplexSplitRenovation(RoomRenovation renovation) 
        {
            
            ClinicRoomManager.Add(renovation.NewRoom);
            renovation.Done = true;
            PersistChanges();
        }

        public static void CheckForRenovations()
        {
            DateTime today = DateTime.Today;
            foreach (var renovation in RoomRenovationList)
            {
                if (!renovation.Done)
                {
                    if (renovation.Duration.EndDate == today)
                    {
                        switch (renovation.Type)
                        {
                            case RenovationType.Simple:
                                renovation.Done = true;
                                PersistChanges();
                                break;
                            case RenovationType.ComplexJoin:
                                CommitComplexJoinRenovation(renovation);
                                break;
                            case RenovationType.ComplexSplit:
                                CommitComplexSplitRenovation(renovation);
                                break;
                        }
                    }
                }
            }
            PersistChanges();
        }
        //==================== FILES STUFF =======================

        static List<RoomRenovation> LoadRoomRenovations()
        {
            List<RoomRenovation> renovations = new List<RoomRenovation>();
            using (StreamReader reader = new StreamReader("../../../Admin/Data/roomRenovations.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    RoomRenovation renovation = ParseRenovation(line);
                    renovations.Add(renovation);
                }
            }
            return renovations;
        }

        static RoomRenovation ParseSimpleRenovation(string [] parameteres)
        {
            RoomRenovation renovation = new RoomRenovation
            {
                Id = Convert.ToInt32(parameteres[0]),
                RoomId = Convert.ToInt32(parameteres[1]),
                Duration = new DateRange
                {
                    StartDate = DateTime.Parse(parameteres[2]),
                    EndDate = DateTime.Parse(parameteres[3])
                },
                Done = bool.Parse(parameteres[4]),
                Type = RenovationType.Simple
            };
            return renovation;
        }

        static RoomRenovation ParseComplexJoinRenovation(string[] parameteres)
        {
            RoomRenovation renovation = new RoomRenovation
            {
                Id = Convert.ToInt32(parameteres[0]),
                RoomId = Convert.ToInt32(parameteres[1]),
                Duration = new DateRange
                {
                    StartDate = DateTime.Parse(parameteres[2]),
                    EndDate = DateTime.Parse(parameteres[3])
                },
                Done = bool.Parse(parameteres[4]),
                Type = RenovationType.ComplexJoin,
                JoinedRoomId = Convert.ToInt32(parameteres[6])
            };
            return renovation;
        }

        static RoomRenovation ParseComplexSplitRenovation(string[] parameteres)
        {
            RoomType roomType;
            switch (parameteres[7])
            {
                case "Operations":
                    roomType = RoomType.Operations;
                    break;
                case "Waiting":
                    roomType = RoomType.Waiting;
                    break;
                case "Examinations":
                    roomType = RoomType.Examinations;
                    break;
                default:
                    roomType = RoomType.STORAGE;
                    break;
            }
            RoomRenovation renovation = new RoomRenovation
            {
                Id = Convert.ToInt32(parameteres[0]),
                RoomId = Convert.ToInt32(parameteres[1]),
                Duration = new DateRange
                {
                    StartDate = DateTime.Parse(parameteres[2]),
                    EndDate = DateTime.Parse(parameteres[3])
                },
                Done = bool.Parse(parameteres[4]),
                Type = RenovationType.ComplexSplit,
                NewRoom = new ClinicRoom
                {
                    Name = parameteres[6],
                    Type = roomType
                }
            };
            return renovation;
        }

        static RoomRenovation ParseRenovation(string line)
        {
            string[] parameteres = line.Split("|");
            if (parameteres[5] == "Simple")
            {
                return ParseSimpleRenovation(parameteres);
            }
            else if (parameteres[5] == "ComplexJoin")
            {
                return ParseComplexJoinRenovation(parameteres);
            }
            else
            {
                return ParseComplexSplitRenovation(parameteres);
            }
        }

        static void PersistChanges()
        {
            File.Delete("../../../Admin/Data/roomRenovations.txt");
            foreach (RoomRenovation renovation in RoomRenovationList)
            {
                string newLine;
                if(renovation.Type == RenovationType.Simple)
                {

                    newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type);
                    
                }
                else if(renovation.Type == RenovationType.ComplexJoin)
                {
                    newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type) + "|" + Convert.ToString(renovation.JoinedRoomId);
                }
                else
                {
                    newLine = Convert.ToString(renovation.Id) + "|" + Convert.ToString(renovation.RoomId) + "|" + renovation.Duration.StartDate.ToString("d") + "|" + renovation.Duration.EndDate.ToString("d") + "|" + Convert.ToString(renovation.Done) + "|" + Convert.ToString(renovation.Type) + "|" + renovation.NewRoom.Name + "|" + Convert.ToString(renovation.NewRoom.Type);
                }
                using (StreamWriter sw = File.AppendText("../../../Admin/Data/roomRenovations.txt"))
                {
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
