using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class DoctorSurvey
    {
        Doctor ratedDoctor { get; set; }
        int doctorRating { get; set; }
        bool recommendToFriends { get; set; }
        string customersComment { get; set; }

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
