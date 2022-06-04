using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public abstract class Appointment
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }

        public bool Finished { get; set; }

        public int Tombstone { get; set; }

        public int Edited { get; set; }
        public char Type { get; set; }
        public int Duration { get; set; }

        public abstract string Compress();
        public abstract void ToFile();
        public abstract void View();
        public abstract DateTime NextAvailable();

        public static int GetLastID()
        {
            int id = 0;
            foreach (int appointmentID in SystemFunctions.AllAppointments.Keys)
            {
                if (appointmentID > id)
                {
                    id = appointmentID;
                }
            }
            id++;

            return id;
        }
    }
}
