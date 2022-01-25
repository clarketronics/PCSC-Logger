using PCSC;
using PCSC.Iso7816;
using PCSC.Monitoring;
using System;
using System.IO;
using System.Windows.Forms;

namespace PCSC_Logger
{
    public partial class ConfigForm : Form
    {
        ContextFactory contextFactory = new ContextFactory();

        public ConfigForm()
        {
            InitializeComponent();
            ControlBox = false;
            txtBoxFilePath.Text = Properties.Settings.Default.FileLocation;
        }

        private void cmbBoxReaders_Click(object sender, EventArgs e)
        {
            RefreshReaders();
        }

        private void RefreshReaders()
        {
            using (var conText = contextFactory.Establish(SCardScope.System))
            {
                cmbBoxReaders.Items.Clear();
                var readerNames = conText.GetReaders();
                foreach (var readerName in readerNames)
                {
                    cmbBoxReaders.Items.Add(readerName);
                }
            }
        }

        private void cmbBoxReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbBoxReaders.Text))
            {
                Properties.Settings.Default.Reader = cmbBoxReaders.Text;
            }
        }

        private void btnSetFilePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            var DialogResult = dialog.ShowDialog();
            if (DialogResult == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                Properties.Settings.Default.FileLocation = dialog.SelectedPath;
                txtBoxFilePath.Text = dialog.SelectedPath;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Close();
        }
    }

    public partial class AppContext : ApplicationContext
    {
        NotifyIcon notifyIcon = new NotifyIcon();
        ConfigForm configWindow = new ConfigForm();
        string path;
        bool failed;
        uint reads;

        MenuItem startLoggingMenuItem = new MenuItem();
        MenuItem stopLoggingMenuItem = new MenuItem();
        MenuItem exitMenuItem = new MenuItem();


        MonitorFactory monitorFactory = new MonitorFactory(new ContextFactory());
        ISCardMonitor monitor;

        public AppContext()
        {
            Properties.Settings.Default.Reader = "";

            MenuItem configMenuItem = new MenuItem("Config", new EventHandler(ShowConfig));

            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += new EventHandler(Exit);

            startLoggingMenuItem.Text = "Start Logging";
            startLoggingMenuItem.Click += new EventHandler(StartLogging);

            stopLoggingMenuItem.Text = "Stop Logging";
            stopLoggingMenuItem.Click += new EventHandler(StopLogging);

            notifyIcon.Icon = Properties.Resources.Icon;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
                { configMenuItem,startLoggingMenuItem, exitMenuItem
                });
            notifyIcon.Visible = true;

            monitor = monitorFactory.Create(SCardScope.System);
            monitor.CardInserted += Monitor_CardInserted;
        }

        private void Monitor_CardInserted(object sender, CardStatusEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("dd-MMM-yy HH:mm:ss");
            string uid = GetUID();

            if (uid == "The specified reader is not currently available for use.")
                failed = true;

            if (!failed)
            {
                using (StreamWriter streamWriter = File.AppendText(path))
                {
                    streamWriter.WriteLine(timestamp + "," + uid);
                }
            } 
            else
            {
                using (StreamWriter streamWriter = File.AppendText(path))
                {
                    streamWriter.WriteLine(timestamp + "," + "Read failed at " + reads + " reads.");
                }
                StopLogging(sender, e);
                MessageBox.Show("Read failed at " + DateTime.Now + " after " + reads + " reads.", "Read error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ShowConfig(object sender, EventArgs e)
        {
            // If we are already showing the window, merely focus it.
            if (configWindow.Visible)
            {
                configWindow.Activate();
            }
            else
            {
                configWindow.ShowDialog();
            }

        }

        void StartLogging(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.Reader))
                    throw new ArgumentException("Reader is not set!");

                if (string.IsNullOrEmpty(Properties.Settings.Default.FileLocation))
                    throw new ArgumentException("Log file location is not set!");

                Properties.Settings.Default.Save();

                path = Properties.Settings.Default.FileLocation + "\\PCSC-Log_" + DateTime.Now.ToString("dd-MMM-yy_HH-mm") + ".csv";

                FileStream fileStream = File.Create(path);

                fileStream.Close();

                monitor.Start(Properties.Settings.Default.Reader);

                reads = 0;

                notifyIcon.ContextMenu.MenuItems.Remove(startLoggingMenuItem);
                notifyIcon.ContextMenu.MenuItems.Remove(exitMenuItem);
                notifyIcon.ContextMenu.MenuItems.Add(stopLoggingMenuItem);
                notifyIcon.ContextMenu.MenuItems.Add(exitMenuItem);

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Argument error", MessageBoxButtons.OK ,MessageBoxIcon.Error);
            }
        }

        void StopLogging(object sender, EventArgs e)
        {
            monitor.Cancel();
            notifyIcon.ContextMenu.MenuItems.Remove(stopLoggingMenuItem);
            notifyIcon.ContextMenu.MenuItems.Remove(exitMenuItem);
            notifyIcon.ContextMenu.MenuItems.Add(startLoggingMenuItem);
            notifyIcon.ContextMenu.MenuItems.Add(exitMenuItem);
        }

        string GetUID()
        {
            ContextFactory contextFactory = new ContextFactory();

            try
            {
                using (var ctx = contextFactory.Establish(SCardScope.System))
                {
                    using (var isoReader = new IsoReader(ctx, Properties.Settings.Default.Reader, SCardShareMode.Shared, SCardProtocol.Any, false))
                    {
                        var getUIDAPDU = new CommandApdu(IsoCase.Case2Short, isoReader.ActiveProtocol)
                        {
                            CLA = 0xFF,
                            Instruction = InstructionCode.GetData,
                            P1 = 0x00,
                            P2 = 0x00,
                            Le = 0x00
                        };

                        reads++;

                        return BitConverter.ToString(isoReader.Transmit(getUIDAPDU).GetData()).Replace("-", ":");
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        void Exit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
