/* Part of PortListApp. Licensed under GNU AGPL version 3 or later. */

using System.ComponentModel;
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
        private readonly ToolStripLabel _noPortsLabel = new ToolStripLabel("(no ports)") {
            Enabled = false
        };

        private SerialMenu()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.TrayIcon,
                Text = programName,

                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle = programName,
            };

            var programInfoLabel = new ToolStripLabel($"{programName} v{programVersion}");

            var portsMenuItem = new ToolStripMenuItem("Ports");

            portsMenuItem.DropDownItemClicked += PortClicked;

            _portsDropDown = new ToolStripDropDown();

            portsMenuItem.DropDown = _portsDropDown;

            var quitButton = new ToolStripButton("Quit", null, QuitClicked);

            _contextMenuStrip = new ContextMenuStrip()
            {
                Items = {
                    programInfoLabel,
                    new ToolStripSeparator(),
                    portsMenuItem,
                    new ToolStripSeparator(),
                    quitButton,
                }
            };

            _contextMenuStrip.Opening += new CancelEventHandler(MenuOpened);

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

            /* timeout is not actually used anymore; system settings used instead */
            _notifyIcon.ShowBalloonTip(0, programName, $"Launching on {port}...", ToolTipIcon.Info);

            TermLauncher.Start(port, baudRate, TermTitle);
        }

        private void QuitClicked(object? sender, EventArgs e)
        {
            _ = sender; _ = e;

            Application.Exit();
        }

        private static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        private void MenuOpened(object? sender, CancelEventArgs e)
        {
            _ = sender; _ = e;

            _portsDropDown.SuspendLayout();
            _portsDropDown.Items.Clear();

            string[] ports = GetPorts();

            if (ports.Length == 0)
            {
                _portsDropDown.Items.Add(_noPortsLabel);
            }
            else
            {
                /* add menu item for each port */
                Array.ForEach(ports, (string p) => { _portsDropDown.Items.Add(new ToolStripMenuItem(p) { Tag = p }); });
            }

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
