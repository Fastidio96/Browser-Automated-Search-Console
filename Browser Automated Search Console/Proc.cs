using System;
using System.Diagnostics;
using System.Threading;

namespace Browser_Automated_Search_Console
{
    /// <summary>
    /// This class is used for the management of the processes
    /// </summary>
    public class Proc : IDisposable
    {
        private SetBrowser.BrowserAvailable _selectedBrowser { get; set; }
        private bool _isMobile { get; set; }
        private bool _isRandom { get; set; }
        private int _numSearches { get; set; }

        private ProcessStartInfo _psi;
        private bool disposedValue;

        public Proc(SetBrowser.BrowserAvailable selectedBrowser, bool isMobile, bool isRandom, int numSearches)
        {
            _selectedBrowser = selectedBrowser;
            _isMobile = isMobile;
            _isRandom = isRandom;
            _numSearches = numSearches;

            Init();
        }

        private bool Init()
        {
            Messages.Clear();

            string pathBrowser;
            switch (_selectedBrowser)
            {
                case SetBrowser.BrowserAvailable.Chrome:
                    {
                        pathBrowser = SetBrowser.ChromePath();
                        break;
                    }
                case SetBrowser.BrowserAvailable.Firefox:
                    {
                        pathBrowser = SetBrowser.FirefoxPath();
                        break;
                    }
                case SetBrowser.BrowserAvailable.Edge:
                    {
                        pathBrowser = SetBrowser.EdgePath();
                        break;
                    }
                default:
                    {
                        pathBrowser = SetBrowser.EdgePath();
                        break;
                    }
            }

            //Set the args for process
            _psi = new ProcessStartInfo(pathBrowser);
            _psi.UseShellExecute = true;
            //psi.CreateNoWindow = true;
            _psi.WindowStyle = ProcessWindowStyle.Minimized;

            return KillProcs();
        }

        public bool Start()
        {
            for (int i = 1; i <= _numSearches; i++)
            {
                Word searchQuery = new Word(_isRandom);
                _psi.Arguments = string.Format("{0} \"https://www.bing.com/search?q=what+is+{1}\"",
                    BrowserUA(_isMobile), searchQuery);

                //Open the process
                try
                {
                    Process process = Process.Start(_psi);
                    process.Dispose();
                }
                catch (Exception ex)
                {
                    Messages.Error("An error occurred while opening the process.", ex);
                    return false;
                }

                Messages.Log(string.Format("Done searches: {0}/{1}", i, _numSearches));

                //Do some sleep
                int msSleep = new Random().Next(3000, 7000);
                Messages.Log(string.Format("Sleeping for {0} seconds..", msSleep / 1000));
                Thread.Sleep(msSleep);

                //If is opened more than 5 processes proceed with kill
                if ((i % 5) == 0)
                {
                    KillProcs();
                }

                Messages.Clear();
            }
            //When the method is completed close the remain browser's tab
            KillProcs();

            return true;
        }

        public void Stop()
        {
            this.Dispose();
        }

        private bool KillProcs(string processName = null)
        {
            processName = processName ?? Enum.GetName(typeof(SetBrowser.BrowserAvailable), _selectedBrowser).ToLower();

            if (processName == "edge" || processName == "msedge")
            {
                processName = "msedge";
                if (!KillProcs("msedgewebview2"))
                {
                    return false;
                }
            }

            try
            {
                if (IsRunning(processName))
                {
                    foreach (Process procs in Process.GetProcessesByName(processName))
                    {
                        if (procs.HasExited)
                        {
                            continue;
                        }

                        procs.Kill();
                        procs.Dispose();
                    }
                }
            }
            // This exception is very rare, when it happens there is no need to be worried.
            // The browser made some trash or the process doesn't exist anymore because it's crashed.
            catch (InvalidOperationException) { } // Do nothing
            //Security exception
            catch (System.ComponentModel.Win32Exception ex)
            {
                Messages.Error(string.Format("A security exception has occurred while killing the process \"{0}\".", processName), ex);
                return false;
            }

            return true;
        }

        private bool IsRunning(string processName)
        {
            Process[] pname = Process.GetProcessesByName(processName);
            return pname.Length > 0;
        }

        /// <param name="isMobile"></param>
        /// <returns>The user agent (desktop or mobile)</returns>
        private string BrowserUA(bool isMobile)
        {
            string mobileUA = "Mozilla/5.0 (Linux; U; Android 4.4.2; en-us; SCH-I535 Build/KOT49H) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
            string desktopUA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";

            return string.Format("--user-agent=\"{0}\" ", isMobile ? mobileUA : desktopUA);
        }

        #region IDisposable support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _psi = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null

                KillProcs();
                

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Proc()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
