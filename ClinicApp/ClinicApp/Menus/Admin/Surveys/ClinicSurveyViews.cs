using ClinicApp.Clinic.Surveys;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin.Surveys
{
    class ClinicSurveyViews
    {
        public static void Averages()
        {
            Console.WriteLine("Average doctors rating: " + ClinicSurveyService.GetAverageDoctorsScore());
            Console.WriteLine("Average nurses rating: " + ClinicSurveyService.GetAverageNursesScore());
            Console.WriteLine("Average other employees rating: " + ClinicSurveyService.GetAverageOtherEmployeesScore());
            Console.WriteLine("Average hygiene rating: " + ClinicSurveyService.GetAverageHygieneScore());
        }
        public static void Comments()
        {
            foreach (var comment in ClinicSurveyService.GetAllComments())
            {
                Console.WriteLine(comment);
            }
        }
        public static void Histograms()
        {
            List<int> doctors = ClinicSurveyService.DoctorRatingHistogram();
            Console.WriteLine("Doctors Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + doctors[i]);
            }
            List<int> nursses = ClinicSurveyService.NurssesRatingHistogram();
            Console.WriteLine("Nursses Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + nursses[i]);
            }
            List<int> others = ClinicSurveyService.OthersRatingHistogram();
            Console.WriteLine("Other employees Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + others[i]);
            }
            List<int> hygiene = ClinicSurveyService.HygieneRatingHistogram();
            Console.WriteLine("Other employees Histogram");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine((i + 1) + ": " + hygiene[i]);
            }
        }
    }
}
