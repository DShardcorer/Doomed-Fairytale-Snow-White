using DateTimeDayNightSystem;
using GeneralManagers;
using UnityEngine;
using EventBus.Time;
using UnityEngine.SceneManagement;

namespace DateDayNightSystem
{
    public class GameTimeManager : MonoBehaviour, ILifecycle<GameManager>
    {
        [Header("Time Settings")]
        [SerializeField] private int startDay = 1;
        [SerializeField] [Range(0f, 24f)] private float startTimeOfDay = 0f;
        [SerializeField] private float secondsPerGameDay = 1200f;
        [SerializeField] private bool startPaused = false;
        [SerializeField] private float timeScale = 200f;

        [Header("Time Reversal")]
        [SerializeField] private bool enableTimeReversal = true;
        [SerializeField] private float timeReversalScale = 1f;
        [SerializeField] private bool _isReversing = false;

        [Header("Update Intervals")]
        [SerializeField] private float timeAdvanceInterval = 0.5f;
        [SerializeField] private const float EVENT_CHECK_INTERVAL = 1f;
        [SerializeField] private const float LIGHTING_UPDATE_INTERVAL = 1f;

        // Properties
        public bool IsReversing
        {
            get => _isReversing;
            set => _isReversing = enableTimeReversal && value;
        }

        public int CurrentDay { get; private set; }
        public TimePoint CurrentTime { get; private set; }
        public float NormalizedTime => CurrentTime.Normalized;
        public bool IsDaytime => CurrentTime.hourOfDay > DayPhases.Dawn.hourOfDay &&
                                 CurrentTime.hourOfDay < DayPhases.Dusk.hourOfDay;
        public bool IsNighttime => !IsDaytime;
        public DayPhase CurrentPhase { get; private set; }
        public bool IsPaused { get; private set; }
        public GameDateTime CurrentDateTime => new GameDateTime(CurrentDay, CurrentTime);
        public GameTimeEvents TimeEvents => _timeEvents;

        // Private fields
        private DayPhase _lastPhase;
        private float _timeAccumulator = 0f;
        private float _lastEventCheckTime;
        private float _lastLightingUpdateTime;
        private GameManager _parent;
        private GameTimeEvents _timeEvents;

        public void Initialize(GameManager parent)
        {
            _parent = parent;
            _timeEvents = new GameTimeEvents();
            CurrentDay = startDay;
            CurrentTime = new TimePoint { hourOfDay = startTimeOfDay };
            IsPaused = startPaused;
            IsReversing = false;
            UpdateDayPhase();
            _lastPhase = CurrentPhase;
        }

        public void InvokeInitialEvents()
        {
            TimeEventSystem.InvokeDateChanged(CurrentDay);
            TimeEventSystem.InvokeTimeChanged(CurrentTime);
            TimeEventSystem.InvokeGameDateTimeChanged(CurrentDateTime);
            TimeEventSystem.InvokeDayPhaseChanged(CurrentPhase);
        }

        public void Dispose()
        {
            _parent = null;
            Destroy(gameObject);
        }

        private void Update()
        {
            if (IsPaused) return;

            _timeAccumulator += Time.deltaTime;
            if (_timeAccumulator >= timeAdvanceInterval)
            {
                float timeDelta = _timeAccumulator * timeScale;
                if (IsReversing)
                    timeDelta = -timeDelta * timeReversalScale;

                AdvanceTime(timeDelta);
                _timeAccumulator = 0f;
                UpdateDayPhase();
            }
        }

        #region Time Advance APIS

        /// <summary>
        /// Advances time by the specified number of in-game minutes
        /// </summary>
        public void AdvanceTimeByMinutes(float minutes)
        {
            // 1 day = 1440 minutes
            float dayFraction = minutes / 1440f;
            AdvanceTime(dayFraction * secondsPerGameDay);
        }

        /// <summary>
        /// Advances time by the specified number of in-game hours
        /// </summary>
        public void AdvanceTimeByHours(float hours)
        {
            // 1 day = 24 hours
            float dayFraction = hours / 24f;
            AdvanceTime(dayFraction * secondsPerGameDay);
        }

        /// <summary>
        /// Advances time by the specified number of in-game days
        /// </summary>
        public void AdvanceTimeByDays(float days)
        {
            AdvanceTime(days * secondsPerGameDay);
        }

        /// <summary>
        /// Advances time by the specified number of game seconds
        /// (where secondsPerGameDay represents a full day)
        /// </summary>
        public void AdvanceTimeByGameSeconds(float gameSeconds)
        {
            AdvanceTime(gameSeconds);
        }

