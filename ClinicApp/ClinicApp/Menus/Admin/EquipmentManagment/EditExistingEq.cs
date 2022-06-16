using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    class EditExistingEq
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Enter ID of equipment to change:");
            int id = CLI.GetValidEquipmentId();
            Equipment eq = EquipmentService.Get(id);
            if (eq.RoomId != 0)
            {
                CLI.CLIWriteLine("Equipment not in Storage cannot be edited directly, use the option 1. in the Manage Equipment menu");
                return;
            }
            else
            {
                CLI.CLIWriteLine("Enter new amount: ");
                int amount = CLI.CLIEnterNumberWithLimit(1, 99999999);
                EquipmentService.Update(eq.Id, amount);
            }
        }
    }
}
