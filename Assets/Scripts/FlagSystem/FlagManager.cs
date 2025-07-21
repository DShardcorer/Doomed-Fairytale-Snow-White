using System;
using System.Collections.Generic;
using FlagSystem.FlagSystem;
using UnityEngine;

namespace FlagSystem
{
    public class FlagManager : MonoBehaviour
    {
        public static FlagManager Instance { get; private set; }

        private Dictionary<string, GameFlag> _flags = new Dictionary<string, GameFlag>();

        // Event system for flag changes
        public event Action<OnFlagChangedEventArgs> OnFlagChanged; // id, oldValue, newValue
        public class OnFlagChangedEventArgs : EventArgs
        {
            public string FlagId { get; }
            public object OldValue { get; }
            public object NewValue { get; }

            public OnFlagChangedEventArgs(string flagId, object oldValue, object newValue)
            {
                FlagId = flagId;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Type-safe getters
        public bool GetBool(string id, bool defaultValue = false) =>
            _flags.TryGetValue(id, out var flag) && flag.GetFlagType() == FlagType.Boolean ?
                (bool)flag.GetValue() : defaultValue;

        public int GetInt(string id, int defaultValue = 0) =>
            _flags.TryGetValue(id, out var flag) && flag.GetFlagType() == FlagType.Integer ?
                (int)flag.GetValue() : defaultValue;

        public float GetFloat(string id, float defaultValue = 0f) =>
            _flags.TryGetValue(id, out var flag) && flag.GetFlagType() == FlagType.Float ?
                (float)flag.GetValue() : defaultValue;

        public string GetString(string id, string defaultValue = "") =>
            _flags.TryGetValue(id, out var flag) && flag.GetFlagType() == FlagType.String ?
                (string)flag.GetValue() : defaultValue;

        // Type-safe setters
        public void SetBool(string id, bool value) => SetFlag(id, value, FlagType.Boolean);
        public void SetInt(string id, int value) => SetFlag(id, value, FlagType.Integer);
        public void SetFloat(string id, float value) => SetFlag(id, value, FlagType.Float);
        public void SetString(string id, string value) => SetFlag(id, value, FlagType.String);

        // Base setter with change notification
        private void SetFlag(string id, object value, FlagType type)
        {
            object oldValue = null;

            if (_flags.TryGetValue(id, out var flag))
            {
                oldValue = flag.GetValue();
                // Replace with new flag since we can't set values directly
                _flags[id] = CreateFlag(id, value, type);
            }
            else
            {
                _flags[id] = CreateFlag(id, value, type);
            }

            OnFlagChanged?.Invoke(new OnFlagChangedEventArgs(id, oldValue, value));
        }

        // Helper method to create flag instances
        private GameFlag CreateFlag(string id, object value, FlagType type)
        {
            switch (type)
            {
                case FlagType.Boolean:
                    return new BoolGameFlag { id = id, value = (bool)value };
                case FlagType.Integer:
                    return new IntGameFlag { id = id, value = (int)value };
                case FlagType.Float:
                    return new FloatGameFlag { id = id, value = (float)value };
                case FlagType.String:
                    return new StringGameFlag { id = id, value = (string)value };
                default:
                    throw new ArgumentException($"Unsupported flag type: {type}");
            }
        }

        // Persistence support
        public Dictionary<string, object> SerializeFlags()
        {
            var data = new Dictionary<string, object>();
            foreach (var flag in _flags)
            {
                data[flag.Key] = new object[] { (int)flag.Value.GetFlagType(), flag.Value.GetValue() };
            }
            return data;
        }

        public void DeserializeFlags(Dictionary<string, object> data)
        {
            _flags.Clear();
            foreach (var entry in data)
            {
                var flagData = (object[])entry.Value;
                var type = (FlagType)(int)flagData[0];
                var value = flagData[1];
                _flags[entry.Key] = CreateFlag(entry.Key, value, type);
            }
        }
    }
}