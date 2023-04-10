using Microsoft.VisualBasic;
using System;
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

        public Dictionary<string, string> Paths { get; private set; }

        public override string Name { get; protected set; }
        public QUACCComandsExecuter()
        {
            Name = "QUACC";

            Config();
        }

        /// <summary>
        /// Initializes <see cref="CommandsExecuter.Commands"/> and <see cref="Paths"/> using "quacc_config.json" file.
        /// </summary>
        /// <exception cref="FileFormatException"></exception>
        private void Config()
        {
            Commands = new Dictionary<string, Func<string[], CommandExecutionRespond>>()
            {
                { "hw", HelloWorld },
                { "op", Open },
                { "adds", AddToPath },
                { "exit", Exit },
                { "s", Search }
            };

            FileStream fs = File.OpenRead(Directory.GetCurrentDirectory() + "\\Configs\\quacc_config.json");
            Paths = JsonSerializer.Deserialize<Dictionary<string, string>>(fs) ?? throw new FileFormatException("Config file could not be deserialized!");
        }

        /// <inheritdoc/>
        public override bool CommandIsValid(string c) =>
            Commands.ContainsKey(c.Split(' ')[0]);


        #region COMMAND FUNCTIONS

        /// <summary>
        /// Simple Hello World command function.
        /// </summary>
        private CommandExecutionRespond HelloWorld(string[] args)
        {
            MessageBox.Show("Hello World!\n" + string.Join(' ', args));
            return CommandExecutionRespond.Executed;
        }

        /// <summary>
        /// Checks whether the <paramref name="args"/> is valid path shortcut or URL. Then opens it with default app.
        /// </summary>
        private CommandExecutionRespond Open(string[] args)
        {
            if (args.Length != 1) return CommandExecutionRespond.WrongNumberOfArguments;

            var path = string.Join(' ', args);
            var url = CommandsHelper.CreateValidURLFrom(path);
            if (Paths.ContainsKey(path))
                CommandsHelper.OpenWithDefaultApp(Paths[path]);
            else if (url != null)
                CommandsHelper.OpenWithDefaultApp(url);
            else return CommandExecutionRespond.CouldNotExecute;
            return CommandExecutionRespond.Executed;
        }

        /// <summary>
        /// Adds new path shortcut to <see cref="Paths"/>.
        /// </summary>
        private CommandExecutionRespond AddToPath(string[] args)
        {
            if(args.Length != 2) return CommandExecutionRespond.WrongNumberOfArguments;

            var url = CommandsHelper.CreateValidURLFrom(args[1]);
            if (url != null) args[1] = url;
            if (args.Length == 2 && (Directory.Exists(args[1]) || File.Exists(args[1]) || url != null))
            {
                var regex = new Regex(@"^\w+$");
                if (regex.IsMatch(args[0]))
                {
                    Paths.Add(args[0], args[1]);
                    return CommandExecutionRespond.Executed;
                }
                else MessageBox.Show("Path shortcut can only contain letters, numbers and underscore!");
            }
            //else
            //{
            //    throw new notimplementedexception("directory selection here.");
            //}

            return CommandExecutionRespond.CouldNotExecute;
        }

        /// <summary>
        /// Saves <see cref="Paths"/> and then exits the application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private CommandExecutionRespond Exit(string[] args)
        {
            string deserialized = JsonSerializer.Serialize(Paths);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Configs\\quacc_config.json", deserialized);

            return CommandExecutionRespond.Exit;
        }

        private CommandExecutionRespond Search(string[] args)
        {
            if (args.Length < 1) return CommandExecutionRespond.WrongNumberOfArguments;

            string encoded = Uri.EscapeDataString(string.Join("+", args));
            return Open(("https://google.com/search?q=" + encoded).Split(' '));
        }

        #endregion

    }
}
