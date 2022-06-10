using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicApp.Users
{
    public class SearchDoctorService
    {
        public SearchDoctorService()
        {
        }

        public void SearchUI()
        {
            SearchDoctors search = new SearchDoctors();
            CLI.CLIWriteLine("Do you want to search doctors by their first name? y/n");
            string answer = CLI.CLIEnterString();
            if(answer.ToLower() == "y")
            {
                search.FilterByName = true;
                CLI.CLIWriteLine("Please enter your doctors name.");
                search.FirstName = CLI.CLIEnterString();
            }
            CLI.CLIWriteLine("Do you want to search doctors by their last name? y/n");
            if (answer.ToLower() == "y")
            {
                search.FilterByLastName = true;
                CLI.CLIWriteLine("Please enter your doctors last name.");
                search.LastName = CLI.CLIEnterString();
            }
            CLI.CLIWriteLine("Do you want to search doctors by their field of work? y/n");
            if (answer.ToLower() == "y")
            {
                search.FilterByField = true;
                CLI.CLIWriteLine("Please enter your doctors field of work.");
                search.FieldOfWork = CLI.CLIEnterString();
            }
            List<Doctor> result = Search(search);

            //TODO sort i ili ispis
        }

        public List<Doctor> Search(SearchDoctors search)
        {
            List<Doctor> results = UserRepository.Doctors.Values.ToList();
            if (search.FilterByName == true)
            {
                results = FilterByName(results,search.FirstName);
            }
            if (search.FilterByLastName == true)
            {
                results = FilterByLastName(results, search.LastName);
            }
            if (search.FilterByField == true)
            {
                results = FilterByField(results, search.FieldOfWork);
            }
            return results;
            
        }

        public List<Doctor> FilterByName(List<Doctor> doctors, string name)
        {
            var results = new List<Doctor>();
            foreach(Doctor doctor in doctors)
            {
                if(doctor.Name == name)
                {
                    results.Add(doctor);
                }
            }
            return results;
        }

        public List<Doctor> FilterByLastName(List<Doctor> doctors, string lastName)
        {
            var results = new List<Doctor>();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.LastName == lastName)
                {
                    results.Add(doctor);
                }
            }
            return results;
        }

        public List<Doctor> FilterByField(List<Doctor> doctors, string field)
        {
            var results = new List<Doctor>();
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Field.ToString() == field.ToUpper())
                {
                    results.Add(doctor);
                }
            }
            return results;
        }
    }
}
