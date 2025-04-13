using System;
using System.Collections.Generic;

namespace NarrativeSystem
{
    public class WorldState
    {
        private Dictionary<string, bool> flags = new Dictionary<string, bool>();
        public event Action<string, bool> OnFlagChanged;

        public void SetFlag(string flagName, bool value)
        {
            flags[flagName] = value;
            OnFlagChanged?.Invoke(flagName, value);
        }

        public bool GetFlag(string flagName)
        {
            flags.TryGetValue(flagName, out bool value);
            return value;
        }
        
    }
}