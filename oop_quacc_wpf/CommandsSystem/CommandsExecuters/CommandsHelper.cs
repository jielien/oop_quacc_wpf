using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem.CommandsExecuters
{
    /// <summary>
    /// Static class providing helper functions to executers.
    /// </summary>
    public static class CommandsHelper
    {
        /// <summary>
        /// Opens directory or file with default app.
        /// </summary>
        public static void OpenWithDefaultApp(string path)
        {
            Process.Start("explorer", path);
        }

        /// <summary>
        /// Creates valid URL from <paramref name="url"/> to be passed to <see cref="Process.Start(string)"/>.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string? CreateValidURLFrom(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var regexProtocol = new Regex("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");
                var regexNoProtocol = new Regex("^[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");

                if(regexNoProtocol.IsMatch(url)) url = "http://" + url;
                if (regexProtocol.IsMatch(url)) return url;
            }
            return null;
        }
    }
}
