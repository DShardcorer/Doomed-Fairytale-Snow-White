using System;
using System.Collections.Generic;
using DateDayNightSystem;
using EventSystem.Time;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DateTimeDayNightSystem
{
    /// <summary>
    /// Provides an easy way to add time-based events
    /// </summary>
    public class GameTimeEvents :MonoBehaviour
    {
        private GameTimeManager gameTimeManager;

        #region Inspector Fields

        [Header("Daily Events")] public UnityEvent onDawn = new UnityEvent();
        public UnityEvent onMorning = new UnityEvent();
        public UnityEvent onNoon = new UnityEvent();
        public UnityEvent onAfternoon = new UnityEvent();
        public UnityEvent onDusk = new UnityEvent();
        public UnityEvent onEvening = new UnityEvent();
        public UnityEvent onMidnight = new UnityEvent();

        [Header("Custom Time Events")] [SerializeField]
        private List<GameTimeEvent> customTimeEvents = new List<GameTimeEvent>();

        #endregion

        #region Helper Classes

        [Serializable]
        public class GameTimeEvent
        {
            public string eventName;
            [Range(0f, 24f)] public float triggerHour;
            public bool repeatsDaily = true;
            public UnityEvent onTrigger = new UnityEvent();

            [HideInInspector] public bool hasTriggeredToday = false;
        }

        #endregion

        #region Unity Lifecycle

        public void Awake()
        {
            // Subscribe to events
            TimeEventSystem.OnDayPhaseChanged += OnDayPhaseChanged;
            TimeEventSystem.OnDateChanged += OnDateChanged;
            TimeEventSystem.OnTimeChanged += OnTimeChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            gameTimeManager = FindAnyObjectByType<GameTimeManager>();
        }

        public void OnDestroy()
        {
            gameTimeManager = null;
            TimeEventSystem.OnDayPhaseChanged -= OnDayPhaseChanged;
            TimeEventSystem.OnDateChanged -= OnDateChanged;
            TimeEventSystem.OnTimeChanged -= OnTimeChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region Event Handlers

        private void OnDayPhaseChanged(TimeEventSystem.DayPhaseChangedEventArgs obj)
        {
            switch (obj.Phase)
            {
                case DayPhase.Dawn:
                    onDawn?.Invoke();
                    break;
                case DayPhase.Morning:
                    onMorning?.Invoke();
                    break;
                case DayPhase.Noon:
                    onNoon?.Invoke();
                    break;
                case DayPhase.Afternoon:
                    onAfternoon?.Invoke();
                    break;
                case DayPhase.Dusk:
                    onDusk?.Invoke();
                    break;
                case DayPhase.Evening:
                    onEvening?.Invoke();
                    break;
                case DayPhase.Night:
                    // Only trigger midnight at exactly midnight
                    if (Mathf.Approximately(gameTimeManager.CurrentTime.hourOfDay, 0f))
                        onMidnight?.Invoke();
                    break;
            }
        }

        private void OnDateChanged(TimeEventSystem.DateChangedEventArgs obj)
        {
            // Reset custom events for the new day
            foreach (GameTimeEvent timeEvent in customTimeEvents)
            {
                timeEvent.hasTriggeredToday = false;
            }
        }

        private void OnTimeChanged(TimeEventSystem.TimeChangedEventArgs obj)
        {
            // Check custom time events
            CheckCustomTimeEvents(obj.TimePoint);
        }

        #endregion

        #region Private Methods

        private void CheckCustomTimeEvents(TimePoint currentTime)
        {
            const float TRIGGER_THRESHOLD = 0.1f; // 6 minutes in game time

            foreach (var timeEvent in customTimeEvents)
            {
                if (timeEvent.hasTriggeredToday && timeEvent.repeatsDaily)
                    continue;

                // Check if we've crossed the trigger hour
                bool hasTriggered =
                    IsTimeWithinThreshold(currentTime.hourOfDay, timeEvent.triggerHour, TRIGGER_THRESHOLD);

                if (hasTriggered)
                {
                    timeEvent.onTrigger?.Invoke();
                    timeEvent.hasTriggeredToday = true;
                }
            }
        }

        private bool IsTimeWithinThreshold(float currentHour, float targetHour, float threshold)
        {
            // Handle cases where we're crossing midnight
            if (targetHour == 0f || targetHour == 24f)
            {
                return (currentHour > 24f - threshold) || (currentHour < threshold);
            }

            return Mathf.Abs(currentHour - targetHour) < threshold;
        }

        #endregion
    }
}