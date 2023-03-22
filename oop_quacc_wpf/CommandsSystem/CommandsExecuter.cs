using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
	public interface CommandsExecuter
	{
		public CommandExecutionState TryExecute(string c);
	}
}
