namespace ClinicApp.Clinic.Medicine
{
    public class MedicineRequest
    {
        public int Id { get; set; }
        public Medicine Medicine { get; set; }
        public string Comment { get; set; }
    }
}
