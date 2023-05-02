using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem.ResponseSystem
{
    public enum ResponseStatus
    {
        CommandNotFound,
        Executed,
        CouldNotExecute,
        InvalidExecuter,
        WrongNumberOfArguments
    }
}
