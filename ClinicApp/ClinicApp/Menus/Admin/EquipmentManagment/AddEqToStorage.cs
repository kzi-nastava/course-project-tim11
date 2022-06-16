using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin
{
    class AddEqToStorage
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("List Equipment in Storage? (y/n): ");
            string answer = CLI.CLIEnterString();
            if (answer.ToLower() == "y")
            {
                EquipmentManagment.ListAllEquipmentInRoom(0);
            }
            StorageSubmenu.Menu();
        }
    }
}
