using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Menus.Admin.Surveys
{
    class DoctorSurveySubmenu
    {
        public static void Menu()
        {
            while (true)
            {
                CLI.CLIWriteLine("Review doctor surveys");
                CLI.CLIWriteLine("1. Review averages");
                CLI.CLIWriteLine("2. Review comments");
                CLI.CLIWriteLine("3. Review histograms");
                CLI.CLIWriteLine("4. Top 3 doctors");
                CLI.CLIWriteLine("5. Bottom 3 doctors");
                CLI.CLIWriteLine("0 to return");
                int choice = CLI.CLIEnterNumberWithLimit(0, 4);
                switch (choice)
                {
                    case 1:
                        DoctorSurveyViews.Averages();
                        break;
                    case 2:
                        DoctorSurveyViews.Comments();
                        break;
                    case 3:
                        DoctorSurveyViews.Histograms();
                        break;
                    case 4:
                        DoctorSurveyViews.BestThree();
                        break;
                    case 5:
                        DoctorSurveyViews.BottomThree();
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
