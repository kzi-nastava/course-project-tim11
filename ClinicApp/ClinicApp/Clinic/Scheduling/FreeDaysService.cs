using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class FreeDaysService
    {
        public static bool IsDoctorFree(DateTime startDate, DateTime endDate, Doctor doctor)
        {
            foreach (Appointment appointment in doctor.Appointments)
            {
                if (appointment.DateTime <= endDate || appointment.DateTime >= startDate)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDoctorAvailible(DateTime date, Doctor doctor)
        {
            foreach (FreeDaysRequest request in FreeDaysRequestRepo.FreeDaysRequests)
            {
                if (request.Doctor.UserName == doctor.UserName && request.State == FreeDaysState.Accepted)
                {
                    if (date.Date >= request.DateFrom && date.Date <= request.DateTo)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
