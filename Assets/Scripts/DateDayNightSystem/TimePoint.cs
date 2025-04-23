using System;
using UnityEngine;

namespace DateDayNightSystem
{
    /// <summary>
    /// Represents a specific time of day
    /// </summary>
    [Serializable]
    public struct TimePoint
    {
        [Range(0f, 24f)] public float hourOfDay;
        
        // Normalized value between 0-1
        public float Normalized => hourOfDay / 24f;
        
        public static TimePoint FromNormalized(float normalized)
        {
            return new TimePoint { hourOfDay = Mathf.Clamp01(normalized) * 24f };
        }
        
        public string GetTimeString()
        {
            int hours = Mathf.FloorToInt(hourOfDay);
            int minutes = Mathf.FloorToInt((hourOfDay - hours) * 60f);
            return $"{hours:00}:{minutes:00}";
        }
    }
}