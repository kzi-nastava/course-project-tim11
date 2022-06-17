using ClinicApp.Clinic;
using ClinicApp.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Dialogs
{
    public static class FreeDaysRequestDialog
    {
        public static void GatherInfoFreeDayRequest(ref Doctor doctor)
        {
            Console.WriteLine("Is this request urgent(y/n)?");
            bool urgent = OtherFunctions.EnterBool();
            Console.WriteLine("Enter the start date for your free days (e.g. 22/10/1987): ");
            DateTime dateFrom = OtherFunctions.GetGoodDate();
            if (DateTime.Today.AddDays(2) >= dateFrom && !urgent)
            {
                Console.WriteLine("You have to request free days at least two days before your leave!");
                return;
            }
            Console.WriteLine("Enter the end date for your free days (e.g. 22/10/1987): ");
            DateTime dateTo = OtherFunctions.GetGoodDate();
            if (dateTo < dateFrom)
            {
                Console.WriteLine("Invalid end date.");
                return;
            }
            if (!DoctorService.IsDoctorFree(dateFrom, dateTo, doctor))
            {
                Console.WriteLine("You are not free, you have scheduled appointments in that time period!");
                return;
            }
            if ((dateTo - dateFrom).TotalDays > 5 && urgent)
            {
                Console.WriteLine("An urgent request can't be longer than 5 days.");
                return;
            }
            Console.WriteLine("Enter the reason behind your request?");
            string reason = Console.ReadLine();
            FreeDaysRequestService.RequestFreeDay(ref doctor, dateFrom, dateTo, urgent, reason);
            Console.WriteLine("Successfully created your request.");
        }

        public static void ViewFreeDays(Doctor doctor)
        {
            bool found = false;
            foreach(FreeDaysRequest request in FreeDaysRequestRepo.FreeDaysRequests)
            {
                if(request.Doctor.UserName == doctor.UserName)
                {
                    ViewRequest(request);
                    Console.WriteLine();
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("No requests for free days found.");
            }

        }
        public static void ViewRequest(FreeDaysRequest request)
        {
            Console.WriteLine($"REQUEST FOR DAYS OFF ID: {request.ID}");
            Console.WriteLine($"From: {request.DateFrom.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"To: {request.DateTo.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"For: {request.Doctor.Name} {request.Doctor.LastName}, {request.Doctor.UserName}");
            Console.WriteLine($"Created: {request.DateCreated.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"State: {request.State}");
            Console.WriteLine($"Urgent: {request.Urgent}");
        }

        public static void ViewAllRequests()
        {
            foreach (FreeDaysRequest request in FreeDaysRequestRepo.FreeDaysRequests)
            {
                    ViewRequest(request);
                    Console.WriteLine();
                
            }
        }
    }
}
