using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static void ManageFreeDayRequests()
        {
            int option;
            string reason;
            foreach(FreeDaysRequest request in FreeDaysRequestRepo.FreeDaysRequests)
                if(request.State == FreeDaysState.Waiting)
                {
                    CLI.CLIWriteLine("\nDoctor: " + request.Doctor.UserName);
                    CLI.CLIWriteLine("From: " + request.DateFrom);
                    CLI.CLIWriteLine("To: " + request.DateTo);
                    CLI.CLIWriteLine("Urgent: " + request.Urgent);
                    CLI.CLIWriteLine("Reason: " + request.Reason);

                    CLI.CLIWriteLine("\n1: Approve");
                    CLI.CLIWriteLine("2: Deny");
                    CLI.CLIWriteLine("0: Back to menu");
                    option = OtherFunctions.EnterNumberWithLimit(0, 2);
                    if (option == 0)
                        return;
                    if(option == 1)
                    {
                        request.State = FreeDaysState.Accepted;
                        request.Doctor.MessageBox.AddMessage("Your free days request has been approved.");
                    }
                    if(option == 2)
                    {
                        CLI.CLIWriteLine("Enter the reason for denying this request:");
                        reason = CLI.CLIEnterString();
                        request.State = FreeDaysState.Denied;
                        request.Doctor.MessageBox.AddMessage("Your free days request has been denied. Reason: " + reason);
                    }
                }
            CLI.CLIWriteLine("There are no free days requests left.");
        }
    }
}
