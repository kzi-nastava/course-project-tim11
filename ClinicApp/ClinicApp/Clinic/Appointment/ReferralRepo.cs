using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.Clinic
{
    public class ReferralRepo
    {
        public static List<Referral> Referrals { get; set; } = new List<Referral>();
        public static string ReferralsFilePath = "../../../Data/referrals.txt";

        public static void Load()
        {
            using (StreamReader reader = new StreamReader(ReferralsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Referral referral = new Referral(line);
                    referral.Patient.Referrals.Add(referral);
                    Referrals.Add(referral);
                }
            }
        }

        public static void Add(Referral referral)
        {
            Referrals.Add(referral);
        }

        public static void Delete(Referral referral)
        {
            Referrals.Remove(referral);
        }

        public static void PersistChanges()
        {
            string newLine;
            using (StreamWriter sw = File.CreateText(ReferralsFilePath))
            {
                foreach (Referral referral in Referrals)
                {
                    newLine = referral.Compress();
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
