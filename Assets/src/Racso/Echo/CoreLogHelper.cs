namespace Racso.Echo
{
    internal static class CoreLogHelper
    {
        public static uint FNV1a32(string str, uint hash = 2166136261u)
        {
            // The hash parameter is useful for chaining hashes.

            foreach (char c in str)
            {
                hash ^= c;
                hash *= 16777619u;
            }

            return hash;
        }

        internal static string GetLabel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => "[DEBUG]",
                LogLevel.Info => "[INFO]",
                LogLevel.Warn => "[WARN]",
                LogLevel.Error => "[ERROR]",
                _ => "[???]"
            };
        }

        internal static T GetElementFromHash<T>(T[] collection, string stringToHash)
        {
            uint hash = FNV1a32(stringToHash);
            int index = (int)(hash % (uint)collection.Length);
            return collection[index];
        }
    }
}