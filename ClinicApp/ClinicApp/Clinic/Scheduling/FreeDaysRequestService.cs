using ClinicApp.Clinic.Appointmens;
using ClinicApp.Users;
using System;
using System.Linq;


namespace ClinicApp.Clinic.Scheduling
{
    public class FreeDaysRequestService
    {
        public static void RequestFreeDay(ref Doctor doctor, DateTime dateFrom, DateTime dateTo, bool urgent, string comment) {
            int id;
            if (FreeDaysRequestRepo.FreeDaysRequests.Count == 0)
            {
                id = 1;
            }
            else
            {
                id = FreeDaysRequestRepo.FreeDaysRequests.Last().ID + 1;
            }
            FreeDaysState state;
            if (urgent) state = FreeDaysState.Accepted;
            else state = FreeDaysState.Waiting;

            FreeDaysRequest request = new FreeDaysRequest(id, doctor, DateTime.Today, dateFrom, dateTo, state, urgent, comment);
            FreeDaysRequestRepo.Add(request);

        }
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
                if (request.Doctor.UserName == doctor.UserName)
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
