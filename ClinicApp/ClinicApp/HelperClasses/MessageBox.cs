using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.HelperClasses
{
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
            foreach (string message in messages)
                AddMessage(message);
        }

        public void DisplayMessages()
        {
            CLI.CLIWriteLine("\nThese are your current messages:\n");
            foreach (string message in _messages)
                CLI.CLIWriteLine("-" + message + "\n");
            Empty();
        }
    }
}