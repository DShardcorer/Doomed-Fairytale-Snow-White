namespace DateDayNightSystem
{
    /// <summary>
    /// Predefined time points for common day phases
    /// </summary>
    public static class DayPhases
    {
        public static readonly TimePoint Midnight = new TimePoint { hourOfDay = 0f };
        public static readonly TimePoint Dawn = new TimePoint { hourOfDay = 6f };
        public static readonly TimePoint Morning = new TimePoint { hourOfDay = 9f };
        public static readonly TimePoint Noon = new TimePoint { hourOfDay = 12f };
        public static readonly TimePoint Afternoon = new TimePoint { hourOfDay = 15f };
        public static readonly TimePoint Dusk = new TimePoint { hourOfDay = 18f };
        public static readonly TimePoint Night = new TimePoint { hourOfDay = 21f };
    }
}