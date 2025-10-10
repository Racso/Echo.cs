namespace Racso.Echo
{
    internal static class Helpers
    {
        internal static uint FNV1a32(string str, uint hash = 2166136261u)
        {
            // The hash parameter is useful for chaining hashes.

            foreach (char c in str)
            {
                hash ^= c;
                hash *= 16777619u;
            }

            return hash;
        }

        internal static T GetElementFromHash<T>(T[] collection, string stringToHash)
        {
            uint hash = FNV1a32(stringToHash);
            int index = (int)(hash % (uint)collection.Length);
            return collection[index];
        }
    }
}