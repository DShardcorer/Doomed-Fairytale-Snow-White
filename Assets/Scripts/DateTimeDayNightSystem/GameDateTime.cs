using System;

namespace DateTimeDayNightSystem
{
    /// <summary>
    /// Represents a complete date-time
    /// </summary>
    [Serializable]
    public struct GameDateTime
    {
        public int day;
        public TimePoint timeOfDay;
        
        public GameDateTime(int day, TimePoint timeOfDay)
        {
            this.day = day;
            this.timeOfDay = timeOfDay;
        }
        
        public GameDateTime(int day, float normalizedTime)
        {
            this.day = day;
            this.timeOfDay = TimePoint.FromNormalized(normalizedTime);
        }
    }
}