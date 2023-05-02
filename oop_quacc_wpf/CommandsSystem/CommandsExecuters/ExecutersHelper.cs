using oop_quacc_wpf.CommandsSystem.ResponseSystem;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace oop_quacc_wpf.CommandsSystem.CommandsExecuters
{
    /// <summary>
    /// Static class providing helper functions to executers.
    /// </summary>
    public static class ExecutersHelper
    {
        /// <summary>
        /// Returns deserialized data from .json file in Configs folder.
        /// </summary>
        /// <typeparam name="T">Type of deserialized data.</typeparam>
        /// <param name="configName">Name of config file.</param>
        /// <exception cref="FileFormatException"></exception>
        public static T ReadConfig<T>(string configName)
        {
            using FileStream fs = File.OpenRead(Directory.GetCurrentDirectory() + $"\\Configs\\{configName}.json");
            return JsonSerializer.Deserialize<T>(fs) ?? throw new FileFormatException("Config file could not be deserialized!");
        }

        /// <summary>
        /// Saves data to .json file in Configs folder.
        /// </summary>
        public static void SaveConfig<T>(T data, string configName)
        {
            string deserialized = JsonSerializer.Serialize(data);
            File.WriteAllText(Directory.GetCurrentDirectory() + $"\\Configs\\{configName}.json", deserialized);
        }

        /// <summary>
        /// Opens directory or file with default app.
        /// </summary>
        public static void OpenWithDefaultApp(string path)
        {
            Process.Start("explorer", "\"" + path + "\"");
        }

        /// <summary>
        /// Creates valid URL from <paramref name="url"/> to be passed to <see cref="Process.Start(string)"/>.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>A valid URL if it can be created, otherwise returns <paramref name="url"/>.</returns>
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
