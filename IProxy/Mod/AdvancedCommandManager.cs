//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProxy.Mod
{
    public abstract class AdvancedCommandManager : ICommandManager
    {
        public delegate bool Command(string[] args);

        private Dictionary<string, Command> m_commands;

        public AdvancedCommandManager()
        {
            m_commands = new Dictionary<string, Command>();
            HookCommands();
        }

        public void ApplyCommandHook(string command, Command callback)
        {
            if (command.IndexOf(" ") > -1) throw new ArgumentException("Whitespaces are not allowed in commands", "command", null);
            string commandString = command.StartsWith("/") ? command.Remove(0, 1) : command;
            if(m_commands.ContainsKey(commandString)) throw new InvalidOperationException("Command already bound to a callback");
            m_commands.Add(commandString, callback);
        }

        public IEnumerable<string> RegisterCommands()
        {
            return m_commands.Select(_ => _.Key);
        }

        public bool OnCommandGet(string command, string[] args)
        {
            if (m_commands.ContainsKey(command))
                return m_commands[command](args);
            return true;
        }

        public abstract void HookCommands();
    }
}
