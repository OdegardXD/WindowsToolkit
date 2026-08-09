using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace WindowsToolkit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // todo:
        // fix delete if statement
        // fix exit if error occured because i dont think it actually exits now. im unsure
        // fix help button not doing anything
        // add the path for the temp file directories 


        private static class GlobalVariables
        {
            public static string WindowsUsername = Environment.UserName;
            public static bool isAdmin; // variable for if program is ran as admin. used in form load
            public static bool isRunning; // check to see if run method is already running. used to prevent user from running method again whilst its already running.
            public static string PrefetchPath = "C:\\Windows\\Prefetch";
            public static string TempPath = "C:\\Windows\\Temp";
            public static string ProcentTempPath = $"C:\\Users\\{WindowsUsername}\\AppData\\Local\\Temp";
        }

        //
        // On Load
        //

        private void Form1_Load(object sender, EventArgs e)
        {
            // Check if window has admin privileges
            GlobalVariables.isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            // WindowsIdentity.GetCurrent() gets an object representing the current user
            // new WindowsPrincipal() ??
            // .IsInRole(WindowsBuiltInRole.Administrator) checks if the user that WindowsIdentity.GetCurrent() returned is a administrator
            // this lets us find out if the user that ran the program has admin privileges

            // now for the if statements to either relaunch as admin or just close.
            if (!GlobalVariables.isAdmin)
            {
                // show message saying user needs to restart program as admin
                MessageBox.Show("This program requires admin privileges! \nPlease relaunch with admin privileges. \nClick 'OK' to close program.", "Requires Admin Privileges");
                // exit program after user clicks ok on messagebox
                Application.Exit();
            }

            else
            {
                LogBox.Text += "Program started.\n";
            }
            LogBox.Text += $"Got Windows Username: {GlobalVariables.WindowsUsername}\n";
        }

        //
        // Run Commands
        //

        private async void RunButton_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.isRunning) 
            {
                MessageBox.Show("Toolkit is already doing its magic! Wait u dingus!", "Error!");
            }
            else 
            { 
                GlobalVariables.isRunning = true;
                if (DeleteTempFilesCheckBox.Checked) // deleting files wont the be same as running programs.
                {
                    LogBox.Text += "Started deleting temp files...\n";
                }
                if (SFCCheckBox.Checked)
                {
                    // log about starting command
                    LogBox.Text += "Starting 'sfc /scannow'\n";
                    try { 
                        // declare the variable and build the object
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "sfc.exe",
                            Arguments = "/scannow",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = true
                        };
        
                        Process sfcProcess = new Process();
                        sfcProcess.StartInfo = startInfo;
        
                        sfcProcess.OutputDataReceived += (sender1, e1) =>
                        {
                            if (e1.Data != null)
                            {
                                LogBox.Invoke(() => LogBox.Text += e1.Data + "\n");
                            }
                        };
        
                        sfcProcess.Start();
                        sfcProcess.BeginOutputReadLine();
                        await sfcProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => LogBox.Text += "SFC failed to run: " + ex.Message + "\n");
                        return;
                    }
                }
                if (DISMCheckBox.Checked)
                {
                    // log about starting command
                    LogBox.Text += "Starting 'DISM'\n";
                    try { 
                        // declare the variable and build the object
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "dism.exe",
                            Arguments = "/Online /Cleanup-Image /RestoreHealth",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = true
                        };
    
                        Process DISMProcess = new Process();
                        DISMProcess.StartInfo = startInfo;
    
                        DISMProcess.OutputDataReceived += (sender1, e1) =>
                        {
                            if (e1.Data != null)
                            {
                                LogBox.Invoke(() => LogBox.Text += e1.Data + "\n");
                            }
                        };
    
                        DISMProcess.Start();
                        DISMProcess.BeginOutputReadLine();
                        await DISMProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => LogBox.Text += "DISM failed to run: " + ex.Message + "\n");
                        return;
                    }
                }
                if (CheckDiskCheckBox.Checked) // chkdsk requires interaction. has a "writeline" line that sends "Y" to confirm to do chkdsk at restart
                {
                    // log about starting command
                    LogBox.Text += "Starting 'CHKDSK'\n";
                    try { 
                        // declare the variable and build the object
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "chkdsk.exe",
                            Arguments = "C: /f",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            RedirectStandardInput = true // this is also new. needed to send input
                        };
    
                        Process chkdskProcess = new Process();
                        chkdskProcess.StartInfo = startInfo;
        
                        chkdskProcess.OutputDataReceived += (sender1, e1) =>
                        {
                            if (e1.Data != null)
                            {
                                LogBox.Invoke(() => LogBox.Text += e1.Data + "\n");
                            }
                        };
    
                        chkdskProcess.Start();
                        chkdskProcess.BeginOutputReadLine();
    
                        chkdskProcess.StandardInput.WriteLine("Y");
                        await chkdskProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => LogBox.Text += "CHKDSK failed to run: " + ex.Message + "\n");
                        return;
                    }
                }
                GlobalVariables.isRunning = false;
                LogBox.Text += "Finished!";
            }
        }
    }
}
