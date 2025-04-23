using System;
using EventSystem.Time;
using UnityEngine;
using UnityEngine.Events;

namespace DateDayNightSystem
{
    /// <summary>
    /// Provides an easy way to add time-based events
    /// </summary>
    public class GameTimeEvents : MonoBehaviour
    {
        #region Inspector Fields
        
        [Header("Time Events")]
        [SerializeField] private GameTimeManager timeManager;
        
        [Header("Daily Events")]
        public UnityEvent onDawn = new UnityEvent();
        public UnityEvent onMorning = new UnityEvent();
        public UnityEvent onNoon = new UnityEvent();
        public UnityEvent onAfternoon = new UnityEvent();
        public UnityEvent onDusk = new UnityEvent();
        public UnityEvent onEvening = new UnityEvent();
        public UnityEvent onMidnight = new UnityEvent();
        
        [Header("Custom Time Events")]
        [SerializeField] private GameTimeEvent[] customTimeEvents;
        
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
        
        private void Start()
        {
            if (!timeManager)
                timeManager = FindObjectOfType<GameTimeManager>();
                
            if (!timeManager)
            {
                Debug.LogError("GameTimeEvents: No GameTimeManager found in scene!");
                return;
            }
            
            // Subscribe to events
            TimeEventSystem.OnDayPhaseChanged += OnDayPhaseChanged;
            TimeEventSystem.OnDateChanged += OnDateChanged;
            TimeEventSystem.OnTimeChanged += OnTimeChanged;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            TimeEventSystem.OnDayPhaseChanged -= OnDayPhaseChanged;
            TimeEventSystem.OnDateChanged -= OnDateChanged;
            TimeEventSystem.OnTimeChanged -= OnTimeChanged;
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
                    if (Mathf.Approximately(timeManager.CurrentTime.hourOfDay, 0f))
                        onMidnight?.Invoke();
                    break;
            }
        }
        
        private void OnDateChanged(TimeEventSystem.DateChangedEventArgs obj)
        {
            // Reset custom events for the new day
            foreach (var timeEvent in customTimeEvents)
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
                bool hasTriggered = IsTimeWithinThreshold(currentTime.hourOfDay, timeEvent.triggerHour, TRIGGER_THRESHOLD);
                
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