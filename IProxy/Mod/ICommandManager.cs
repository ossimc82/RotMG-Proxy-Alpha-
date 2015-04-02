using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProxy.Mod
{
    public interface ICommandManager
    {
        IEnumerable<string> RegisterCommands();
        bool OnCommandGet(string command, string[] args);
    }
}
