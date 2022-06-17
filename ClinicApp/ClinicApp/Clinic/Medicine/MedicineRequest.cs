using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class MedicineRequest
    {
        public int Id { get; set; }
        public Clinic.Medicine Medicine { get; set; }
        public string Comment { get; set; }
    }
}
