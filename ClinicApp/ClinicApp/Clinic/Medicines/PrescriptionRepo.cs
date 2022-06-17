using ClinicApp.Users;
using System.IO;

namespace ClinicApp.Clinic.Medicine
{
    public class PrescriptionRepo
    {
        public static string PrescriptionsFilePath = "../../../Data/prescriptions.txt";

        public static void Load()
        {
            using (StreamReader reader = new StreamReader(PrescriptionsFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Prescription prescription = new Prescription(line);
                    prescription.Patient.Prescriptions.Add(prescription);
                    prescription.Patient.MessageBox.AddMessage(prescription.PresrciptionToMessage());

                }
            }
        }

        public static void Add(Prescription prescription, Patient patient)
        {
            patient.Prescriptions.Add(prescription);
            using (StreamWriter sw = File.AppendText(PrescriptionsFilePath))
            {
                sw.WriteLine(prescription.Compress());
            }
        }
        
    }
}
