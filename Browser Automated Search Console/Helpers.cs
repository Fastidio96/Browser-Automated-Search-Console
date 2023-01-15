using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Browser_Automated_Search_Console
{
    public class Helpers
    {
        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos
        (
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            int uFlags
        );

        public static void SetCurrentProcessTopMost()
        {
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

            SetWindowPos
            (
                hWnd,
                new IntPtr(HWND_TOPMOST),
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE
            );
        }

        public static bool IsAnotherProcessRunning(out Process[] otherProcesses)
        {
            string appName = Process.GetCurrentProcess().ProcessName;
            int appPID = Process.GetCurrentProcess().Id;

            otherProcesses = Process.GetProcessesByName(appName);

            return otherProcesses != null && otherProcesses.Length > 1 && otherProcesses.Any(p => p.Id != appPID);
        }

        public static bool KillOtherRunningProcesses()
        {
            try
            {
                if (IsAnotherProcessRunning(out Process[] processes))
                {
                    foreach (Process p in processes)
                    {
                        if (!p.WaitForExit(5000))
                        {
                            p.Kill();
                            p.Dispose();
                        }
                    }
                }
            }
            catch (Win32Exception winEx)
            {
                Messages.Error("A security exception has occurred while killing the process!", winEx);
                return false;
            }
            catch(Exception ex)
            {
                Messages.Error("", ex);
                return false;
            }

            return true;
        }

        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


    }
}
