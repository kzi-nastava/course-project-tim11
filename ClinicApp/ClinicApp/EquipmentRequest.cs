using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp
{
    
    
    public class EquipmentRequest
    {
        public EquipmentType Type;
        public int Amount;
        public DateTime DateCreated;

        public EquipmentRequest(EquipmentType type, int amount, DateTime dateCreated)
        {
            Type = type;
            Amount = amount;
            DateCreated = dateCreated;
        }

        public EquipmentRequest(string text)
        {
            string[] data = text.Split("|");
            Enum.TryParse(data[0], out Type);
            Amount = Int32.Parse(data[1]);
            DateCreated = DateTime.Parse(data[2]);
        }

        public string Compress()
        {
            return Type.ToString() + "|" + Amount + "|" + DateCreated.ToString("dd/MM/yyyy");
        }

        public void FulfillOrder()
        {
            Equipment equipment = new Equipment
            {
                Id = 0,
                Name = Type.ToString(),
                Amount = Amount,
                RoomId = 0,
                Type = Type
            };
            EquipmentRepo.Add(equipment);
        }
    }
}
