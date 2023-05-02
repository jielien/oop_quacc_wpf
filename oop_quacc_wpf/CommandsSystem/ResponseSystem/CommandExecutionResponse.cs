using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem.ResponseSystem
{
    /// <summary>
    /// Defines the result of command execution function.
    /// </summary>
    public record CommandExecutionResponse(ResponseStatus Status, AppContext Context)
    {
        public static CommandExecutionResponse WrongNumberOfArguments(int needed, int provided) => 
            new CommandExecutionResponse(ResponseStatus.WrongNumberOfArguments, new AppContext(false, false, $"Wrong Number of Arguments\nNeeded: {needed}\nProvided: {provided}"));
        public static CommandExecutionResponse CouldNotExecute(string reason) =>
            new CommandExecutionResponse(ResponseStatus.CouldNotExecute, new AppContext(false, false, "Could Not Execute" + (string.IsNullOrEmpty(reason) ? string.Empty : $"\n{reason}" )));
        public static CommandExecutionResponse Executed(string? message = null) =>
            new CommandExecutionResponse(ResponseStatus.Executed, new AppContext(false, false, "Successfully Executed" + (string.IsNullOrEmpty(message) ? string.Empty : $"\n{message}")));
        public static CommandExecutionResponse CommandNotFound() =>
            new CommandExecutionResponse(ResponseStatus.CommandNotFound, new AppContext(false, false, "Command Not Found"));
        public static CommandExecutionResponse InvalidExecuter() =>
            new CommandExecutionResponse(ResponseStatus.InvalidExecuter, new AppContext(false, false, "Invalid Executer"));
        public static CommandExecutionResponse Exit() =>
            new CommandExecutionResponse(ResponseStatus.Executed, new AppContext(true, false, "Exit"));
        public static CommandExecutionResponse Hide() =>
            new CommandExecutionResponse(ResponseStatus.Executed, new AppContext(false, true, "Hide"));
    }
}
