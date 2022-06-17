using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class DoctorSurvey
    {
        public Doctor RatedDoctor { get; set; }
        public int DoctorRating { get; set; }
        public bool RecommendToFriends { get; set; }
        public string CustomersComment { get; set; }

        public DoctorSurvey(Doctor doctor, int rating, bool recommend, string comment)
        {
            RatedDoctor = doctor;
            DoctorRating = rating;
            RecommendToFriends = recommend;
            CustomersComment = comment;
        }

        public DoctorSurvey(string line)
        {
            string[] tokens = line.Split('|');
            DoctorRating = Int32.Parse(tokens[1]);
            RecommendToFriends = Convert.ToBoolean(tokens[2]);
            CustomersComment = tokens[3];

        }

        public string Compress()
        {
            return RatedDoctor.UserName + "|" + DoctorRating + "|" + RecommendToFriends.ToString() + "|" + CustomersComment;
        }
    }
}
