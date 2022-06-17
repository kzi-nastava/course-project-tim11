using System.Collections.Generic;
using System.IO;
using ClinicApp.Users;

namespace ClinicApp.HelperClasses
{
    public class MessageBoxRepository
    {
        public static string MessageBoxesFilePath = "../../../Data/message_boxes.txt";

        public static void Load()
        {
            using (StreamReader reader = new StreamReader(MessageBoxesFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    User user;
                    string[] data = line.Split("|");
                    if (UserRepository.Users.TryGetValue(data[0], out user))
                        user.MessageBox.LoadMessages(data[1]);
                }
            }
        }

        public static void Upload()
        {
            string newLine;

            using (StreamWriter sw = File.CreateText(MessageBoxesFilePath))
            {
                foreach (KeyValuePair<string, User> pair in UserRepository.Users)
                {
                    newLine = pair.Value.MessageBox.Compress();
                    if (newLine != null)
                        sw.WriteLine(newLine);
                }
            }
        }
    }
}
