using System;

namespace ClinicApp.HelperClasses
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMinutes(1);
        }

        public DateRange(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
        }

        public bool IsInRange(DateTime date)
        {
            return (date >= StartDate && date <= EndDate);
        }
        public bool IsOverlaping(DateRange other)
        {
            if (other.StartDate < StartDate && other.EndDate > StartDate)
                return true;
            if (other.StartDate > StartDate && other.StartDate < EndDate)
                return true;
            return false;
        }
        public void ValidateDates()
        {
            if (StartDate > EndDate)
            {
                DateTime x = StartDate;
                StartDate = EndDate;
                EndDate = x;
            }
        }
    }
}
