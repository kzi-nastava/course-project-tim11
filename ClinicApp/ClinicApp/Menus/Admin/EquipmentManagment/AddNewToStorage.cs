using ClinicApp.Clinic.Equipments;
using System.Collections.Generic;

namespace ClinicApp.Menus.Admin.EquipmentManagment
{
    class AddNewToStorage
    {
        public static void Dialog()
        {
            List<string> exsistingNames = GetEquipmentNames();
            CLI.CLIWrite("Name: ");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (exsistingNames.Contains(name))
            {
                CLI.CLIWrite("Name already in use! Enter name: ");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            CLI.CLIWriteLine("Enter amount");
            int amount = CLI.CLIEnterNumber();
            CLI.CLIWriteLine("Choose!\n1. Operations\n2. RoomFurniture\n3. Hallway\n4. Examinations");
            EquipmentType type = OtherFunctions.ChooseEquipmentType();
            EquipmentService.AddNew(name, amount, type);
        }

        static List<string> GetEquipmentNames()
        {
            List<string> exsistingNames = new List<string>();
            foreach (Equipment item in EquipmentService.GetAll())
            {
                if (item.RoomId == 0)
                {
                    exsistingNames.Add(item.Name);
                }
            }
            return exsistingNames;
        }
    }
}
