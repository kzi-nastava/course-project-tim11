using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin.EquipmentManagment
{
    class StorageSubmenu
    {
        public static void Menu()
        {
            CLI.CLIWriteLine("1. Add new Equipment");
            CLI.CLIWriteLine("2. Edit amount of existing Equipment");
            int answer = CLI.CLIEnterNumberWithLimit(0, 2);
            switch (answer)
            {
                case 1:
                    AddNewToStorage.Dialog();
                    break;
                case 2:
                    EditExistingEq.Dialog();
                    break;
                case 0:
                    return;
            }
        }
    }
}
