using System;

namespace Racso.Echo.LogWriters
{
    internal class ConsoleLogWriter : LogWriter
    {
        private readonly LogWriterConfig config;
        private static readonly ConsoleColor[] ConsoleColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));

        public ConsoleLogWriter(LogWriterConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void WriteLog(LogLevel level, string system, string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[{0:yyyy-MM-dd HH:mm:ss.fff}] ", DateTime.Now);

            Console.ForegroundColor = GetLevelColor(level);
            Console.Write(Helpers.GetLabel(level));
            Console.Write(" ");

            Console.ForegroundColor = GetConsoleColor(system);
            Console.Write("[");
            Console.Write(system);
            Console.Write("] ");
            Console.ResetColor();

            Console.WriteLine(message);
        }

        private ConsoleColor GetConsoleColor(string system)
        {
            uint hash = Helpers.FNV1a32(system);
            int index = (int)(hash % (uint)ConsoleColors.Length);
            return ConsoleColors[index];
        }

        private ConsoleColor GetLevelColor(LogLevel level) =>
            level switch
            {
                LogLevel.Info => ConsoleColor.Cyan,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                _ => ConsoleColor.Gray
            };
    }
}