using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Linq.Expressions;

namespace WindowsToolkit
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private static class GlobalVariables
        {
            public static string WindowsUsername = Environment.UserName;
            public static bool isAdmin; // variable for if program is ran as admin. used in form load
            public static bool isRunning; // check to see if run method is already running. used to prevent user from running method again whilst its already running.
            public static string PrefetchPath = "C:\\Windows\\Prefetch";
            public static string TempPath = "C:\\Windows\\Temp";
            public static string PercentTempPath = $"C:\\Users\\{WindowsUsername}\\AppData\\Local\\Temp";
            public static Version? localVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
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

            // future ode here: what the fuck is this slop

            // now for the if statements to either relaunch as admin or just close.
            if (!GlobalVariables.isAdmin)
            {
                // show message saying user needs to restart program as admin
                MessageBox.Show("This program requires admin privileges! \nPlease relaunch with admin privileges. \nClick 'OK' to close program.", "Requires Admin Privileges");
                // exit program after user clicks ok on messagebox
                Application.Exit();
                return;
            }

            else
            {
                AppendLog("Program started.\n");
            }
            AppendLog($"Got Windows Username: {GlobalVariables.WindowsUsername}\n");
            VersionChecker();
        }

        //
        // FormClosing event - ask for confirmation if IsRunning is true when X has been clicked
        //

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GlobalVariables.isRunning)
            {
                var result = MessageBox.Show(
                    "The toolkit is still running something.\nClosing now will keep the current task running but will stop any future tasks.\nClose anyway?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        //
        // VersionChecker
        //

        private async void VersionChecker()
        {
            AppendLog($"Current Version: v{GlobalVariables.localVersion}\n");
            var latest = await CheckForUpdate.GetLatestReleaseVersionAsync();
            if (latest != null && latest > GlobalVariables.localVersion)
            {
                AppendLog($"Update available: v{latest} (you're on v{GlobalVariables.localVersion})\n");
            }
        }

        //
        // Append Log
        //

        private void AppendLog(string text)
        {
            LogBox.AppendText(text);
            LogBox.SelectionStart = LogBox.Text.Length;
            LogBox.ScrollToCaret();
        }

        //
        // CHKDSK Restart Warn
        //

        private void CHKDSK_Restart_warn(object sender, EventArgs e)
        {
            if (CheckDiskCheckBox.Checked)
            {
                DialogResult result = MessageBox.Show("Warning! CHKDSK requires a full PC restart. Press no to unselect, yes to proceed.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    CheckDiskCheckBox.Checked = false;
                }
            }
        }

        //
        // Run Commands
        //

        private async void RunButton_Click(object sender, EventArgs e)
        {
            if (!DeleteTempFilesCheckBox.Checked && !ClearRecycleBinCheckBox.Checked && !DISMCheckBox.Checked && !SFCCheckBox.Checked && !DISMCleanupCheckBox.Checked && !CheckDiskCheckBox.Checked && !FlushDNSCheckBox.Checked)
            {
                MessageBox.Show("Please select a option before trying to run...", "Error!");
                return;
            }
            if (GlobalVariables.isRunning)
            {
                MessageBox.Show("Toolkit is already doing its magic! Wait u dingus!", "Error!");
            }
            else
            {
                GlobalVariables.isRunning = true;
                try
                {

                // -- delete temp files --

                if (DeleteTempFilesCheckBox.Checked) // runs on a background thread since folders like Temp can have thousands of files - doing it on the UI thread freezes the window
                {
                    var enumOptions = new EnumerationOptions
                    {
                        RecurseSubdirectories = true,
                        IgnoreInaccessible = true
                    };

                    AppendLog("Started deleting temp files...\n");
                    await Task.Run(() =>
                    {
                        LogBox.Invoke(() => AppendLog("1/3 - Temp\n"));
                        foreach (var file in Directory.EnumerateFiles(GlobalVariables.TempPath, "*", enumOptions))
                        {
                            try
                            {
                                File.Delete(file);
                                LogBox.Invoke(() => AppendLog($"Deleted: {file}\n"));
                            }
                            catch (Exception ex)
                            {
                                LogBox.Invoke(() => AppendLog($"Failed: {file} - {ex.Message}\n"));
                            }
                        }
                        LogBox.Invoke(() => AppendLog("2/3 - %Temp%\n"));
                        foreach (var file in Directory.EnumerateFiles(GlobalVariables.PercentTempPath, "*", enumOptions))
                        {
                            try
                            {
                                File.Delete(file);
                                LogBox.Invoke(() => AppendLog($"Deleted: {file}\n"));
                            }
                            catch (Exception ex)
                            {
                                LogBox.Invoke(() => AppendLog($"Failed: {file} - {ex.Message}\n"));
                            }
                        }
                        LogBox.Invoke(() => AppendLog("3/3 - Prefetch\n"));
                        foreach (var file in Directory.EnumerateFiles(GlobalVariables.PrefetchPath, "*", enumOptions))
                        {
                            try
                            {
                                File.Delete(file);
                                LogBox.Invoke(() => AppendLog($"Deleted: {file}\n"));
                            }
                            catch (Exception ex)
                            {
                                LogBox.Invoke(() => AppendLog($"Failed: {file} - {ex.Message}\n"));
                            }
                        }
                    });
                    AppendLog("Done Deleting Temp Files.\n");
                }

                // -- clear recycle bin --

                if (ClearRecycleBinCheckBox.Checked) 
                {
                    AppendLog("Clearing Recycle Bin...\n");
                    [DllImport("Shell32.dll")]
                    static extern int SHEmptyRecycleBin(IntPtr hwnd, string? pszRootPath, uint dwFlags);

                    SHEmptyRecycleBin(IntPtr.Zero, null, 7);
                    AppendLog("Finished Clearing Recycle Bin!\n");
                }

                // -- dism restore health --

                if (DISMCheckBox.Checked)
                {
                    // log about starting command
                    AppendLog("Starting 'DISM Restore Health'\n");
                    try
                    {
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
                                LogBox.Invoke(() => AppendLog(e1.Data + "\n"));
                            }
                        };

                        DISMProcess.Start();
                        DISMProcess.BeginOutputReadLine();
                        await DISMProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => AppendLog("DISM failed to run: " + ex.Message + "\n"));
                        return;
                    }
                }

                // -- sfc --

                if (SFCCheckBox.Checked)
                {
                    // log about starting command
                    AppendLog("Starting 'sfc /scannow'\n");
                    try
                    {
                        // declare the variable and build the object
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "sfc.exe",
                            Arguments = "/scannow",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            StandardOutputEncoding = System.Text.Encoding.Unicode
                        };

                        Process sfcProcess = new Process();
                        sfcProcess.StartInfo = startInfo;

                        sfcProcess.OutputDataReceived += (sender1, e1) =>
                        {
                            if (e1.Data != null)
                            {
                                LogBox.Invoke(() => AppendLog(e1.Data + "\n"));
                            }
                        };

                        sfcProcess.Start();
                        sfcProcess.BeginOutputReadLine();
                        await sfcProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => AppendLog("SFC failed to run: " + ex.Message + "\n"));
                        return;
                    }
                }

                // -- dism cleanup --

                if (DISMCleanupCheckBox.Checked)
                {
                    // log about starting command
                    AppendLog("Starting 'DISM Cleanup'\n");
                    try
                    {
                        // declare the variable and build the object
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "dism.exe",
                            Arguments = "/Online /Cleanup-Image /StartComponentCleanup",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = true
                        };

                        Process DISMCleanupProcess = new Process();
                        DISMCleanupProcess.StartInfo = startInfo;

                        DISMCleanupProcess.OutputDataReceived += (sender1, e1) =>
                        {
                            if (e1.Data != null)
                            {
                                LogBox.Invoke(() => AppendLog(e1.Data + "\n"));
                            }
                        };

                        DISMCleanupProcess.Start();
                        DISMCleanupProcess.BeginOutputReadLine();
                        await DISMCleanupProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => AppendLog("DISM failed to run: " + ex.Message + "\n"));
                        return;
                    }
                }

                // -- chkdsk --

                if (CheckDiskCheckBox.Checked) // chkdsk requires interaction. has a "writeline" line that sends "Y" to confirm to do chkdsk at restart
                {
                    // log about starting command
                    AppendLog("Starting 'CHKDSK'\n");
                    try
                    {
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
                                LogBox.Invoke(() => AppendLog(e1.Data + "\n"));
                            }
                        };

                        chkdskProcess.Start();
                        chkdskProcess.BeginOutputReadLine();

                        chkdskProcess.StandardInput.WriteLine("Y");
                        await chkdskProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => AppendLog("CHKDSK failed to run: " + ex.Message + "\n"));
                        return;
                    }
                }

                // -- flush dns --

                if (FlushDNSCheckBox.Checked)
                {
                    // log about starting command
                    AppendLog("Starting Flush DNS\n");
                    try
                    {
                        // declare the variable and build the object
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "ipconfig.exe",
                            Arguments = "/flushdns",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardOutput = true
                        };

                        Process FlushDNSProcess = new Process();
                        FlushDNSProcess.StartInfo = startInfo;

                        FlushDNSProcess.OutputDataReceived += (sender1, e1) =>
                        {
                            if (e1.Data != null)
                            {
                                LogBox.Invoke(() => AppendLog(e1.Data + "\n"));
                            }
                        };

                        FlushDNSProcess.Start();
                        FlushDNSProcess.BeginOutputReadLine();
                        await FlushDNSProcess.WaitForExitAsync();
                    }
                    catch (Exception ex)
                    {
                        LogBox.Invoke(() => AppendLog("Flush DNS failed to run: " + ex.Message + "\n"));
                        return;
                    }
                }

                AppendLog("Finished!\n");
                }
                finally
                {
                    GlobalVariables.isRunning = false;
                }
            }
        }

        //
        // Help Button
        //

        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Windows Toolkit, Made by OdegardXD\n\n" +
                "Delete Temp Files\nDeletes some unnecessary leftover files. Specifically the folders are 'Temp', '%Temp%' and 'Prefetch' Programs use these folders to dump temporary files and its not always that they delete them so the folders can take up space over time.\n\n" +
                "Clear Recycle Bin\nEmpties your recycle bin.\n\n" +
                "DISM Restore Health\nDISM repairs the underlying Windows System image itself.\n\n" +
                "sfc /scannow\nThis is a built in Windows tool that scans all Windows system files and compares them against a known good copy to check if any are broken/corrupted and then replaces them if they are.\n\n" +
                "DISM Component Cleanup\nRemoves old, superseded versions of system components that pile up in the component store after Windows updates, freeing up space without touching anything currently in use.\n\n" +
                "CHKDSK\nCHKDSK scans your storage drive for issues. Issues like damage to the drive itself and the file system structure\n\n" +
                "Flush DNS\nClears your DNS cache. Can help if internet things arent connecting.", "Help - Windows Toolkit");
        }

        //
        // Clear Log Box
        //

        private void ClearLogBox_Click(object sender, EventArgs e)
        {
            LogBox.Text = string.Empty;
        }
    }
}
