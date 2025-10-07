#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_BUILD
using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Racso.Echo.LogWriters
{
    internal class UnityLogWriter : LogWriter
    {
        private readonly LogWriterConfig config;
        private readonly StringBuilder stringBuilder = new(256);

        private static readonly string[] SystemColorTags = GetSystemColorTags();
        private static readonly string[] LevelColorTags = GetLevelColorTags();

        public UnityLogWriter(LogWriterConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void WriteLog(LogLevel level, string system, string message)
        {
            stringBuilder.Clear();
            if (config.Timestamp)
                stringBuilder.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}]", DateTime.Now);

            stringBuilder.Append(LevelColorTags[(int)level]);
            stringBuilder.Append(Helpers.GetLabel(level));
            stringBuilder.Append("</color>");

            if (config.SystemColors)
            {
                string colorTag = Helpers.GetElementFromHash(SystemColorTags, system);
                stringBuilder.Append(colorTag);
                stringBuilder.Append("[");
                stringBuilder.Append(system);
                stringBuilder.Append("]</color> ");
            }
            else
            {
                stringBuilder.Append($"[{system}] ");
            }

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

        private static string[] GetSystemColorTags()
        {
            Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta };
            return colors.Select(ColorToTag).ToArray();
        }

        private static string[] GetLevelColorTags()
        {
            string[] tags = new string[Enum.GetValues(typeof(LogLevel)).Length];
            SetForLevel(LogLevel.Debug, Color.white);
            SetForLevel(LogLevel.Info, Color.cyan);
            SetForLevel(LogLevel.Warn, Color.yellow);
            SetForLevel(LogLevel.Error, Color.red);
            return tags;

            void SetForLevel(LogLevel level, Color color)
                => tags[(int)level] = ColorToTag(color);
        }

        private static string ColorToTag(Color color)
            => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
    }
}
#endif