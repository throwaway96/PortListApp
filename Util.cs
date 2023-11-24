/* Part of PortListApp. Licensed under GNU AGPL version 3 or later. */

using System.Runtime.CompilerServices;

namespace PortListApp
{
    internal static class Util
    {

        internal static void DebugLog(string msg, [CallerMemberName] string callerMember = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
        {
            System.Diagnostics.Debug.WriteLine($"{callerMember} ({callerFile}:{callerLine}): {msg}");
        }

        internal static string GetUserDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
}
