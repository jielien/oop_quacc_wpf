using oop_quacc_wpf.CommandsSystem.CommandsExecuters;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
    /// <summary>
    /// Handles all the <see cref="CommandsExecuter"/>s and input commands.
    /// </summary>
    public class CommandsSystemManager
    {
        private const uint HISTORY_SIZE = 16;

        public CommandsSystemManager(List<CommandsExecuter> executers)
        {
            Executers = executers;
            CommandsHistory = new CommandsHistory(HISTORY_SIZE);
        }

        /// <summary>
        /// All usable <see cref="CommandsExecuter"/>s.
        /// </summary>
        public List<CommandsExecuter> Executers { get; private set; }

        /// <summary>
        /// Provides a history of recent commands.
        /// </summary>
        private CommandsHistory CommandsHistory { get; set; }

        /// <summary>
        /// Tries to find and execute command. If command was not found in <paramref name="executer"/>, tries to find it and execute in other <see cref="Executers"/>.
        /// </summary>
        public CommandExecutionRespond ExecuteCommand(string command, string executerName)
        {
            CommandsHistory.Add(command);

            // Extract command and arguments
            var splitted = command.Split(' ');
            var comm = splitted[0];
            var args = splitted.Skip(1).ToArray();

            // Find valid executer and try to execute
            var validExecuters = Executers.Where(e => e.Name == executerName);
            if (validExecuters.Count() == 1) // if executer was found
            {
                var executer = validExecuters.ElementAt(0);
                if (executer.CommandIsValid(command)) // if command is valid
                    return executer.Execute(comm, args);
                else
                {
                    foreach (var e in Executers)
                        if (!e.GetType().Equals(executer)) // if executer is not same as e
                            if (e.CommandIsValid(command)) // if command is valid
                                return e.Execute(comm, args);
                    return CommandExecutionRespond.CommandNotFound;
                }
            }
            else return CommandExecutionRespond.InvalidExecuter;
        }

        /// <summary>
        /// Tries to add <see cref="CommandsExecuter"/> <paramref name="e"/>, unless there already is a <see cref="CommandsExecuter"/> of the same type.
        /// </summary>
        /// <returns>True if <paramref name="e"/> was successfully canAdd, False instead.</returns>
        public bool AddExecuter(CommandsExecuter e)
        {
            bool canAdd = true;
            foreach (var executer in Executers)
            {
                // check if there already is Executer of the same type
                if (executer.GetType() == e.GetType())
                {
                    canAdd = false;
                    break;
                }
            }
            if (canAdd) Executers.Add(e);

            return canAdd;
        }

        public string? NextCommand() =>
            CommandsHistory.Next();
        public string? PreviousCommand() =>
            CommandsHistory.Previous();
    }
}