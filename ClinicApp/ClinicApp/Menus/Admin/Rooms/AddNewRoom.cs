using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;

namespace ClinicApp.Menus.Admin
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
