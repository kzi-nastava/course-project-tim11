using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.Clinic
{
    public class FreeDaysRequestRepo
    {
        public static List<FreeDaysRequest> FreeDaysRequests { get; set; } = new List<FreeDaysRequest>();
        public static string FreeDaysRequestsFilePath = "../../../Data/free_days_requests.txt";

        public static void Load()
        {
            using (StreamReader reader = new StreamReader(FreeDaysRequestsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    FreeDaysRequest request = new FreeDaysRequest(line);
                    FreeDaysRequests.Add(request);
                }
            }
        }

        public static void Add(FreeDaysRequest request)
        {
            FreeDaysRequests.Add(request);
            PersistChanges();
        }
        public static void Update(FreeDaysRequest request)
        {
            for (int i = 0; i < FreeDaysRequests.Count(); i++) {
                if (FreeDaysRequests[i].ID == request.ID) {
                    FreeDaysRequests[i] = request;
                }
            }
            PersistChanges();
        }

        public static void Delete(FreeDaysRequest request)
        {
            FreeDaysRequests.Remove(request);
            PersistChanges();
        }

        public static void PersistChanges()
        {
            string newLine;
            using (StreamWriter sw = File.CreateText(FreeDaysRequestsFilePath))
            {
                foreach (FreeDaysRequest request in FreeDaysRequests)
                {
                    newLine = request.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }
        
    }
}
