using System;
using System.Collections.Generic;
using System.IO;

namespace ClinicApp.Clinic.Surveys
{
    public class DoctorSurveyRepo
    {
        public static List<DoctorSurvey> DoctorSurveys { get; set; }
        public static string FilePathDoctorSurveys = "../../../Data/doctorsurveys.txt";
        public DoctorSurveyRepo()
        {
        }

        public static void LoadDoctorSurveys()
        {
            using (StreamReader reader = new StreamReader(FilePathDoctorSurveys))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    DoctorSurvey survey = new DoctorSurvey(line);
                    AddDoctorSurvey(survey);
                }
            }
        }


        public static void AddDoctorSurvey(DoctorSurvey survey)
        {
            DoctorSurveys.Add(survey);
            PersistChanges();
        }

        public static void PersistChanges()
        {
            string newLine;
            using (StreamWriter sw = File.CreateText(FilePathDoctorSurveys))
            {
                foreach (DoctorSurvey survey in DoctorSurveys)
                {
                    newLine = survey.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }


    }
}
