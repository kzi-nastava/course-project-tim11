using System;
using System.Collections.Generic;
using System.Text;
using ClinicApp.Users;

namespace ClinicApp.Menus.Admin
{

    class Menu
    {
        public static int Write(User admin)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");
            Console.WriteLine("2: Display new messages (" + admin.MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Manage Clinic Rooms");
            Console.WriteLine("4: Manage Clinic Equipment");
            Console.WriteLine("5: Manage Room Renovations");
            Console.WriteLine("6: Manage Medicines");
            Console.WriteLine("7: Review Surveys");
            Console.WriteLine("0: Exit");

            return 7;
        }

        public static void Do(User admin, int option)
        {
            switch (option)
            {
                case 2:
                    admin.MessageBox.DisplayMessages();
                    break;
                case 3:
                    Rooms.RoomManagment.Menu();
                    break;
                case 4:
                    EquipmentManagment.EquipmentManagment.Menu();
                    break;
                case 5:
                    RoomRenovations.RoomRenovations.Menu();
                    break;
                case 6:
                    MedicineRequest.Medicines.Menu();
                    break;
                case 7:
                    Surveys.Surveys.Menu();
                    break;
            }
        }
    }
}
