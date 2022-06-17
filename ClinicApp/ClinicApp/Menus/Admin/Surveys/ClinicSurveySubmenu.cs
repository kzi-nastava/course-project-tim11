using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin
{
    class ClinicSurveySubmenu
    {
        public static void Menu()
        {
            while (true)
            {
                CLI.CLIWriteLine("Review clinic surveys");
                CLI.CLIWriteLine("1. Review averages");
                CLI.CLIWriteLine("2. Review comments");
                CLI.CLIWriteLine("3. Review histograms");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 4);
                switch (choice)
                {
                    case 1:
                        ClinicSurveyViews.Averages();
                        break;
                    case 2:
                        ClinicSurveyViews.Comments();
                        break;
                    case 3:
                        ClinicSurveyViews.Histograms();
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
