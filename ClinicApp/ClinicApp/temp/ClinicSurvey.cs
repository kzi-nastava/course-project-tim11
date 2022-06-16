using System;
namespace ClinicApp.Clinic
{
    public class ClinicSurvey
    {
        public int NurssesRating { get; set; }
        public int DoctorsRating { get; set; }
        public int OtherEmployeesRatings { get; set; }
        public int HygieneRating { get; set; }
        public string CustomersComment { get; set; }

        public ClinicSurvey(int ratingNurse,int ratingDoctor, int ratingEmployees, int ratingHygiene, string comment)
        {
            NurssesRating = ratingNurse;
            DoctorsRating = ratingDoctor;
            OtherEmployeesRatings = ratingEmployees;
            HygieneRating = ratingHygiene;
            CustomersComment = comment;
        }

        public ClinicSurvey(string line)
        {
            string[] tokens = line.Split('|');
            NurssesRating = Int32.Parse(tokens[0]);
            DoctorsRating = Int32.Parse(tokens[1]);
            OtherEmployeesRatings = Int32.Parse(tokens[2]);
            HygieneRating = Int32.Parse(tokens[3]);
            CustomersComment = tokens[4];
        }
    }
}
