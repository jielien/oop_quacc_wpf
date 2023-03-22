using oop_quacc_wpf.CommandsSystem.CommandsExecuters;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
	public class CommandsSystemManager
	{
		public CommandsSystemManager(List<CommandsExecuter> executers)
		{
			Executers = executers;

			foreach (var e in Executers)
			{
				if (e is QUACCComandsExecuter)
					SelectedExecuter = e;
			}
		}

		public List<CommandsExecuter> Executers { get; private set; }
		public CommandsExecuter SelectedExecuter { get; private set; }

		/// <summary>
		/// Tries to find and execute command. If no <see cref="CommandsExecuter"/> is given, executes on <see cref="SelectedExecuter"/>. If command was not found in <see cref="SelectedExecuter"/>, tries to find it and execute in other <see cref="Executers"/>.
		/// </summary>
		public CommandExecutionState ExecuteCommand(string c, CommandsExecuter? e = null)
		{
			var state = CommandExecutionState.CouldNotExecute; // init command execution state

			if (e == null)
			{
				state = SelectedExecuter.TryExecute(c); // try to execute with SelectedExecutor primarily
				if (state == CommandExecutionState.CommandNotFound)
					foreach (var executer in Executers) // if command was not found, try to execute with each Executer
					{
						if (executer.GetType() != SelectedExecuter.GetType()) // don't execute with SelectedExecuter again
							state = ExecuteCommand(c, executer);
						if (state == CommandExecutionState.Executed) break; // if command successfully executed, break foreach
					}
			}
			else
				state = e.TryExecute(c);

			return state;
		}

		/// <summary>
		/// Tries to add <see cref="CommandsExecuter"/> <paramref name="e"/>, unless there already is a <see cref="CommandsExecuter"/> of the same type.
		/// </summary>
		/// <returns>True if <paramref name="e"/> was successfully added, False instead.</returns>
		public bool AddExecuter(CommandsExecuter e)
		{
			bool added = true;
			foreach (var executer in Executers)
			{
				if (executer.GetType() == e.GetType())
				{
					added = false;
					break;
				}
			}
			if (added) Executers.Add(e);

			return added;
		}
	}
}
