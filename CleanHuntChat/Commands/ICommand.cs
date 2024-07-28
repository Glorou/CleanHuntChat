using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanHuntChat.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string HelpMessage { get; }
        void OnCommand(string command, string args);
        void Dispose();
    }
}