        #endregion

        public void ToggleTimeReversal()
        {
            if (!enableTimeReversal) return;
            IsReversing = !IsReversing;
        }

        public void StartTimeReversal()
        {
            IsReversing = true;
        }

        public void StopTimeReversal()
        {
            IsReversing = false;
        }

        public void SetDateTime(int day, float hourOfDay)
        {
            SetDateTime(new GameDateTime(day, new TimePoint { hourOfDay = hourOfDay }));
        }

        public void SetDateTime(GameDateTime dateTime)
        {
            bool dayChanged = CurrentDay != dateTime.day;
            bool timeChanged = !Mathf.Approximately(CurrentTime.hourOfDay, dateTime.timeOfDay.hourOfDay);

            CurrentDay = dateTime.day;
            CurrentTime = dateTime.timeOfDay;
            UpdateDayPhase();

            if (dayChanged)
                TimeEventSystem.InvokeDateChanged(CurrentDay);
            if (timeChanged)
                TimeEventSystem.InvokeTimeChanged(CurrentTime);
            if (dayChanged || timeChanged)
                TimeEventSystem.InvokeGameDateTimeChanged(CurrentDateTime);
            if (CurrentPhase != _lastPhase)
            {
                TimeEventSystem.InvokeDayPhaseChanged(CurrentPhase);
                _lastPhase = CurrentPhase;
            }
        }

        public void SetDay(int day)
        {
            if (day < 1) return;
            if (CurrentDay != day)
            {
                CurrentDay = day;
                TimeEventSystem.InvokeDateChanged(CurrentDay);
                TimeEventSystem.InvokeGameDateTimeChanged(CurrentDateTime);
            }
        }

        public void SetTimeOfDay(float hourOfDay)
        {
            hourOfDay = Mathf.Clamp(hourOfDay, 0f, 24f);
            if (!Mathf.Approximately(CurrentTime.hourOfDay, hourOfDay))
            {
                CurrentTime = new TimePoint { hourOfDay = hourOfDay };
                UpdateDayPhase();
                TimeEventSystem.InvokeTimeChanged(CurrentTime);
                TimeEventSystem.InvokeGameDateTimeChanged(CurrentDateTime);
                if (CurrentPhase != _lastPhase)
                {
                    TimeEventSystem.InvokeDayPhaseChanged(CurrentPhase);
                    _lastPhase = CurrentPhase;
                }
            }
        }

        public void AdvanceToNextDay(float startHour = 6f)
        {
            SetDateTime(CurrentDay + 1, startHour);
        }

        public void PauseTime() => IsPaused = true;
        public void ResumeTime() => IsPaused = false;
        public void TogglePause() => IsPaused = !IsPaused;
        public void SetTimeScale(float scale) => timeScale = Mathf.Max(0f, scale);
        public void SetDayLength(float seconds) => secondsPerGameDay = Mathf.Max(0f, seconds);

        public void AdvanceTime(float deltaSeconds)
        {
            float dayFraction = deltaSeconds / secondsPerGameDay;
            float newHourOfDay = CurrentTime.hourOfDay + (dayFraction * 24f);

            // Handle day changes, allowing for negative days when reversing time
            int daysToAdd = Mathf.FloorToInt(newHourOfDay / 24f);
            newHourOfDay %= 24f;

            // Handle negative hours
            if (newHourOfDay < 0)
            {
                newHourOfDay += 24f;
                daysToAdd -= 1;
            }

            // Prevent going below day 1
            if (CurrentDay + daysToAdd < 1)
            {
                daysToAdd = 1 - CurrentDay;
                newHourOfDay = 0f;
            }

            SetDateTime(CurrentDay + daysToAdd, newHourOfDay);
        }

        public string GetTimeString() => CurrentTime.GetTimeString();
        public string GetDateTimeString() => $"Day {CurrentDay}, {GetTimeString()}";

        private void UpdateDayPhase()
        {
            float hour = CurrentTime.hourOfDay;
            if (hour >= 21f || hour < 5f)
                CurrentPhase = DayPhase.Night;
            else if (hour < 8f)
                CurrentPhase = DayPhase.Dawn;
            else if (hour < 11f)
                CurrentPhase = DayPhase.Morning;
            else if (hour < 14f)
                CurrentPhase = DayPhase.Noon;
            else if (hour < 17f)
                CurrentPhase = DayPhase.Afternoon;
            else if (hour < 19f)
                CurrentPhase = DayPhase.Dusk;
            else
                CurrentPhase = DayPhase.Evening;
        }
    }
}