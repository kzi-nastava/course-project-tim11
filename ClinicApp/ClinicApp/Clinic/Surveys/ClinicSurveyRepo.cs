using System.Collections.Generic;
using System.IO;

namespace ClinicApp.Clinic.Surveys
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
                    ClinicSurvey survey = new ClinicSurvey(line);
                    AddClinicSurvey(survey);
                }
            }
        }

        public static void PersistChanges()
        {
            string newLine;
            using (StreamWriter sw = File.CreateText(FilePathSurveys))
            {
                foreach (ClinicSurvey survey in AllSurveys)
                {
                    newLine = survey.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }

        public static void AddClinicSurvey(ClinicSurvey survey)
        {
            AllSurveys.Add(survey);
            PersistChanges();
        }
    }
}
