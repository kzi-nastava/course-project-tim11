using System;
using System.Collections.Generic;

namespace ClinicApp.Clinic.Surveys
{
    public class ClinicSurveyService
    {
        public ClinicSurveyService()
        {
        }

        public static void DoClinicSurvey()
        {
            Console.WriteLine("Please rate our nursses work from 0 to 5:");
            int nurssesRating = CLI.CLIEnterNumberWithLimit(0, 5);
            Console.WriteLine("Please rate out doctors work from 0 to 5:");
            int doctorRating = CLI.CLIEnterNumberWithLimit(0, 5);
            Console.WriteLine("Please rate our other workers:");
            int otherEmployeesRating = CLI.CLIEnterNumberWithLimit(0,5);
            Console.WriteLine("PLease rate huygiene of our clinic from 0 to 5:");
            int hygieneRating = CLI.CLIEnterNumberWithLimit(0,5);
            Console.WriteLine("If you want you can leave a comment, so we can improve our customers experience:");
            string comment = Console.ReadLine();
            ClinicSurveyRepo.AddClinicSurvey(new ClinicSurvey(nurssesRating,doctorRating,otherEmployeesRating,hygieneRating,comment));

        }

        public static int GetAverageDoctorsScore()
        {
            int average = 0;
            int count = 0;
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                average += survey.DoctorsRating;
                count++;
            }
            return average / count;

        }
        public static int GetAverageNursesScore()
        {
            int average = 0;
            int count = 0;
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                average += survey.NurssesRating;
                count++;
            }
            return average / count;

        }
        public static int GetAverageOtherEmployeesScore()
        {
            int average = 0;
            int count = 0;
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                average += survey.OtherEmployeesRatings;
                count++;
            }
            return average / count;

        }
        public static int GetAverageHygieneScore()
        {
            int average = 0;
            int count = 0;
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                average += survey.HygieneRating;
                count++;
            }
            return average / count;
        }
        public static List<string> GetAllComments()
        {
            List<string> comments = new List<string>();
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                if (survey.CustomersComment != "")
                {
                    comments.Add(survey.CustomersComment);
                }
            }
            return comments;
        }
        public static List<int> DoctorRatingHistogram()
        {
            List<int> histogram = new List<int> { 0, 0, 0, 0, 0 };
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                switch (survey.DoctorsRating)
                {
                    case 1:
                        histogram[0]++;
                        break;
                    case 2:
                        histogram[1]++;
                        break;
                    case 3:
                        histogram[2]++;
                        break;
                    case 4:
                        histogram[3]++;
                        break;
                    case 5:
                        histogram[4]++;
                        break;
                }
            }
            return histogram;
        }
        public static List<int> NurssesRatingHistogram()
        {
            List<int> histogram = new List<int> { 0, 0, 0, 0, 0 };
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                switch (survey.NurssesRating)
                {
                    case 1:
                        histogram[0]++;
                        break;
                    case 2:
                        histogram[1]++;
                        break;
                    case 3:
                        histogram[2]++;
                        break;
                    case 4:
                        histogram[3]++;
                        break;
                    case 5:
                        histogram[4]++;
                        break;
                }
            }
            return histogram;
        }
        public static List<int> OthersRatingHistogram()
        {
            List<int> histogram = new List<int> { 0, 0, 0, 0, 0 };
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                switch (survey.OtherEmployeesRatings)
                {
                    case 1:
                        histogram[0]++;
                        break;
                    case 2:
                        histogram[1]++;
                        break;
                    case 3:
                        histogram[2]++;
                        break;
                    case 4:
                        histogram[3]++;
                        break;
                    case 5:
                        histogram[4]++;
                        break;
                }
            }
            return histogram;
        }
        public static List<int> HygieneRatingHistogram()
        {
            List<int> histogram = new List<int> { 0, 0, 0, 0, 0 };
            foreach (var survey in ClinicSurveyRepo.AllSurveys)
            {
                switch (survey.HygieneRating)
                {
                    case 1:
                        histogram[0]++;
                        break;
                    case 2:
                        histogram[1]++;
                        break;
                    case 3:
                        histogram[2]++;
                        break;
                    case 4:
                        histogram[3]++;
                        break;
                    case 5:
                        histogram[4]++;
                        break;
                }
            }
            return histogram;
        }
    }
}
