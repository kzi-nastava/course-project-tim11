using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class PatientRequest
    {
        public Examination EditedExamination { get; set; }
        public bool Approved { get; set; }

        public PatientRequest(Examination examination)
        {
            EditedExamination = examination;
            Approved = false;
        }
    }
}
