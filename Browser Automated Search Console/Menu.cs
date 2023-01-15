using System;
using System.Threading;

namespace Browser_Automated_Search_Console
{
    public class Menu
    {

        /// <summary>
        /// Platform choice - Menu 1
        /// </summary>
        public void MainMenu()
        {
            Messages.GiveMeSomeSpace();
            Messages.Log("Choose the platform:");
            Messages.Log("1. Desktop --default");
            Messages.Log("2. Mobile");

            switch (Console.ReadLine())
            {
                case "1":
                    BrowserMenu(false);
                    break;
                case "2":
                    BrowserMenu(true);
                    break;
                default:
                    Messages.Log("Desktop", true);
                    BrowserMenu(false);
                    break;
            }
        }

        /// <summary>
        /// Browser choice - Menu 2
        /// </summary>
        /// <param name="isMobile"></param>
        public void BrowserMenu(bool isMobile)
        {
            Messages.GiveMeSomeSpace();
            Messages.Log("Choose the browser:");
            Messages.Log("1. Edge --default");
            Messages.Log("2. Chrome");
            Messages.Log("3. Firefox");

            switch (Console.ReadLine())
            {
                case "1":
                    WordMenu(SetBrowser.BrowserAvailable.Edge, isMobile);
                    break;
                case "2":
                    WordMenu(SetBrowser.BrowserAvailable.Chrome, isMobile);
                    break;
                case "3":
                    WordMenu(SetBrowser.BrowserAvailable.Firefox, isMobile);
                    break;
                default:
                    Messages.Log("Edge", true);
                    WordMenu(SetBrowser.BrowserAvailable.Edge, isMobile);
                    break;
            }
        }

        /// <summary>
        /// Word generation choice - Menu 3
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="isMobile"></param>
        public void WordMenu(SetBrowser.BrowserAvailable browser,bool isMobile)
        {
            Messages.GiveMeSomeSpace();
            Messages.Log("Choose the method for generating the words:");
            Messages.Log("1. Random --default");
            Messages.Log("2. Wordlist");

            switch (Console.ReadLine())
            {
                case "1":
                    SearchesMenu(browser, isMobile, true);
                    break;
                case "2":
                    SearchesMenu(browser, isMobile, false);
                    break;
                default:
                    Messages.Log("Random", true);
                    SearchesMenu(browser, isMobile, true);
                    break;
            }
        }

        /// <summary>
        /// Number of searches - Menu 4
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="isMobile"></param>
        /// <param name="isRandom"></param>
        public void SearchesMenu(SetBrowser.BrowserAvailable browser,bool isMobile,bool isRandom)
        {
            Messages.GiveMeSomeSpace();

            int numSearch;

            if (isMobile)
            {
                Messages.Log("Set number of mobile searches [1-22]: ", true);
                string answer = Console.ReadLine();

                if (Int32.TryParse(answer,out numSearch))
                {
                    if (numSearch < 1)
                    {
                        Messages.Error("Please insert a number greater than 0.");
                        SearchesMenu(browser, isMobile, isRandom);
                    }
                    else if (numSearch > 22)
                    {
                        Messages.Error("Please insert a number lesser than 22.");
                        SearchesMenu(browser, isMobile, isRandom);
                    }
                }
                else //default
                {
                    numSearch = 5;
                    Messages.Log("5", true);
                }
            }
            else
            {
                Messages.Log("Set number of desktop searches [1-34]: ", true);
                string answer = Console.ReadLine();

                if (Int32.TryParse(answer, out numSearch))
                {
                    if (numSearch < 1)
                    {
                        Messages.Error("Please insert a number greater than 0.");
                        SearchesMenu(browser, isMobile, isRandom);
                    }
                    else if (numSearch > 34)
                    {
                        Messages.Error("Please insert a number lesser than 34.");
                        SearchesMenu(browser, isMobile, isRandom);
                    }
                }
                else //default
                {
                    numSearch = 5;
                    Messages.Log("5", true);
                }
            }

            //Run the processes
            using (Proc proc = new Proc(browser, isMobile, isRandom, numSearch))
            {
                //AppDomain.CurrentDomain.ProcessExit += delegate (object sender, EventArgs e)
                //{
                //    Messages.Msg("Exiting..");
                //    proc.Stop();
                //    Thread.Sleep(1000); 
                //};

                if (!proc.Start())
                {
                    Messages.Warning("Searches has failed to complete.");
                }
                else
                {
                    Messages.Msg("Searches completed successfully.");
                }
            }

            //Return to Main Menu
            MainMenu();
        }
    }
}
