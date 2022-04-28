using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class Examination
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }



    }
}
