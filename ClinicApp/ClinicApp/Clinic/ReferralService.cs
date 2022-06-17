using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class ReferralService
    {
        //=======================================================================================================================================================================
        // CREATE REFERRAL FOR PATIENT

        public static void CreateReferral(ref Patient patient, ref Doctor doctorExamined)
        {
            Console.WriteLine("Create referral for (1) specific doctor or (2) specific field");
            int i = CLI.CLIEnterNumberWithLimit(0, 3);
            if (i == 1)
            {
                Doctor doctor = OtherFunctions.AskUsernameDoctor();
                if (doctor == null) return;
                Referral referral = new Referral(doctorExamined, patient, doctor, doctor.Field);
                patient.Referrals.Add(referral);
                ReferralRepo.Referrals.Add(referral);
            }
            else
            {
                Fields field = OtherFunctions.AskField();
                Referral referral = new Referral(doctorExamined, patient, null, field);
                patient.Referrals.Add(referral);
                ReferralRepo.Referrals.Add(referral);

            }
            Console.WriteLine("Referral created successfully!");

        }
    }
}
