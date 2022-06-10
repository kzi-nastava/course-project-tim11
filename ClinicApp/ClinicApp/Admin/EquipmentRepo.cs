using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class EquipmentRepo
    {
        static string Path { get; set; } = "../../../Admin/Data/equipment.txt";

        static public List<Equipment> ClinicEquipmentList { get; set; } = new List<Equipment>();

        static EquipmentRepo()
        {
            ClinicEquipmentList = EquipmentRepo.Load();

        }
        public static List<Equipment> GetAll() => ClinicEquipmentList;

        public static Equipment? Get(int id) => ClinicEquipmentList.FirstOrDefault(p => p.Id == id);

        public static void Add(Equipment eq)
        {
            eq.Id = ClinicEquipmentList.Last().Id + 1;
            ClinicEquipmentList.Add(eq);
            EquipmentRepo.PersistChanges();
        }
        public static void Delete(int id)
        {
            var heq = Get(id);
            if (heq is null)
                return;
            ClinicEquipmentList.Remove(heq);
            EquipmentRepo.PersistChanges();
        }
        public static void Update(int id, int newAmount)
        {
            var eqToUpdate = Get(id);
            if (eqToUpdate == null)
            {
                return;
            }
            eqToUpdate.Amount = newAmount;
            PersistChanges();
        }
        public static void PersistChanges()
        {
            File.Delete(Path);
            foreach (Equipment eq in ClinicEquipmentList)
            {
                string newLine = Convert.ToString(eq.Id) + "|" + eq.Name + "|" + Convert.ToString(eq.Amount) + "|" + Convert.ToString(eq.RoomId) + "|" + Convert.ToString(eq.Type)+"|"+Convert.ToString(eq.Dynamic);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(newLine);
                }
            }

        }
        public static List<Equipment> Load()
        {
            List<Equipment> eqList = new List<Equipment>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Equipment eq = ParseEquipment(line);
                    eqList.Add(eq);
                }
            }
            return eqList;
        }
        static Equipment ParseEquipment(string line)
        {
            string[] parameteres = line.Split("|");
            EquipmentType type = EquipmentType.Examinations;
            switch (parameteres[4])
            {
                case "Operations":
                    type = EquipmentType.Operations;
                    break;
                case "Hallway":
                    type = EquipmentType.Hallway;
                    break;
                case "RoomFurniture":
                    type = EquipmentType.RoomFurniture;
                    break;
                case "Examinations":
                    type = EquipmentType.Examinations;
                    break;
                case "Gauzes":
                    type = EquipmentType.Gauzes;
                    break;
                case "Stiches":
                    type = EquipmentType.Stiches;
                    break;
                case "Vaccines":
                    type = EquipmentType.Vaccines;
                    break;
                case "Bandages":
                    type = EquipmentType.Bandages;
                    break;
            }
            Equipment eq = new Equipment
            {
                Id = Convert.ToInt32(parameteres[0]),
                Name = parameteres[1],
                Amount = Convert.ToInt32(parameteres[2]),
                RoomId = Convert.ToInt32(parameteres[3]),
                Type = type,
                Dynamic = Convert.ToBoolean(parameteres[5])
            };

            return eq;
        }
    }
}
