using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace HIDMacrosSwitcher
{
    static class IconHidingManager
    {
        // Import the required Windows API functions
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideWindow(string appName)
        {

            Process[] processes = Process.GetProcessesByName(appName);
            if (processes.Length > 0)
            {
                IntPtr hWnd = processes[0].MainWindowHandle;
                int currentStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                SetWindowLong(hWnd, GWL_EXSTYLE, currentStyle | WS_EX_TOOLWINDOW);
            }
            else
            {
                MessageBox.Show("Process not found!");
            }
        }
    }
}