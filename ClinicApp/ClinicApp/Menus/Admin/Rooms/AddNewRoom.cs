using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;
using ClinicApp.Clinic.Rooms;

namespace ClinicApp.Menus.Admin.Rooms
{
    class AddNewRoom
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Enter name: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            CLI.CLIWriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = OtherFunctions.ChooseRoomType();

            RoomService.AddNewRoom(name, roomType);
        }
    }
}
