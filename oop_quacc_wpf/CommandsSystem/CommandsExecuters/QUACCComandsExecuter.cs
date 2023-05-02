using Microsoft.VisualBasic;
using oop_quacc_wpf.CommandsSystem.ResponseSystem;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace oop_quacc_wpf.CommandsSystem.CommandsExecuters
{
    /// <summary>
    /// One of the default executers. When not defined otherwise, this executer will always be default selected executer.
    /// </summary>
    public class QUACCComandsExecuter : CommandsExecuter
    {
        public override string Name { get; protected set; }
        public override string ConfigName { get; protected set; }
        public Dictionary<string, string> Paths { get; private set; }

        public QUACCComandsExecuter()
        {
            Name = "QUACC";
            ConfigName = "quacc_config";

            Config();
        }

        /// <summary>
        /// Initializes <see cref="CommandsExecuter.Commands"/> and <see cref="Paths"/> using "quacc_config.json" file.
        /// </summary>
        /// <exception cref="FileFormatException"></exception>
        private void Config()
        {
            Commands = new Dictionary<string, Func<string[], CommandExecutionResponse>>()
            {
                { "hw", HelloWorld },
                { "os", OpenShortcut },
                { "as", AddShortcut },
                { "rs", RemoveShortcut },
                { "s", Search },
                { "h", Hide },
                { "x", Exit },
            };

            Paths = ExecutersHelper.ReadConfig<Dictionary<string, string>>(ConfigName);
        }

        /// <inheritdoc/>
        public override bool CommandIsValid(string c) =>
            Commands.ContainsKey(c.Split(' ')[0]);


        #region COMMAND FUNCTIONS

        /// <summary>
        /// Simple Hello World command function.
        /// </summary>
        private CommandExecutionResponse HelloWorld(string[] args)
        {
            MessageBox.Show("Hello World!\n" + string.Join(' ', args));

            return CommandExecutionResponse.Executed();
        }

        /// <summary>
        /// Checks whether the <paramref name="args"/> is valid path shortcut or URL. Then opens it with default app.
        /// </summary>
        private CommandExecutionResponse OpenShortcut(string[] args)
        {
            if (args.Length != 1) return CommandExecutionResponse.WrongNumberOfArguments(1, args.Length);

            var path = string.Join(' ', args);
            var url = ExecutersHelper.CreateValidURLFrom(path);
            if (Paths.ContainsKey(path))
                ExecutersHelper.OpenWithDefaultApp(Paths[path]);
            else if (url != null)
                ExecutersHelper.OpenWithDefaultApp(url);
            else return CommandExecutionResponse.CouldNotExecute("Source not found!");
            return CommandExecutionResponse.Executed();
        }

        /// <summary>
        /// Adds new path shortcut to <see cref="Paths"/>.
        /// </summary>
        private CommandExecutionResponse AddShortcut(string[] args)
        {
            if(args.Length < 2) return CommandExecutionResponse.WrongNumberOfArguments(2, args.Length);

            string alias = args[0];
            string path = string.Join(" ", args.Skip(1));

            // Try to create a valid URL from path
            var url = ExecutersHelper.CreateValidURLFrom(path);
            if (url != null) path = url;

            // If there is a valid directory, file or URL
            if (Directory.Exists(path) || File.Exists(path) || url != null)
            {
                var regex = new Regex(@"^\w+$"); // Regex for letters, numbers and underscore
                if (regex.IsMatch(alias))
                {
                    Paths.Add(alias, path);
                    ExecutersHelper.SaveConfig(Paths, ConfigName);
                    return CommandExecutionResponse.Executed();
                }
                else return CommandExecutionResponse.CouldNotExecute("Path shortcut can only contain letters, numbers or underscore.");
            }

            return CommandExecutionResponse.CouldNotExecute("Invalid source path.");
        }

        /// <summary>
        /// Removes a path from <see cref="Paths"/>.
        /// </summary>
        private CommandExecutionResponse RemoveShortcut(string[] args)
        {
            if(args.Length != 1) return CommandExecutionResponse.WrongNumberOfArguments(1, args.Length);

            string alias = args.First();

            if (Paths.ContainsKey(alias))
            {
                Paths.Remove(alias);
                ExecutersHelper.SaveConfig(Paths, ConfigName);
                return CommandExecutionResponse.Executed();
            }
            else
                return CommandExecutionResponse.CouldNotExecute("Path shortcut not found.");
        }

        /// <summary>
        /// Hides command window.
        /// </summary>
        private CommandExecutionResponse Hide(string[] args) =>
            CommandExecutionResponse.Hide();

        /// <summary>
        /// Searches for <paramref name="args"/> on Google.
        /// </summary>
        private CommandExecutionResponse Search(string[] args)
        {
            if (args.Length < 1) return CommandExecutionResponse.WrongNumberOfArguments(1, args.Length);

            string encoded = Uri.EscapeDataString(string.Join("+", args));
            return OpenShortcut(("https://google.com/search?q=" + encoded).Split(' '));
        }

        /// <summary>
        /// Saves <see cref="Paths"/> and then exits the application.
        /// </summary>
        private CommandExecutionResponse Exit(string[] args)
        {
            ExecutersHelper.SaveConfig(Paths, ConfigName);

            return CommandExecutionResponse.Exit();
        }

        #endregion

    }
}
