using ClinicApp.Users;
using System;
using System.Linq;


namespace ClinicApp.Clinic
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
    }
}
