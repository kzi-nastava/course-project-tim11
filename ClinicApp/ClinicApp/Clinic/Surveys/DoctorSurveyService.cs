using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Users;

namespace ClinicApp.Clinic.Surveys
{
    public class DoctorSurveyService
    {
        public DoctorSurveyService()
        {
            
        }

        public static void RateDoctor(Doctor doctor)
        {
            bool recommend = false;
            Console.WriteLine("Please rate doctor from 0 to 5:");
            int doctorRate = CLI.CLIEnterNumberWithLimit(0,5);
            Console.WriteLine("Would you like to recommend this doctor to your friend?(yes/no)");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "yes")
            {
                recommend = true;
            }
            Console.WriteLine("Leave a comment if you want, it's optional:");
            string comment = Console.ReadLine();
            DoctorSurveyRepo.AddDoctorSurvey(new DoctorSurvey(doctor,doctorRate,recommend,comment)); ;
        }

        public static Dictionary<Doctor, int> GetDoctorAverages()
        {
            Dictionary<Doctor, List<int>> ratings = new Dictionary<Doctor, List<int>>();
            Dictionary<Doctor, int> averages = new Dictionary<Doctor, int>();
            foreach (var survey in DoctorSurveyRepo.DoctorSurveys)
            {
                if (ratings.ContainsKey(survey.RatedDoctor))
                    ratings[survey.RatedDoctor].Add(survey.DoctorRating);
                else
                    ratings[survey.RatedDoctor] = new List<int>() { survey.DoctorRating };
            }
            foreach (var key in ratings.Keys)
            {
                int average = 0;
                int count = 0;
                foreach (var rating in ratings[key])
                {
                    average += rating;
                    count++;
                }
                average = average / count;
                averages[key] = average;
            }
            return averages;
        }

        public static Dictionary<Doctor, List<string>> GetComments()
        {
            Dictionary<Doctor, List<string>> comments = new Dictionary<Doctor, List<string>>();
            foreach (var survey in DoctorSurveyRepo.DoctorSurveys)
            {
                if (comments.ContainsKey(survey.RatedDoctor))
                    comments[survey.RatedDoctor].Add(survey.CustomersComment);
                else
                    comments[survey.RatedDoctor] = new List<string>() { survey.CustomersComment };
            }
            return comments;
        }

        public static Dictionary<Doctor, List<int>> GetHstograms()
        {
            Dictionary<Doctor, List<int>> histogram = new Dictionary<Doctor, List<int>>();
            foreach (var survey in DoctorSurveyRepo.DoctorSurveys)
            {
                if (!histogram.ContainsKey(survey.RatedDoctor))
                {
                    histogram[survey.RatedDoctor] = new List<int> { 0, 0, 0, 0, 0 };
                }
            }
            foreach (var survey in DoctorSurveyRepo.DoctorSurveys)
            {
                switch (survey.DoctorRating)
                {
                    case 1:
                        histogram[survey.RatedDoctor][0]++;
                        break;
                    case 2:
                        histogram[survey.RatedDoctor][1]++;
                        break;
                    case 3:
                        histogram[survey.RatedDoctor][2]++;
                        break;
                    case 4:
                        histogram[survey.RatedDoctor][3]++;
                        break;
                    case 5:
                        histogram[survey.RatedDoctor][4]++;
                        break;
                }
            }
            return histogram;
        }

        public static List<Doctor> SortedByAverageRating()
        {
            List<Doctor> doctors = new List<Doctor>();
            Dictionary<Doctor, int> averages = GetDoctorAverages();
            var ordered = averages.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            foreach (var doctor in ordered.Keys)
                doctors.Add(doctor);
            return doctors;
        }

    }
}
