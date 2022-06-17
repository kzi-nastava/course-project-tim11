using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.HelperClasses;
using ClinicApp.Clinic;
using ClinicApp.Clinic.Rooms;

namespace ClinicApp.Menus.Admin.RoomRenovations
{
    class CreateComplexJoinRenovation
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
            CLI.CLIWriteLine("Enter ID of the room you want to Renovate");
            int otherRoomId = CLI.GetValidRoomId();
            if (otherRoomId == 0)
            {
                CLI.CLIWriteLine("You cannot delete Storage!");
                return;
            }
            if (OtherFunctions.CheckForExaminations(duration, otherRoomId))
            {
                CLI.CLIWriteLine("This room has an appointment at the given time, discarding");
                return;
            }

            RoomRenovationService.CreateComplexJoin(id, duration, otherRoomId);
        }
    }
}
