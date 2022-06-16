using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class DoctorSurveyService
    {
        public DoctorSurveyService()
        {
        }

        public static void rateDoctor(Doctor doctor)
        {
            Console.WriteLine("Please rate doctor from 0 to 5:");
            Console.WriteLine("Would you like to recommend this doctor to your friend?(yes/no)");
            Console.WriteLine("Leave a comment if you want, it's optional:");
        }
        public static Dictionary<Doctor, int> GetDoctorAverages()
        {
            Dictionary<Doctor, List<int>> ratings = new Dictionary<Doctor, List<int>>();
            Dictionary<Doctor, int> averages = new Dictionary<Doctor, int>();
            foreach (var survey in DoctorSurveyRepo.doctorSurveys) 
            {
                ratings[survey.ratedDoctor].Add(survey.doctorRating);
            }
            foreach(var key in ratings.Keys)
            {
                int average = 0;
                int count = 0;
                foreach(var rating in ratings[key])
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
            foreach(var survey in Clinic.DoctorSurveyRepo.doctorSurveys)
            {
                comments[survey.ratedDoctor].Add(survey.customersComment);
            }
            return comments;
        }
        public static Dictionary<Doctor, List<int>> GetHstograms()
        {
            Dictionary<Doctor, List<int>> histogram = new Dictionary<Doctor, List<int>>();
            foreach (var survey in Clinic.DoctorSurveyRepo.doctorSurveys)
            {
                if (!histogram.ContainsKey(survey.ratedDoctor))
                {
                    histogram[survey.ratedDoctor].Add(0);
                    histogram[survey.ratedDoctor].Add(0);
                    histogram[survey.ratedDoctor].Add(0);
                    histogram[survey.ratedDoctor].Add(0);
                    histogram[survey.ratedDoctor].Add(0);
                }
            }
            foreach (var survey in Clinic.DoctorSurveyRepo.doctorSurveys)
            {
                switch (survey.doctorRating)
                {
                    case 1:
                        histogram[survey.ratedDoctor][0]++;
                        break;
                    case 2:
                        histogram[survey.ratedDoctor][1]++;
                        break;
                    case 3:
                        histogram[survey.ratedDoctor][2]++;
                        break;
                    case 4:
                        histogram[survey.ratedDoctor][3]++;
                        break;
                    case 5:
                        histogram[survey.ratedDoctor][4]++;
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
