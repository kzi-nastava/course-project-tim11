using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class DoctorSurvey
    {
        public Doctor ratedDoctor { get; set; }
        public int doctorRating { get; set; }
        public bool recommendToFriends { get; set; }
        public string customersComment { get; set; }
        public DoctorSurvey(Doctor doctor, int rating,bool recommend , string comment)
        {
            ratedDoctor = doctor;
            doctorRating = rating;
            recommendToFriends = recommend;
            customersComment = comment;
        }
    }
}
