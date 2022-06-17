using ClinicApp.Clinic.Appointmens;

namespace ClinicApp.Clinic.Patients
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
