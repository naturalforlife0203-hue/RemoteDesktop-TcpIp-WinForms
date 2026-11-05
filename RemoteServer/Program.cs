using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RemoteServer
{
    internal static class Program
    {
        // Khai báo hàm tắt biến dạng DPI do Windows scale 125%/150%
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Server());
        }
    }
}