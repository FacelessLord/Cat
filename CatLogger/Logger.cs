using System.Collections.Generic;
using System.Linq;

namespace CatLexing
{
    public class Logger : ILogger
    {
        public Dictionary<string, List<string>> Channels { get; private set; }

        public Logger()
        {
            Channels = LogLevel.Levels.ToDictionary(l => l, l => new List<string>());
        }

        public void Log(string level, string source, string message)
        {
            Channels[level].Add($"[{source}] {message}");
        }
    }
}