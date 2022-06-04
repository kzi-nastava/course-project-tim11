using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class EquipmentMovementRepo
    {
        static string Path { get; set; } = "../../../Admin/Data/equipmentMovement.txt";
        static public List<EquipmentMovement> EquipmentMovementList { get; set; }

        static EquipmentMovementRepo()
        {
            EquipmentMovementList = Load();
        }
        public static List<EquipmentMovement> GetAll() => EquipmentMovementList;

        public static EquipmentMovement? Get(int id) => EquipmentMovementList.FirstOrDefault(p => p.Id == id);

        public static void Add(EquipmentMovement item)
        {
            if (EquipmentMovementList.Count == 0)
            {
                item.Id = 1;
            }
            else
            {
                item.Id = EquipmentMovementList.Last().Id + 1;
            }
            EquipmentMovementList.Add(item);
            PersistChanges();
        }
        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            EquipmentMovementList.Remove(item);
            PersistChanges();
        }
        public static List<EquipmentMovement> Load()
        {
            List<EquipmentMovement> movements = new List<EquipmentMovement>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    EquipmentMovement movement = ParseMovement(line);
                    movements.Add(movement);
                }
            }
            return movements;
        }
        public static EquipmentMovement ParseMovement(string line)
        {
            string[] parameteres = line.Split("|");
            EquipmentMovement movement = new EquipmentMovement
            {
                Id = Convert.ToInt32(parameteres[0]),
                EquipmentId = Convert.ToInt32(parameteres[1]),
                NewRoomId = Convert.ToInt32(parameteres[2]),
                Amount = Convert.ToInt32(parameteres[3]),
                MovementDate = DateTime.Parse(parameteres[4]),
                Done = Boolean.Parse(parameteres[5])
            };
            return movement;
        }
        public static void PersistChanges()
        {
            File.Delete(Path);
            foreach (EquipmentMovement movement in EquipmentMovementList)
            {
                string newLine = Convert.ToString(movement.Id) + "|" + Convert.ToString(movement.EquipmentId) + "|" + Convert.ToString(movement.NewRoomId) + "|" + Convert.ToString(movement.Amount) + "|" + movement.MovementDate.ToString("d") + "|" + movement.Done.ToString();
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
