using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class EquipmentMovementRepo
    {
        static string Path { get; set; } = "../../../Admin/Data/equipmentMovement.txt";
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
            foreach (EquipmentMovement movement in EquipmentMovementService.EquipmentMovementList)
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
