using System;
using System.Collections.Generic;

namespace Racso.Echo.LogWriters
{
    internal class ConsoleLogWriter : EchoLogWriter
    {
        private readonly LogWriterConfig config;

        private static readonly ConsoleColor[] SystemColors = WritersHelpers.SystemColors;
        private static readonly Dictionary<LogLevel, ConsoleColor> LevelColors = WritersHelpers.LevelColors;

        public ConsoleLogWriter(LogWriterConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void WriteLog(LogLevel level, string system, string message)
        {
            if (config.Timestamp)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[{0:yyyy-MM-dd HH:mm:ss.fff}] ", DateTime.Now);
            }

            if (config.LevelColors)
                Console.ForegroundColor = LevelColors[level];
            Console.Write(WritersHelpers.GetLabel(level));
            Console.Write(" ");
            if (config.LevelColors)
                Console.ResetColor();

            if (config.SystemColors)
                Console.ForegroundColor = Helpers.GetElementFromHash(SystemColors, system);
            Console.Write("[");
            Console.Write(system);
            Console.Write("] ");
            if (config.SystemColors)
                Console.ResetColor();

            Console.WriteLine(message);
        }
    }
}