using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
    /// <summary>
    /// Base abstract class for command executers.
    /// </summary>
    public abstract class CommandsExecuter
    {
        /// <summary>
        /// Defines what whould be shown in <see cref="CommandWindow.ExecuterComboBox"/>.
        /// </summary>
        public abstract string Name { get; protected set; }

        /// <summary>
        /// Defines all the commands and their callbacks.
        /// </summary>
        public Dictionary<string, Func<string[], CommandExecutionState>> Commands { get; protected set; }

        /// <summary>
        /// Executes command <paramref name="comm"/> from <see cref="Commands"/> with arguments <paramref name="args"/>.
        /// </summary>
        public CommandExecutionState Execute(string comm, params string[] args) =>
            Commands[comm](args);

        /// <summary>
        /// Takes whole command string and determines wheter it is valid or not.
        /// </summary>
        public abstract bool CommandIsValid(string c);
    }
}
