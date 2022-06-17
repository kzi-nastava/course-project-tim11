using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;

namespace ClinicApp.Menus.Admin
{
    class EditRoom
    {
        public static void Dialog()
        {
            string name;
            RoomType roomType;
            int id = CLI.GetValidRoomId();
            Room room = RoomRepository.Get(id);
            if (room.Id == 0)
            {
                CLI.CLIWriteLine("You cannot edit Storage!");
                return;
            }
            CLI.CLIWriteLine("Do you wish to edit this rooms' name? Y/N");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                CLI.CLIWriteLine("Enter new name: ");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            else name = room.Name;
            CLI.CLIWriteLine("Do you wish to edit this rooms' type? Y/N");
            answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                CLI.CLIWriteLine("\nChoose Type (1 for Operations, 2 for Examinations, 3 for Waiting): ");
                roomType = OtherFunctions.ChooseRoomType();
            }
            else roomType = room.Type;

            RoomService.EditRoom(room.Id, name, roomType);
        }
    }
}
