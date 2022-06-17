using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicApp.Clinic
{
    public class FreeDaysRequestService
    {
        public void RequestFreeDay(ref Doctor doctor, DateTime dateFrom, DateTime dateTo, bool urgent, string comment) {
            int id = FreeDaysRequestRepo.FreeDaysRequests.Last().ID + 1;
            FreeDaysState state;
            if (urgent) state = FreeDaysState.Accepted;
            else state = FreeDaysState.Waiting;

            FreeDaysRequest request = new FreeDaysRequest(id, doctor, DateTime.Today, dateFrom, dateTo, state, urgent, comment);
            FreeDaysRequestRepo.Add(request);
        }

        public void GatherInfoFreeDayRequest(ref Doctor doctor) {
            Console.WriteLine("Is this request urgent(y/n)?");
            bool urgent = CLI.EnterBool();
            Console.WriteLine("Enter the start date for your free days (e.g. 22/10/1987): ");
            DateTime dateFrom = CLI.CLIEnterNonPastDate();
            if (DateTime.Today.AddDays(2) <= dateFrom && !urgent) {
                Console.WriteLine("You have to request free days at least two days before your leave!");
                return;
            }
            Console.WriteLine("Enter the end date for your free days (e.g. 22/10/1987): ");
            DateTime dateTo = CLI.CLIEnterNonPastDate();
            if(dateTo < dateFrom) {
                Console.WriteLine("Invalid end date.");
                return;
            }
            if(!DoctorService.IsDoctorFree(dateFrom, dateTo, doctor)) {
                Console.WriteLine("You are not free, you have scheduled appointments in that time period!");
                return;
            }
            if((dateTo - dateFrom).TotalDays > 5 && urgent)
            {
                Console.WriteLine("An urgent request can't be longer than 5 days.");
                return;
            }
            Console.WriteLine("Enter the reason behind your request?");
            string reason = Console.ReadLine();
            RequestFreeDay(ref doctor, dateFrom, dateTo, urgent, reason);
            Console.WriteLine("Successfully created your request.");
        }
       

    }
}
