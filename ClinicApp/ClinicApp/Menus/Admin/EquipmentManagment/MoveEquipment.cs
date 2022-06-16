using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin
{
    class MoveEquipment
    {
        public static void Menu()
        {
            while (true)
            {
                CLI.CLIWriteLine("1. Move Equipment");
                CLI.CLIWriteLine("2. Add new Equipment to Storage");
                CLI.CLIWriteLine("0 to return");
                int answer = CLI.CLIEnterNumberWithLimit(0, 2);
                switch (answer)
                {
                    case 0:
                        return;
                    case 1:
                        CreateMovement.Dialog();
                        break;
                    case 2:
                        AddEqToStorage.Dialog();
                        break;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
