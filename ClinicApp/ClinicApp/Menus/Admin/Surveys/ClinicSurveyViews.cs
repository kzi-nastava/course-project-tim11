using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin
{
    class ClinicSurveyViews
    {
        public static void Averages()
        {
            Console.WriteLine("Average doctors rating: " + Clinic.ClinicSurveyService.GetAverageDoctorsScore());
            Console.WriteLine("Average nurses rating: " + Clinic.ClinicSurveyService.GetAverageNursesScore());
            Console.WriteLine("Average other employees rating: " + Clinic.ClinicSurveyService.GetAverageOtherEmployeesScore());
            Console.WriteLine("Average hygiene rating: " + Clinic.ClinicSurveyService.GetAverageHygieneScore());
        }
        public static void Comments()
        {
            foreach (var comment in Clinic.ClinicSurveyService.GetAllComments())
            {
                Console.WriteLine(comment);
            }
        }
        public static void Histograms()
        {
            List<int> doctors = Clinic.ClinicSurveyService.DoctorRatingHistogram();
            Console.WriteLine("Doctors Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + doctors[i]);
            }
            List<int> nursses = Clinic.ClinicSurveyService.NurssesRatingHistogram();
            Console.WriteLine("Nursses Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + nursses[i]);
            }
            List<int> others = Clinic.ClinicSurveyService.OthersRatingHistogram();
            Console.WriteLine("Other employees Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + others[i]);
            }
            List<int> hygiene = Clinic.ClinicSurveyService.HygieneRatingHistogram();
            Console.WriteLine("Other employees Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + hygiene[i]);
            }
        }
    }
}
