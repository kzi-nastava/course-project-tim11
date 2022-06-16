using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.HelperClasses;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class CreateComplexSplitRenovation
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
            CLI.CLIWrite("Name of new room: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            CLI.CLIWrite("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = OtherFunctions.ChooseRoomType();
            RoomRenovationService.CreateComplexSplit(name, roomType, id, duration);
        }
    }
}
