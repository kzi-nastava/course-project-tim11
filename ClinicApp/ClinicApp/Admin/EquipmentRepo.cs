﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class EquipmentRepo
    {
        static string Path { get; set; } = "../../../Admin/Data/equipment.txt";

        public static void PersistChanges()
        {
            File.Delete(Path);
            foreach (Equipment eq in EquipmentService.ClinicEquipmentList)
            {
                string newLine = Convert.ToString(eq.Id) + "|" + eq.Name + "|" + Convert.ToString(eq.Amount) + "|" + Convert.ToString(eq.RoomId) + "|" + Convert.ToString(eq.Type);
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
            }
            Equipment eq = new Equipment
            {
                Id = Convert.ToInt32(parameteres[0]),
                Name = parameteres[1],
                Amount = Convert.ToInt32(parameteres[2]),
                RoomId = Convert.ToInt32(parameteres[3]),
                Type = type
            };

            return eq;
        }
    }
}
