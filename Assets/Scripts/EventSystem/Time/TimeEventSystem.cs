using System;
using DateDayNightSystem;
using UnityEngine;

namespace EventSystem.Time
{
    public static class TimeEventSystem
    {
        // Event arguments classes
        public class DateChangedEventArgs : EventArgs
        {
            public int Day;
            public DateChangedEventArgs(int day) => Day = day;
        }

        public class TimeChangedEventArgs : EventArgs
        {
            public TimePoint TimePoint;
            public TimeChangedEventArgs(TimePoint timePoint) => TimePoint = timePoint;
        }

        public class GameDateTimeChangedEventArgs : EventArgs
        {
            public GameDateTime DateTime;
            public GameDateTimeChangedEventArgs(GameDateTime dateTime) => DateTime = dateTime;
        }

        public class DayPhaseChangedEventArgs : EventArgs
        {
            public DayPhase Phase;
            public DayPhaseChangedEventArgs(DayPhase phase) => Phase = phase;
        }

        // Events
        public static Action<DateChangedEventArgs> OnDateChanged;
        public static Action<TimeChangedEventArgs> OnTimeChanged;
        public static Action<GameDateTimeChangedEventArgs> OnGameDateTimeChanged;
        public static Action<DayPhaseChangedEventArgs> OnDayPhaseChanged;

        // Invoker methods
        public static void InvokeDateChanged(int day)
        {
            OnDateChanged?.Invoke(new DateChangedEventArgs(day));
        }

        public static void InvokeTimeChanged(TimePoint timePoint)
        {
            OnTimeChanged?.Invoke(new TimeChangedEventArgs(timePoint));
        }

        public static void InvokeGameDateTimeChanged(GameDateTime dateTime)
        {
            OnGameDateTimeChanged?.Invoke(new GameDateTimeChangedEventArgs(dateTime));
        }

        public static void InvokeDayPhaseChanged(DayPhase phase)
        {
            OnDayPhaseChanged?.Invoke(new DayPhaseChangedEventArgs(phase));
        }
    }
}