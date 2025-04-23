using EventSystem.Time;
using GeneralManagers;
using UnityEngine;

namespace DateDayNightSystem
{
    /// <summary>
    /// Handles environmental effects based on time of day
    /// </summary>
    public class SkyLightingManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _parent;

        #region Inspector Fields

        [Header("Celestial Bodies")] [SerializeField]
        private Light sunLight;

        [SerializeField] private Light moonLight;
        [SerializeField] private Transform skyParent;

        [Header("Light Settings")] [SerializeField]
        private AnimationCurve sunIntensityCurve = new AnimationCurve(
            new Keyframe(0f, 0f), // Midnight
            new Keyframe(6f / 24f, 0.5f), // Dawn
            new Keyframe(12f / 24f, 1f), // Noon
            new Keyframe(18f / 24f, 0.5f), // Dusk
            new Keyframe(1f, 0f) // Midnight
        );

        [SerializeField] private AnimationCurve moonIntensityCurve = new AnimationCurve(
            new Keyframe(0f, 0.8f), // Midnight
            new Keyframe(6f / 24f, 0f), // Dawn
            new Keyframe(18f / 24f, 0f), // Dusk
            new Keyframe(1f, 0.8f) // Midnight
        );

        [SerializeField] private Color dayAmbientColor = new Color(0.75f, 0.75f, 0.9f);
        [SerializeField] private Color nightAmbientColor = new Color(0.1f, 0.1f, 0.2f);

        [Header("Sky Rotation")] [SerializeField]
        private bool rotateSky = true;

        [SerializeField] private Vector3 sunriseRotation = new Vector3(0, 0, 0);
        [SerializeField] private Vector3 noonRotation = new Vector3(90, 0, 0);
        [SerializeField] private Vector3 sunsetRotation = new Vector3(180, 0, 0);
        [SerializeField] private Vector3 midnightRotation = new Vector3(270, 0, 0);

        [Header("References")] [SerializeField]
        private GameTimeManager timeManager;

        #endregion

        #region Unity Lifecycle

        public void Initialize(GameManager parent)
        {
            _parent = parent;
        }

        public void Dispose()
        {
            _parent = null;
        }

        private void Awake()
        {
            // Create required components if not assigned
            if (!sunLight)
            {
                Debug.LogWarning("EnvironmentController: Sun light not assigned. Creating a directional light.");
                GameObject sunObj = new GameObject("Sun");
                sunLight = sunObj.AddComponent<Light>();
                sunLight.type = LightType.Directional;
                sunLight.intensity = 1f;
                sunLight.transform.parent = transform;
            }

            if (!moonLight)
            {
                Debug.LogWarning("EnvironmentController: Moon light not assigned. Creating a directional light.");
                GameObject moonObj = new GameObject("Moon");
                moonLight = moonObj.AddComponent<Light>();
                moonLight.type = LightType.Directional;
                moonLight.intensity = 0.3f;
                moonLight.color = new Color(0.6f, 0.6f, 1f);
                moonLight.transform.parent = transform;
            }

            if (!skyParent)
            {
                skyParent = new GameObject("Sky").transform;
                skyParent.parent = transform;
                sunLight.transform.parent = skyParent;
                moonLight.transform.parent = skyParent;
            }

            // Find time manager if not assigned
            if (!timeManager)
                timeManager = FindObjectOfType<GameTimeManager>();

            if (!timeManager)
            {
                Debug.LogError("EnvironmentController: No GameTimeManager found in scene!");
            }
        }

        private void Start()
        {
            if (timeManager)
            {
                // Initial update
                UpdateEnvironment(timeManager.NormalizedTime);

                // Subscribe to time change events
                TimeEventSystem.OnTimeChanged += OnTimeChanged;
            }
        }


        private void OnDestroy()
        {
            TimeEventSystem.OnTimeChanged -= OnTimeChanged;
        }

        #endregion

        #region Event Handlers

        private void OnTimeChanged(TimeEventSystem.TimeChangedEventArgs obj)
        {
            UpdateEnvironment(obj.TimePoint.Normalized);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Manually update the environment based on a normalized time (0-1)
        /// </summary>
        public void UpdateEnvironment(float normalizedTime)
        {
            UpdateSkyRotation(normalizedTime);
            UpdateLightIntensities(normalizedTime);
            UpdateAmbientLight(normalizedTime);
        }

        #endregion

        #region Private Methods

        private void UpdateSkyRotation(float normalizedTime)
        {
            if (!rotateSky)
                return;

            float timeHours = normalizedTime * 24f;
            Quaternion rotation;

            if (timeHours <= 6f) // Midnight to dawn
            {
                float factor = timeHours / 6f;
                rotation = Quaternion.Euler(Vector3.Lerp(midnightRotation, sunriseRotation, factor));
            }
            else if (timeHours <= 12f) // Dawn to noon
            {
                float factor = (timeHours - 6f) / 6f;
                rotation = Quaternion.Euler(Vector3.Lerp(sunriseRotation, noonRotation, factor));
            }
            else if (timeHours <= 18f) // Noon to dusk
            {
                float factor = (timeHours - 12f) / 6f;
                rotation = Quaternion.Euler(Vector3.Lerp(noonRotation, sunsetRotation, factor));
            }
            else // Dusk to midnight
            {
                float factor = (timeHours - 18f) / 6f;
                rotation = Quaternion.Euler(Vector3.Lerp(sunsetRotation, midnightRotation, factor));
            }

            skyParent.rotation = rotation;

            // Moon is opposite the sun
            moonLight.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
        }

        private void UpdateLightIntensities(float normalizedTime)
        {
            // Update light intensities
            sunLight.intensity = sunIntensityCurve.Evaluate(normalizedTime);
            moonLight.intensity = moonIntensityCurve.Evaluate(normalizedTime);
        }

        private void UpdateAmbientLight(float normalizedTime)
        {
            float timeHours = normalizedTime * 24f;

            // Determine if it's daytime (between 6 AM and 6 PM)
            bool isDaytime = timeHours > 6f && timeHours < 18f;

            if (isDaytime)
            {
                // During day, interpolate toward day color
                float factor = Mathf.InverseLerp(6f, 12f, timeHours);
                if (timeHours > 12f)
                    factor = Mathf.InverseLerp(18f, 12f, timeHours);

                RenderSettings.ambientLight = Color.Lerp(
                    Color.Lerp(nightAmbientColor, dayAmbientColor, 0.5f),
                    dayAmbientColor,
                    factor
                );
            }
            else
            {
                // During night, interpolate toward night color
                float factor = (timeHours <= 6f)
                    ? Mathf.InverseLerp(6f, 0f, timeHours)
                    : Mathf.InverseLerp(18f, 24f, timeHours);

                RenderSettings.ambientLight = Color.Lerp(
                    Color.Lerp(dayAmbientColor, nightAmbientColor, 0.5f),
                    nightAmbientColor,
                    factor
                );
            }
        }

        #endregion
    }
}