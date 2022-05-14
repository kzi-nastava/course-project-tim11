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
    public class MessageBox
    {
        private List<string> _messages;
        private int _numberOfMessages;
        public int NumberOfMessages { get { return _numberOfMessages; } }
        public Users.User User = null;

        public MessageBox(Users.User user)
        {
            _numberOfMessages = 0;
            _messages = new List<string>();
            User = user;
        }

        public string Compress()
        {
            if (_numberOfMessages <= 0) //There is nothing to upload to the database.
                return null;
            string text = User.UserName + "|";
            foreach (string message in _messages)
                text += message + ";";
            if (text != "")
                text.Remove(text.Length - 1); //We remove the last character of the string because it is a ";" that we don-t need at the end.
            return text;
        }

        public void Empty()
        {
            _numberOfMessages = 0;
            _messages.Clear();
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
            _numberOfMessages++;
        }

        public void LoadMessages(string text)
        {
            string[] messages = text.Split(";");
            foreach(string message in messages)
                AddMessage(message);
        }

        public void DisplayMessages()
        {
            Console.WriteLine("\nThese are your current messages:\n");
            foreach (string message in _messages)
                Console.WriteLine("-" + message + "\n");
            Empty();
        }
    }
    public class SearchTerms  //small helper class to ease searching equipment
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
