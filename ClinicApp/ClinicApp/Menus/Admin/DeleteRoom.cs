using ClinicApp.AdminFunctions;

namespace ClinicApp.Menus.Admin
{
    internal class DeleteRoom
    {
        public static void Menu()
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