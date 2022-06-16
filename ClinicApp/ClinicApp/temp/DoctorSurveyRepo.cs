using System;
using System.Collections.Generic;

namespace ClinicApp.Clinic
{
    public class DoctorSurveyRepo
    {
        public static List<DoctorSurvey> doctorSurveys { get; set; }
        public static string FilepathDoctorSurveys = "../../../Data/doctorsurveys.txt";
        public DoctorSurveyRepo()
        {
        }

        public static void loadDoctorSurveys()
        {
        }

        public static void uploadDoctorSurveys()
        {

        }

        public static void addDoctorSurvey(DoctorSurvey survey)
        {
            doctorSurveys.Add(survey);
        }



    }
}
