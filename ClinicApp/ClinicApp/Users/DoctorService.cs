using System;
using System.Collections.Generic;

namespace ClinicApp.Users
{
    public class DoctorService
    {
        public DoctorService()
        {
        }

        public static void ViewAllDoctors()
        {
            int i = 1;
            foreach (KeyValuePair<string, Doctor> entry in UserRepository.Doctors)
            {
                Doctor doctor = entry.Value;
                Console.WriteLine($"{i}.User name: {doctor.UserName} ; Name: {doctor.Name}; Last Name: {doctor.LastName}");
                i++;
            }
        }

        public static List<Doctor> SortDoctorsByFirstName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.Name.CompareTo(d2.Name);
            });
            return doctorsUnsorted;
        }

        public static List<Doctor> SortDoctorsByLastName(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }

        public static List<Doctor> SortDoctorsByField(List<Doctor> doctorsUnsorted)
        {
            doctorsUnsorted.Sort(delegate (Doctor d1, Doctor d2)
            {
                return d1.LastName.CompareTo(d2.LastName);
            });
            return doctorsUnsorted;
        }
        
    }
}
