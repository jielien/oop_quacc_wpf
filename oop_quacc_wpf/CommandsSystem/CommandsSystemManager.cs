﻿using oop_quacc_wpf.CommandsSystem.CommandsExecuters;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
    /// <summary>
    /// Handles all the <see cref="CommandsExecuter"/>s and input commands.
    /// </summary>
    public class CommandsSystemManager
    {
        public CommandsSystemManager(List<CommandsExecuter> executers)
        {
            Executers = executers;
        }

        /// <summary>
        /// All usable <see cref="CommandsExecuter"/>s.
        /// </summary>
        public List<CommandsExecuter> Executers { get; private set; }

        /// <summary>
        /// Tries to find and execute command. If command was not found in <paramref name="executer"/>, tries to find it and execute in other <see cref="Executers"/>.
        /// </summary>
        public CommandExecutionState ExecuteCommand(string command, string executerName)
        {
            var splitted = command.Split(' ');
            var comm = splitted[0];
            var args = splitted.Skip(1).ToArray();

            var validExecuters = Executers.Where(e => e.Name == executerName);
            if (validExecuters.Count() == 1) // if executer was found
            {
                var executer = validExecuters.ElementAt(0);
                if (executer.CommandIsValid(command)) // if command is valid
                    return executer.Execute(comm, args);
                else
                {
                    foreach (var e in Executers)
                        if (!e.GetType().Equals(executer)) // if executert is not same as e
                            if (e.CommandIsValid(command)) // if command is valid
                                return e.Execute(comm, args);
                    return CommandExecutionState.CommandNotFound;
                }
            }
            else return CommandExecutionState.InvalidExecuter;
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
    }
}