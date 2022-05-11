using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp
{
    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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
