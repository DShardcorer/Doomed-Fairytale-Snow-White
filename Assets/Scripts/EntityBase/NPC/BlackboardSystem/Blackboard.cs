using System;
using System.Collections.Generic;

namespace EntityBase.NPC.BlackboardSystem
{
    [Serializable]
    public class Blackboard
    {
        private Dictionary<string, BlackboardKey> keyRegistry = new();
        private Dictionary<BlackboardKey, object> entries = new();

        public bool TryGetValue<T>(BlackboardKey key, out T value)
        {
            if (entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void SetValue<T>(BlackboardKey key, T value)
        {
            entries[key] = new BlackboardEntry<T>(key, value);
        }

        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            if (!keyRegistry.TryGetValue(keyName, out var key))
            {
                key = new BlackboardKey(keyName);
                keyRegistry[keyName] = key;
            }

            return key;
        }

        public bool ContainsKey(string keyName)
        {
            return keyRegistry.ContainsKey(keyName);
        }

        public void RemoveKey(string keyName)
        {
            if (keyRegistry.TryGetValue(keyName, out var key))
            {
                keyRegistry.Remove(keyName);
                entries.Remove(key);
            }
        }

        public void Debug()
        {
            foreach (var entry in entries)
            {
                var EntryType = entry.Value.GetType();
                if (EntryType.IsGenericType && EntryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
                {
                    var ValueProperty = EntryType.GetProperty("Value");
                    if(ValueProperty == null) continue;
                    var Value = ValueProperty.GetValue(entry.Value);
                    UnityEngine.Debug.Log($"{entry.Key}: {Value}");
                }
            }
        }
    }
}