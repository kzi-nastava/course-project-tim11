using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.HelperClasses;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class SimpleRoomRenovation
    {
        public static void Dialog()
        {

            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int id = CLI.GetValidRoomId();
            if (id == 0)
            {
                CLI.CLIWriteLine("You cannot renovate Storage!");
                return;
            }
            DateRange duration = RoomRenovations.GetUninterruptedDateRange(id);
            RoomRenovationService.CreateSimpleRenovation(id, duration);
        }
    }
}
