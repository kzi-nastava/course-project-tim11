using System;
using System.Collections.Generic;

namespace ClinicApp.Users
{
    public class AntiTrollService
    {
        public AntiTrollService()
        {
        }

        public void AntiTroll()
        {
            int update_delete = 0;
            int make = 0;
            DateTime today = DateTime.Now;
            DateTime monthBefore = DateTime.Now - TimeSpan.FromDays(30);
            /*
            foreach (KeyValuePair<DateTime, string> activity in ActivityHistory)
            {
                DateTime date = activity.Key;
                string activity_performed = activity.Value;
                int lower_limit = DateTime.Compare(date, monthBefore);
                int upper_limit = DateTime.Compare(date, today);
                if (lower_limit < 0 || upper_limit > 0)
                {
                    continue;
                }
                if (activity_performed == "MAKE")
                {
                    make += 1;
                }
                else
                {
                    update_delete += 1;
                }
                if (make >= 8 || update_delete >= 5)
                {
                    Console.WriteLine("You have been blocked by system.");
                    this.Blocked = Blocked.System;
                    return;
                }
            }
            */
        }
            
    }
}
