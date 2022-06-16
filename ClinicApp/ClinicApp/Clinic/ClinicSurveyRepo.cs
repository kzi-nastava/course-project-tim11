using System;
using System.Collections.Generic;
using System.IO;

namespace ClinicApp.Clinic
{
    public class ClinicSurveyRepo
    {
        public ClinicSurveyRepo()
        {
        }
        public static List<ClinicSurvey> AllSurveys { get; set; }
        public static string FilePathSurveys = ".. / .. / .. / Data / clinicsurveys.txt";
        public void LoadSurveys()
        {
            using (StreamReader reader = new StreamReader(FilePathSurveys))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //Medicine medicine = new Medicine(line);
                    //Medicine.Add(medicine.Name, medicine);
                }
            }
        }

        public static void UploadSurveys()
        {

        }

        public static void AddClinicSurvey(ClinicSurvey survey)
        {
            AllSurveys.Add(survey);
        }
    }
}
