using System;
using System.Diagnostics;

namespace Browser_Automated_Search_Console
{
    class Program
    {
        public static void Main()
        {
            Init();

            Messages.Header();
            Messages.Msg("Version: " + Helpers.GetVersion());
            Messages.Msg("Remember to save your browser work before proceeding!");

            Menu menu = new Menu();
            menu.MainMenu();
        }

        private static void Init()
        {
            Helpers.SetCurrentProcessTopMost();
            if (!Helpers.KillOtherRunningProcesses())
            {
                Messages.Warning("There is another application running..");
                Messages.Warning("It's strongly recommended to close the other application.");
                Messages.Warning("This application can running at the same time but probabily crash.");
            }

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            
        }
    }
}
