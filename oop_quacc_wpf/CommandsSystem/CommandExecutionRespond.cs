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
	public enum CommandExecutionRespond
	{
		CommandNotFound,
		Executed,
		CouldNotExecute,
		InvalidExecuter,
		WrongNumberOfArguments,

		Exit
	}
}
