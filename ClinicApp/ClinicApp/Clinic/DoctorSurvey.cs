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
        public DoctorSurvey(Doctor doctor, int rating,bool recommend , string comment)
        {
            ratedDoctor = doctor;
            doctorRating = rating;
            recommendToFriends = recommend;
            customersComment = comment;
        }
    }
}
