using System.Collections.Generic;

namespace Base.Shell
{
    public interface ICommandExecutable
    {
        public List<ConsoleCommandData> CommandsList { get; }
    }
}