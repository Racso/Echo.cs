#if UNITY_2017_1_OR_NEWER
using System;
using System.Text;
using UnityEngine;

namespace Racso.Echo.LogWriters
{
    internal class UnityLogWriter : EchoLogWriter
    {
        private readonly LogWriterConfig config;
        private readonly StringBuilder stringBuilder = new(256);

        private readonly string[] levelColorTags;
        private readonly string[] systemColorTags;

        public UnityLogWriter(LogWriterConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));

            levelColorTags = WritersHelpers.GetLevelColorTags(ColorToTag);
            systemColorTags = WritersHelpers.GetSystemColorTags(ColorToTag);
        }

        public void WriteLog(LogLevel level, string system, string message)
        {
            stringBuilder.Clear();
            if (config.Timestamp)
                stringBuilder.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}] ", DateTime.Now);

            AppendIfEnabled(stringBuilder, config.LevelColors, levelColorTags[(int)level]);
            stringBuilder.Append("[");
            stringBuilder.Append(WritersHelpers.GetLabel(level));
            stringBuilder.Append("]");
            AppendIfEnabled(stringBuilder, config.LevelColors, "</color>");
            stringBuilder.Append(" ");

            if (config.SystemColors)
            {
                string colorTag = Helpers.GetElementFromHash(systemColorTags, system);
                stringBuilder.Append(colorTag);
            }

            stringBuilder.Append("[");
            stringBuilder.Append(system);
            stringBuilder.Append("]");
            AppendIfEnabled(stringBuilder, config.SystemColors, "</color>");
            stringBuilder.Append(" ");

            stringBuilder.Append(message);
            string fullMessage = stringBuilder.ToString();

            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    Debug.Log(fullMessage);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning(fullMessage);
                    break;
                case LogLevel.Error:
                    Debug.LogError(fullMessage);
                    break;
            }
        }

        private void AppendIfEnabled(StringBuilder stringBuilder, bool enabled, string value)
        {
            if (enabled)
                stringBuilder.Append(value);
        }

        private static string ColorToTag(ConsoleColor color)
        {
            Color unityColor = color switch
            {
                ConsoleColor.Black => Color.black,
                ConsoleColor.Blue => Color.blue,
                ConsoleColor.Cyan => Color.cyan,
                ConsoleColor.DarkBlue => new Color(0f, 0f, 0.5f),
                ConsoleColor.DarkCyan => new Color(0f, 0.5f, 0.5f),
                ConsoleColor.DarkGray => Color.gray,
                ConsoleColor.DarkGreen => new Color(0f, 0.5f, 0f),
                ConsoleColor.DarkMagenta => new Color(0.5f, 0f, 0.5f),
                ConsoleColor.DarkRed => new Color(0.5f, 0f, 0f),
                ConsoleColor.DarkYellow => new Color(0.5f, 0.5f, 0f),
                ConsoleColor.Gray => Color.grey,
                ConsoleColor.Green => Color.green,
                ConsoleColor.Magenta => Color.magenta,
                ConsoleColor.Red => Color.red,
                ConsoleColor.White => Color.white,
                ConsoleColor.Yellow => Color.yellow,
                _ => Color.white
            };

            return $"<color=#{ColorUtility.ToHtmlStringRGB(unityColor)}>";
        }
    }
}
#endif