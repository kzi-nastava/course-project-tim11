using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class AddNewRoom
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("Enter name: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            CLI.CLIWriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
            RoomType roomType = OtherFunctions.ChooseRoomType();

            RoomService.AddNewRoom(name, roomType);
        }
    }
}
