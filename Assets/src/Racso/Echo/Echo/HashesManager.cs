using System.Collections.Generic;

namespace Racso.Echo
{
    internal class HashesManager
    {
        private readonly HashSet<uint> hashes = new();

        internal bool TryAdd(string system, string message)
        {
            uint hash = Helpers.FNV1a32(message);
            hash = Helpers.FNV1a32(system, hash);
            return hashes.Add(hash);
        }

        internal void Clear()
        {
            hashes.Clear();
        }
    }
}