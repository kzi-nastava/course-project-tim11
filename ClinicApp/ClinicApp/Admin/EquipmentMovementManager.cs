using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public static class EquipmentMovmentManager
    {
        static public List<EquipmentMovement> EquipmentMovementList { get; set; }

        static EquipmentMovmentManager()
        {
            EquipmentMovementList = LoadEquipmentMovement();
        }
        public static List<EquipmentMovement> GetAll() => EquipmentMovementList;

        public static EquipmentMovement? Get(int id) => EquipmentMovementList.FirstOrDefault(p => p.Id == id);

        public static void Add(EquipmentMovement item)
        {
            item.Id = EquipmentMovementList.Last().Id + 1;
            EquipmentMovementList.Add(item);
        }
        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            EquipmentMovementList.Remove(item);
        }
        //--------------FILES STUFF-----------------
        public static List<EquipmentMovement> LoadEquipmentMovement()
        {
            List<EquipmentMovement> movements = new List<EquipmentMovement>();
            using (StreamReader reader = new StreamReader("../../../Admin/Data/equipmentMovement.txt"))
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
                MovementDate = DateTime.Parse(parameteres[4])
            };
            return movement;
        }
        public static void PersistChanges()
        {
            File.Delete("../../../Admin/Data/equipmentMovement.txt");
            foreach (EquipmentMovement movement in EquipmentMovementList)
            {
                string newLine = Convert.ToString(movement.Id) + "|" + Convert.ToString(movement.EquipmentId) + "|" + Convert.ToString(movement.NewRoomId) + "|" + Convert.ToString(movement.Amount) + "|" + movement.MovementDate.ToString("d");
                using (StreamWriter sw = File.AppendText("../../../Admin/Data/equipmentMovement.txt"))
                {
                    sw.WriteLine(newLine);
                }
            }
        }

    }
}
