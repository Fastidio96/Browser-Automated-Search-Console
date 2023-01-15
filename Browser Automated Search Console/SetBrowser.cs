namespace Browser_Automated_Search_Console
{
    public class SetBrowser
    {
        public enum BrowserAvailable : byte
        {
            Chrome = 0,
            Firefox = 1,
            Edge = 2
        }

        public static string ChromePath()
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                return @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            }

            return @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
        }
        public static string FirefoxPath()
        {
            return @"C:\Program Files\Mozilla Firefox\firefox.exe";
        }
        public static string EdgePath()
        {
            return @"C:\Program Files (x86)\Microsoft\Edge\application\msedge.exe";
        }
    }
}
