using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public enum FreeDaysState
    {
        Waiting, Accepted, Denied
    };
    public class FreeDaysRequest
    {
        public int ID { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public FreeDaysState State { get; set; }
        public bool Urgent { get; set; }
        public string Reason { get; set; }

        public FreeDaysRequest(int id, Doctor doctor, DateTime dateCreated, DateTime dateFrom, DateTime dateTo, FreeDaysState state, bool urgent, string reason)
        {
            this.ID = id;
            this.Doctor = doctor;
            this.DateCreated = dateCreated;
            this.DateFrom = dateFrom;
            this.DateTo = dateTo;
            this.State = state;
            this.Urgent = urgent;
            this.Reason = reason;
        }

        public FreeDaysRequest(string line)
        {
            string[] tokens = line.Split('|');
            this.ID = Convert.ToInt32(tokens[0]);
            this.Doctor = UserRepository.Doctors[tokens[1]];
            this.DateCreated = DateTime.Parse(tokens[2]);
            this.DateFrom = DateTime.Parse(tokens[3]);
            this.DateTo = DateTime.Parse(tokens[4]);
            FreeDaysState tmp;
            Enum.TryParse(tokens[5], out tmp);
            this.State = tmp;
            this.Urgent = Convert.ToBoolean(tokens[6]);
            this.Reason = tokens[7];
            
        }

        public string Compress()
        {
            return ID + "|" + Doctor.UserName + "|" + DateCreated + "|" + DateFrom + "|" + DateTo + "|" + State + "|" + Urgent + "|" + Reason;
        }
    }


}

