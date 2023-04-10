using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace oop_quacc_wpf.CommandsSystem.CommandsExecuters
{
    /// <summary>
    /// One of the default executers. Can handle various math expressions and give answers to them.
    /// </summary>
    public class QUACCMathCommandsExecuter : CommandsExecuter
    {
        public override string Name { get; protected set; }
        public QUACCMathCommandsExecuter()
        {
            Name = "QUACC Math";

            Commands = new Dictionary<string, Func<string[], CommandExecutionRespond>>(){
                { "hw", HelloWorld }
            };
        }

        public override bool CommandIsValid(string c)
        {
            return false;
        }

        #region COMMAND FUNCTIONS

        private CommandExecutionRespond HelloWorld(string[] args)
        {
            MessageBox.Show("Hello World!\n" + args);
            return CommandExecutionRespond.Executed;
        }

        #endregion
    }
}
