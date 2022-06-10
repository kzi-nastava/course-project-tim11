using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateRange()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMinutes(1);
        }
        
        public DateRange(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
        }

        public bool IsInRange(DateTime date)
        {
            return (date >= StartDate && date <= EndDate);
        }
        public bool IsOverlaping(DateRange other)
        {
            if (other.StartDate < StartDate && other.EndDate > StartDate)
                return true;
            if (other.StartDate > StartDate && other.StartDate < EndDate)
                return true;
            return false;
        }
        public void ValidateDates()
        {
            if (StartDate > EndDate)
            {
                DateTime x = StartDate;
                StartDate = EndDate;
                EndDate = x;
            }
        }
    }
    
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
