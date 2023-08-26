using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;


namespace HIDMacrosSwitcher
{
    public partial class Form1 : Form
    {
        HIDMacrosConfigManager manager = new HIDMacrosConfigManager();
        public MacroSwitcher Universal = new MacroSwitcher() { processName = "", xmlFile = ".\\Universal.xml" };
        public string SettingsFile = ".\\Main-Settings.xml";
        public string MainSaveFile = "..\\hidmacros.xml";
        public string Folders = ".\\Applications\\";
        public List<MacroSwitcher> switcher = new List<MacroSwitcher>();

        public MacroSwitcher lastTick;
        public bool hasChanged = false;

        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory("./Applications");
            try
            {
                foreach (string file in Directory.GetFiles(Folders))
                {
                    MacroSwitcher thisfile = new MacroSwitcher();
                    thisfile.xmlFile = file;
                    string[] split = file.Split('\\');
                    thisfile.processName = split[split.Length - 1];
                    Console.WriteLine("Added entry with name: " + thisfile.processName, ", and xml file being: " + thisfile.xmlFile);
                    switcher.Add(thisfile);
                }
            }
            catch(Exception exc)
            {
                DetectionTimer.Stop();
                MessageBox.Show(exc.Message + "\nBe sure to have the .exe and all files present in the HIDMacros/Switcher folder! Then, restart the program in the new folder!");
                this.Close();
            }
        }

        private void DetectionTimer_Tick(object sender, EventArgs e)
        {
            //Find the current running process
            bool FoundProcessInList = false;
            string name = ProcessUtils.GetActiveProcessName();
            Console.WriteLine(name);

            //Detect if the process running has a macro file with it.
            foreach(MacroSwitcher thisMacro in switcher)
            {
                //Running this with +".xml" so we can use the file name.
                if(thisMacro.processName == name+".xml")
                {
                    //We don't want to restart the program if we already are on the macro.
                    if (lastTick == thisMacro) { FoundProcessInList = true;  return; }
                    //Set the macro
                    FoundProcessInList = SetMacroFile(thisMacro);
                }
                else
                {
                    continue;
                }
            }
            //If we didn't find the macro, we just set it to be the universal one.
            if(FoundProcessInList == false)
            {
                if (lastTick == Universal) { FoundProcessInList = true; return; }
                Console.WriteLine("Couldn't find name, falling back to default.");
                FoundProcessInList = SetMacroFile(Universal);
            }
            //Now this is to see if we want to restart the program to apply the changes to the program
            if(hasChanged)
            {
                //Reopen the program so it will actually work
                string programPath = @"..\HIDMacros.exe";
                OpenProgram(programPath);
            }
        }

        private bool SetMacroFile(MacroSwitcher thisMacro)
        {
            try
            {
                //Close the program to ensure no errors will follow
                CloseApplicationByExeName("HIDMacros.exe");

                //Read the configs and combine them until it works
                Config newConfigToSet = manager.ReadConfigFromXml(SettingsFile);
                Config MacrosFromOldConfig = manager.ReadConfigFromXml(thisMacro.xmlFile);
                Config UniversalConfig = manager.ReadConfigFromXml(Universal.xmlFile);
                if(newConfigToSet == null||MacrosFromOldConfig == null || UniversalConfig == null)
                {
                    throw new Exception();
                }
                List<int> keycodesUsed = new List<int>();
                //Add any macros in the macro file to the macro list for the new one.
                foreach (Macro macroToAdd in MacrosFromOldConfig.Macros.MacroList)
                {
                    newConfigToSet.Macros.MacroList.Add(macroToAdd);
                    keycodesUsed.Add(macroToAdd.KeyCode);
                }
                //Add the universal keys if it does not exist already...
                foreach (Macro universalMacro in UniversalConfig.Macros.MacroList)
                {
                    //Make sure it has not already been used.
                    if (!keycodesUsed.Contains(universalMacro.KeyCode))
                    {
                        newConfigToSet.Macros.MacroList.Add(universalMacro);
                    }
                }
                //Now that we have changed the file, we want to inform the program to restart HID Macros so the changes can be applied.
                hasChanged = true;
                lastTick = thisMacro;
                manager.SaveConfigToXml(MainSaveFile, newConfigToSet);
                return true;
            }
            catch
            {
                DetectionTimer.Stop();
                MessageBox.Show("Could not read the settings file, macro file, or universal config file! Please make sure you have these files in your root directory, and that the root directory in inside you hidmacros folder!");
                this.Close();
                return false;
            }
        }
        public static void CloseApplicationByExeName(string exeName)
        {
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exeName));
            foreach (Process process in processes)
            {
                // Close the application by terminating the process
                process.Kill();
            }
        }
        public void OpenProgram(string programPath)
        {
            try
            {
                Process.Start(programPath);
                
            }
            catch (Exception ex)
            {
                DetectionTimer.Stop();
                // Handle any exceptions that may occur when trying to open the program
                // For example, display an error message
                MessageBox.Show("Error: " + ex.Message + "\n Be sure you have read the readme file and understand where this .exe should be placed!");
                this.Close();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                icon.Visible = true;
                icon.ShowBalloonTip(1000);
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }
        }

        private void icon_doubleclick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            icon.Visible = false;
            WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
    }

    public static class ProcessUtils
    {
        public static string GetActiveProcessName()
        {
            IntPtr foregroundWindowHandle = GetForegroundWindow();
            uint processId;
            GetWindowThreadProcessId(foregroundWindowHandle, out processId);
            Process process = Process.GetProcessById((int)processId);
            return process.ProcessName;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }

    public class MacroSwitcher
    {
        public string xmlFile;
        public string processName;
    }
}
