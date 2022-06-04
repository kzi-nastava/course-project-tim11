using System;
using System.Collections.Generic;
using System.IO;

namespace ClinicApp.AdminFunctions
{
    class RoomRepo
    {
        static string Path { get; set; } = "../../../Admin/Data/rooms.txt";
        public static void PersistChanges()
        {
            File.Delete(Path);
            foreach (Room room in RoomService.ClinicRooms)
            {
                string newLine = Convert.ToString(room.Id) + "|" + room.Name + "|" + Convert.ToString(room.Type);
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(newLine);
                }
            }

        }
        public static List<Room> Load()
        {
            List<Room> rooms = new List<Room>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Room room = ParseRoom(line);
                    rooms.Add(room);
                }
            }
            return rooms;
        }
        static Room ParseRoom(string line)
        {
            string[] parameteres = line.Split("|");
            RoomType type = RoomType.STORAGE;
            switch (parameteres[2])
            {
                case "Operations":
                    type = RoomType.Operations;
                    break;
                case "Waiting":
                    type = RoomType.Waiting;
                    break;
                case "Examinations":
                    type = RoomType.Examinations;
                    break;
            }
            Room room = new Room { Id = Convert.ToInt32(parameteres[0]), Name = parameteres[1], Type = type };

            return room;
        }
    }
}
