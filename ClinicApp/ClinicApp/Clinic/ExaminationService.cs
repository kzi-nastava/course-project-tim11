using System;
using System.IO;
using System.Linq;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class ExaminationService
    {
        public ExaminationService()
        {
        }

        private void ViewExaminations(Patient patient)
        {
            if (patient.Appointments.Count == 0)
            {
                Console.WriteLine("\nNo future examinations\n");
                return;
            }
            int i = 1;
            foreach (Examination examination in patient.Appointments)
            {
                Console.WriteLine($"\n\n{i}. Examination\n\nId: {examination.ID}; \nTime and Date: {examination.DateTime};\nDoctor last name: {examination.Doctor.LastName}; Doctor name: {examination.Doctor.Name}\n");
                i++;
            }
        }

    }
}
