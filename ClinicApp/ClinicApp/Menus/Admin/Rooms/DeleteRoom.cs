using ClinicApp.Clinic;
using ClinicApp.Clinic.Rooms;

namespace ClinicApp.Menus.Admin.Rooms
{
    internal class DeleteRoom
    {
        public static void Dialog()
        {
            CLI.CLIWriteLine("Enter ID of the room you want to Delete");
            int id = CLI.GetValidRoomId();
            if (id == 0)
            {
                CLI.CLIWriteLine("You cannot delete Storage!");
                return;
            }
            RoomService.DeleteRoom(id);
        }
    }
}