using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
	/// <summary>
	/// Defines the result of command execution function.
	/// </summary>
	public enum CommandExecutionState
	{
		CommandNotFound,
		Executed,
		CouldNotExecute,
		InvalidExecuter,

		Exit
	}
}
