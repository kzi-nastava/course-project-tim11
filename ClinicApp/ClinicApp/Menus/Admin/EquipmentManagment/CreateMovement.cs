using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Clinic;
using ClinicApp.Clinic.Equipments;

namespace ClinicApp.Menus.Admin.EquipmentManagment
{
    class CreateMovement
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Enter ID of equipment to change:");
            int id = CLI.GetValidEquipmentId();
            CLI.CLIWriteLine("Enter amount to move");
            int amount = CLI.CLIEnterNumberWithLimit(1, 100);
            CLI.CLIWriteLine("Enter the Id of the room where the equipment is going to");
            int roomId = CLI.GetValidRoomId();
            CLI.CLIWriteLine("Enter date on which the equipment is being moved");
            DateTime date = CLI.CLIEnterDate();

            EquipmentMovementService.MoveEquipment(id, amount, roomId, date);
        }
    }
}
