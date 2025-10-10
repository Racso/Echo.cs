using System;
using System.Collections.Generic;
using System.Linq;
using Racso.EchoLogger;

namespace Assets.Racso.EchoLogger.LogWriters
{
    public static class WritersHelpers
    {
        public static string GetLevelLabel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => "DEBUG",
                LogLevel.Info => "INFO",
                LogLevel.Warn => "WARN",
                LogLevel.Error => "ERROR",
                _ => "???"
            };
        }

        public static string[] GetSystemColorTags(Func<ConsoleColor, string> colorToTag)
            => SystemColors.Select(colorToTag).ToArray();

        public static string[] GetLevelColorTags(Func<ConsoleColor, string> colorToTag)
        {
            string[] tags = new string[Enum.GetValues(typeof(LogLevel)).Length];
            foreach ((LogLevel level, ConsoleColor color) in LevelColors)
                tags[(int)level] = colorToTag(color);
            return tags;
        }

        public static readonly ConsoleColor[] SystemColors =
        {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Blue,
            ConsoleColor.Yellow,
            ConsoleColor.Cyan,
            ConsoleColor.Magenta
        };

        public static readonly Dictionary<LogLevel, ConsoleColor> LevelColors = new()
        {
            { LogLevel.Debug, ConsoleColor.White },
            { LogLevel.Info, ConsoleColor.Cyan },
            { LogLevel.Warn, ConsoleColor.Yellow },
            { LogLevel.Error, ConsoleColor.Red }
        };
    }
}