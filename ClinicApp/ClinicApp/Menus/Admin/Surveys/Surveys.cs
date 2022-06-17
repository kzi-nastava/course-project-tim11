using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin.Surveys
{
    public class Surveys
    {
        public static void Menu()
        {
            while (true)
            {
                CLI.CLIWriteLine("Review surveys");
                CLI.CLIWriteLine("1. Review clinic surveys");
                CLI.CLIWriteLine("2. Review doctor surveys");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 4);
                switch (choice)
                {
                    case 1:

                        break;
                    case 2:
                        break;
                    case 0:
                        return;
                    default:
                        CLI.CLIWriteLine("Invalid option, try again");
                        break;
                }
            }
        }
    }
}
