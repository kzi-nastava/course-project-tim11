using ClinicApp.Users;
using System.Collections.Generic;

namespace ClinicApp.Clinic.Search
{
    public class SearchDoctors
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FieldOfWork { get; set; }
        public List<Doctor> results { get; set; }
        public bool FilterByName { get; set; }
        public bool FilterByLastName { get; set;}
        public bool FilterByField { get; set; }
        public SearchDoctors()
        {
        }
        
    }
}
