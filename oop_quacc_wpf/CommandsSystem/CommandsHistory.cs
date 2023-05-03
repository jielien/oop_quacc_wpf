using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem
{
    /// <summary>
    /// Holds a fixed number of recent commands and can access them.
    /// </summary>
    public class CommandsHistory
    {
        private readonly string[] commands;
        private int head = 0;
        private int tail = 0;
        private int cursor = -1;

        public CommandsHistory(uint size)
        {
            commands = new string[size];
        }

        public int Count
        {
            get
            {
                if (tail <= head)
                    return head - tail;
                else
                    return commands.Length - (tail - head);
            }
        }

        public void Add(string command)
        {
            cursor = (cursor + 1) % commands.Length;
            commands[cursor] = command;
            head = (cursor + 1) % commands.Length;
            if (head == tail)
                tail = (tail + 1) % commands.Length;
        }

        public string? Next()
        {
            if (Count == 0) return null;

            cursor = (cursor + 1) % commands.Length;
            if (cursor == head)
            {
                cursor = (cursor - 1) % commands.Length;
                return null;
            }

            string command = commands[cursor];
            return command;
        }

        public string? Previous()
        {
            if (Count == 0) return null;
            if (cursor == tail) return null;

            cursor = (cursor + (commands.Length - 1)) % commands.Length;

            return commands[cursor];
        }
    }
}
