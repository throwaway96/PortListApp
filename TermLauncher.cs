/* Part of PortListApp. Licensed under GNU AGPL version 3 or later. */

using System.Diagnostics;

namespace PortListApp
{
    internal static class TermLauncher
    {
        internal const string PuttyPath = @"C:\Program Files\PuTTY\putty.exe";
        internal static readonly string KittyPath = Util.GetUserDir() + @"\.local\bin\kitty.exe";

        internal static readonly string LogDir = Util.GetUserDir() + @"\serial-logs";
        internal const string LogPrefix = "serial-";

        internal const bool UseKitty = true;

        public static void Start(string port, uint baudRate, string termTitle = "")
        {
            var args = new List<string>
            {
                "-serial",
                port,
                "-sercfg", $"{baudRate},8,n,1,N",
                "-sessionlog", $@"{LogDir}\{LogPrefix}&Y&M&DT&T-&H.log",
            };

            if (UseKitty && termTitle != "")
            {
                args.AddRange(["-title", termTitle]);
            }

            var startInfo = new ProcessStartInfo(UseKitty ? KittyPath : PuttyPath, args);

            string argStr = string.Join(' ', args);
            Util.DebugLog($"args: {argStr}");

            Process.Start(startInfo);
        }
    }
}
