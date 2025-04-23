using UnityEngine;

namespace DateDayNightSystem
{
    
    /// <summary>
    /// Provides utility functions for working with game time
    /// </summary>
    public static class GameTimeUtility
    {
        /// <summary>
        /// Convert hours to a normalized time value (0-1)
        /// </summary>
        public static float HoursToNormalized(float hours)
        {
            return Mathf.Repeat(hours / 24f, 1f);
        }
        
        /// <summary>
        /// Convert normalized time (0-1) to hours (0-24)
        /// </summary>
        public static float NormalizedToHours(float normalized)
        {
            return Mathf.Repeat(normalized, 1f) * 24f;
        }
        
        /// <summary>
        /// Create a TimePoint from hour and minute values
        /// </summary>
        public static TimePoint CreateTimePoint(int hour, int minute)
        {
            float hourOfDay = hour + (minute / 60f);
            return new TimePoint { hourOfDay = Mathf.Repeat(hourOfDay, 24f) };
        }
        
        /// <summary>
        /// Format a time point as a 12-hour clock string (e.g., "3:30 PM")
        /// </summary>
        public static string FormatTime12Hour(TimePoint time)
        {
            int hour = Mathf.FloorToInt(time.hourOfDay);
            int minute = Mathf.FloorToInt((time.hourOfDay - hour) * 60f);
            string period = hour >= 12 ? "PM" : "AM";
            
            // Convert to 12-hour format
            hour = hour % 12;
            if (hour == 0) hour = 12;
            
            return $"{hour}:{minute:00} {period}";
        }
        
        /// <summary>
        /// Format a time point as a 24-hour clock string (e.g., "15:30")
        /// </summary>
        public static string FormatTime24Hour(TimePoint time)
        {
            int hour = Mathf.FloorToInt(time.hourOfDay);
            int minute = Mathf.FloorToInt((time.hourOfDay - hour) * 60f);
            return $"{hour:00}:{minute:00}";
        }
    }
}