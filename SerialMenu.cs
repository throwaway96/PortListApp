/* Part of PortListApp. Licensed under GNU AGPL version 3 or later. */

using System.IO.Ports;

namespace PortListApp
{
    internal class SerialMenu : ApplicationContext
    {
        internal const string programName = "Port Monitor";
        internal const string programVersion = "0.1";

        internal uint baudRate = 115200;
        internal string TermTitle => $"Serial %%h ({baudRate}bps)";

        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _contextMenuStrip;
        private readonly ToolStripDropDown _portsDropDown;

        private SerialMenu()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.TrayIcon,
                Text = programName,

                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = programName,
            };

            var toolStripLabel1 = new ToolStripLabel($"{programName} v{programVersion}");

            var portsMenuItem = new ToolStripMenuItem("Ports");

            portsMenuItem.DropDownItemClicked += PortClicked;

            _portsDropDown = new ToolStripDropDown();

            portsMenuItem.DropDown = _portsDropDown;

            var quitButton = new ToolStripButton("Quit", null, QuitClicked);

            _contextMenuStrip = new ContextMenuStrip()
            {
                Items = {
                    toolStripLabel1,
                    new ToolStripSeparator(),
                    portsMenuItem,
                    new ToolStripSeparator(),
                    quitButton,
                }
            };

            _contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(MenuOpened);

            _notifyIcon.ContextMenuStrip = _contextMenuStrip;
            _notifyIcon.Visible = true;
        }

        private void PortClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            _ = sender;

            if (e is null)
            {
                Util.DebugLog("e is null");
                return;
            }

            if (e is not ToolStripItemClickedEventArgs args)
            {
                Util.DebugLog($"e is the wrong type: {e.GetType()}");
                return;
            }

            if (args.ClickedItem?.Tag is not string port)
            {
                Util.DebugLog("port info not found");
                return;
            }

            Util.DebugLog($"port: {port}");

            _notifyIcon.BalloonTipText = $"Launching on {port}...";
            _notifyIcon.ShowBalloonTip(3000);

            TermLauncher.Start(port, baudRate, TermTitle);
        }

        private void QuitClicked(object? sender, EventArgs e)
        {
            _ = sender; _ = e;

            Application.Exit();
        }

        private void MenuOpened(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _ = sender; _ = e;

            _portsDropDown.SuspendLayout();
            _portsDropDown.Items.Clear();

            string[] ports = SerialPort.GetPortNames();

            /* add menu item for each port */
            Array.ForEach(ports, (string p) => { _portsDropDown.Items.Add(new ToolStripMenuItem(p) { Tag = p }); });

            _portsDropDown.ResumeLayout();
        }

        [STAThread]
        static void Main()
        {
            Util.DebugLog("init");
            ApplicationConfiguration.Initialize();
            Application.Run(new SerialMenu());
        }
    }
}
