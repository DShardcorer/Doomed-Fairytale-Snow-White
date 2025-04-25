using DateTimeDayNightSystem;
using GeneralManagers;
using UnityEngine;
using EventSystem.Time;
using UnityEngine.SceneManagement;

namespace DateDayNightSystem
{
    /// <summary>
    /// Core system that manages game time and date
    /// </summary>
    public class GameTimeManager : MonoBehaviour, ILifecycle<GameManager>
    {
        #region Inspector Fields

        [Header("Time Settings")] [SerializeField]
        private int startDay = 1;

        [SerializeField] [Range(0f, 24f)] private float startTimeOfDay = 6f; // 6 AM by default
        [SerializeField] private float secondsPerGameDay = 1200f; // 20 minutes real time = 1 day
        [SerializeField] private bool startPaused = false;
        private float timeScale = 200f; // Additional time speed multiplier

        #endregion

        #region Properties

        /// <summary>
        /// Current day number, starting from 1
        /// </summary>
        public int CurrentDay { get; private set; }

        public TimePoint CurrentTime { get; private set; }

        /// <summary>
        /// Current time of day normalized (0-1)
        /// </summary>
        public float NormalizedTime => CurrentTime.Normalized;

        /// <summary>
        /// Returns whether it's currently daytime (between dawn and dusk)
        /// </summary>
        public bool IsDaytime => CurrentTime.hourOfDay > DayPhases.Dawn.hourOfDay &&
                                 CurrentTime.hourOfDay < DayPhases.Dusk.hourOfDay;

        /// <summary>
        /// Returns whether it's currently nighttime
        /// </summary>
        public bool IsNighttime => !IsDaytime;

        /// <summary>
        /// Current phase of the day
        /// </summary>
        public DayPhase CurrentPhase { get; private set; }

        /// <summary>
        /// Is the time system currently paused
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Complete current date-time
        /// </summary>
        public GameDateTime CurrentDateTime => new GameDateTime(CurrentDay, CurrentTime);

        #endregion

        #region Private Fields

        private DayPhase _lastPhase;
        private float _lastEventCheckTime;
        private const float EVENT_CHECK_INTERVAL = 1f; // Check phase changes every 0.1 seconds
        private float _lastLightingUpdateTime;
        private const float LIGHTING_UPDATE_INTERVAL = 1f; // Update lighting once per second

        private GameManager _parent;
        private GameTimeEvents _timeEvents;
        public GameTimeEvents TimeEvents => _timeEvents;

        #endregion

        #region Unity Lifecycle

        public void Initialize(GameManager parent)
        {
            _parent = parent;

            // Initialize time
            CurrentDay = startDay;
            CurrentTime = new TimePoint { hourOfDay = startTimeOfDay };
            IsPaused = startPaused;

            // Calculate initial day phase
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
            if (IsPaused)
                return;

            // Advance time
            AdvanceTime(Time.deltaTime * timeScale);

            // Check for phase changes periodically rather than every frame
            _lastEventCheckTime += Time.deltaTime;
            if (_lastEventCheckTime >= EVENT_CHECK_INTERVAL)
            {
                UpdateDayPhase();
                _lastEventCheckTime = 0f;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the game time to a specific day and time
        /// </summary>
        public void SetDateTime(int day, float hourOfDay)
        {
            SetDateTime(new GameDateTime(day, new TimePoint { hourOfDay = hourOfDay }));
        }

        /// <summary>
        /// Sets the game time to a specific datetime
        /// </summary>
        public void SetDateTime(GameDateTime dateTime)
        {
            bool dayChanged = CurrentDay != dateTime.day;
            bool timeChanged = !Mathf.Approximately(CurrentTime.hourOfDay, dateTime.timeOfDay.hourOfDay);

            CurrentDay = dateTime.day;
            CurrentTime = dateTime.timeOfDay;

            // Update phase
            UpdateDayPhase();

            // Trigger events
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

        /// <summary>
        /// Sets the day to a specific value
        /// </summary>
        public void SetDay(int day)
        {
            if (day < 1)
            {
                Debug.LogError("GameTimeManager: Day cannot be less than 1.");
                return;
            }

            if (CurrentDay != day)
            {
                CurrentDay = day;
                TimeEventSystem.InvokeDateChanged(CurrentDay);
                TimeEventSystem.InvokeGameDateTimeChanged(CurrentDateTime);
            }
        }

        /// <summary>
        /// Sets the time of day
        /// </summary>
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

        /// <summary>
        /// Advances to the next day at the specified hour
        /// </summary>
        public void AdvanceToNextDay(float startHour = 6f)
        {
            SetDateTime(CurrentDay + 1, startHour);
        }

        /// <summary>
        /// Pauses the time system
        /// </summary>
        public void PauseTime()
        {
            IsPaused = true;
        }

        /// <summary>
        /// Resumes the time system
        /// </summary>
        public void ResumeTime()
        {
            IsPaused = false;
        }

        /// <summary>
        /// Toggles the paused state
        /// </summary>
        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }

        /// <summary>
        /// Sets the time scale (1 = normal, 2 = double speed, etc.)
        /// </summary>
        public void SetTimeScale(float scale)
        {
            timeScale = Mathf.Max(0f, scale);
        }

        /// <summary>
        /// Sets how long a day lasts in real-time seconds
        /// </summary>
        public void SetDayLength(float seconds)
        {
            if (seconds <= 0f)
            {
                Debug.LogError("GameTimeManager: Day length must be greater than 0.");
                return;
            }

            secondsPerGameDay = seconds;
        }

        /// <summary>
        /// Advances time by the specified amount of real seconds
        /// </summary>
        public void AdvanceTime(float deltaSeconds)
        {
            // Calculate how much game time passes
            float dayFraction = deltaSeconds / secondsPerGameDay;
            float newHourOfDay = CurrentTime.hourOfDay + (dayFraction * 24f);

            // Check for day change
            int daysToAdd = Mathf.FloorToInt(newHourOfDay / 24f);
            newHourOfDay %= 24f;

            // Apply changes
            SetDateTime(CurrentDay + daysToAdd, newHourOfDay);
        }

        /// <summary>
        /// Get a formatted time string (HH:MM)
        /// </summary>
        public string GetTimeString()
        {
            return CurrentTime.GetTimeString();
        }

        /// <summary>
        /// Get a formatted date-time string
        /// </summary>
        public string GetDateTimeString()
        {
            return $"Day {CurrentDay}, {GetTimeString()}";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the current day phase based on time
        /// </summary>
        private void UpdateDayPhase()
        {
            float hour = CurrentTime.hourOfDay;

            if (hour >= 21f || hour < 5f)
                CurrentPhase = DayPhase.Night;
            else if (hour >= 5f && hour < 8f)
                CurrentPhase = DayPhase.Dawn;
            else if (hour >= 8f && hour < 11f)
                CurrentPhase = DayPhase.Morning;
            else if (hour >= 11f && hour < 14f)
                CurrentPhase = DayPhase.Noon;
            else if (hour >= 14f && hour < 17f)
                CurrentPhase = DayPhase.Afternoon;
            else if (hour >= 17f && hour < 19f)
                CurrentPhase = DayPhase.Dusk;
            else
                CurrentPhase = DayPhase.Evening;
        }

        #endregion
    }
}