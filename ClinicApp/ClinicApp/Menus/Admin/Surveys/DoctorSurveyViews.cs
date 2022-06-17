using ClinicApp.Clinic.Surveys;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin.Surveys
{
    class DoctorSurveyViews
    {
        public static void Averages()
        {
            Dictionary<Users.Doctor, int> averages = DoctorSurveyService.GetDoctorAverages();
            foreach (var key in averages.Keys)
            {
                Console.WriteLine("Average for " + key.Name + " " + key.LastName + ": " + averages[key]);
            }
        }
        public static void Comments()
        {
            Dictionary<Users.Doctor, List<string>> comments = DoctorSurveyService.GetComments();
            foreach (var key in comments.Keys)
            {
                Console.WriteLine("Comments for: " + key.Name + " " + key.LastName);
                foreach (var comment in comments[key])
                {
                    Console.WriteLine(comment);
                }
            }
        }
        public static void Histograms()
        {
            Dictionary<Users.Doctor, List<int>> histogram = DoctorSurveyService.GetHstograms();
            foreach (var key in histogram.Keys)
            {
                Console.WriteLine("Ratings for: " + key.Name + " " + key.LastName);
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine((i + 1) + ": " + histogram[key][i]);
                }
            }
        }
        public static void BestThree()
        {
            List<Users.Doctor> doctors = DoctorSurveyService.SortedByAverageRating();
            Console.WriteLine("Top 3 rated doctors");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(doctors[i].Name + " " + doctors[i].LastName);
            }
        }
        public static void BottomThree()
        {
            List<Users.Doctor> doctors = DoctorSurveyService.SortedByAverageRating();
            Console.WriteLine("Bottom 3 rated doctors");
            for (int i = doctors.Count - 1; i > doctors.Count - 4; i--)
            {
                Console.WriteLine(doctors[i].Name + " " + doctors[i].LastName);
            }
        }
    }
}
