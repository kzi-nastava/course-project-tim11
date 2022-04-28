using System;

namespace ClinicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // First we load all data from our data bases into the working memorry
            // (implemented using the SystemFunctions class)
            SystemFunctions.LoadData();
            // Next we communicate with our user
            Menu.InitialDialog();
        }
    }
}
