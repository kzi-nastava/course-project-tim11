using System;
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
    }
}
