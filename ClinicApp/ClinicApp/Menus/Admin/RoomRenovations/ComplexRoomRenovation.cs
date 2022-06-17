using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin.RoomRenovations
{
    class ComplexRoomRenovation
    {
        public static void Menu()
        {

            CLI.CLIWriteLine("1. Split room");
            CLI.CLIWriteLine("2. Join 2 rooms");
            CLI.CLIWriteLine("0. Return");
            int answer = CLI.CLIEnterNumberWithLimit(0, 2);
            switch (answer)
            {
                case 0:
                    return;
                case 1:
                    CreateComplexSplitRenovation.Dialog();
                    return;
                case 2:
                    CreateComplexJoinRenovation.Dialog();
                    return;
                default:
                    CLI.CLIWriteLine("Invalid option, try again");
                    break;
            }
        }
    }
}
