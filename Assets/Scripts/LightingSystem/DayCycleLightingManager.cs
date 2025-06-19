using System.Collections.Generic;
using DateDayNightSystem;
using DateTimeDayNightSystem;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Collections;
using EventBus.Time;

namespace DefaultNamespace.LightingSystem
{
    /// <summary>
    /// Handle the cycle of Day and Night. Everything that need to change across time will register itself to this handler
    /// which will update it when it update (e.g. ShadowInstance, Interpolator etc.).
    /// The ticking of that system can be stopped, this is useful e.g. if the game is put in pause (or need to do cutscene
    /// etc..)
    /// </summary>
    public class DayCycleLightingManager : MonoBehaviour, ILifecycle<GameManager>
    {
        public Transform LightsRoot;

        [Header("Day Light")] public Light2D DayLight;
        public Gradient DayLightGradient;

        [Header("Night Light")] public Light2D NightLight;
        public Gradient NightLightGradient;

        [Header("Ambient Light")] public Light2D AmbientLight;
        public Gradient AmbientLightGradient;

        [Header("RimLights")] public Light2D SunRimLight;
        public Gradient SunRimLightGradient;
        public Light2D MoonRimLight;
        public Gradient MoonRimLightGradient;

        [Tooltip("The angle 0 = upward, going clockwise to 1 along the day")]
        public AnimationCurve ShadowAngle;

        [Tooltip("The scale of the normal shadow length (0 to 1) along the day")]
        public AnimationCurve ShadowLength;

        private List<ShadowInstance> _shadows = new();
        private List<LightInterpolator> _lightBlenders = new();

        private GameTimeManager _gameTimeManager;
        public void Initialize(GameManager parent)
        {
            TimeEventSystem.OnTimeChanged += OnTimeChanged;
            _gameTimeManager = parent.GameTimeManager;
        }

        public void Dispose()
        {
            TimeEventSystem.OnTimeChanged -= OnTimeChanged;

            // Unregister all shadows and light blenders
            _shadows.Clear();
            _lightBlenders.Clear();

            // Clear references to lights
            DayLight = null;
            NightLight = null;
            AmbientLight = null;
            SunRimLight = null;
            MoonRimLight = null;
            LightsRoot = null;
        }

        private void OnTimeChanged(TimeEventSystem.TimeChangedEventArgs obj)
        {
            UpdateLight(obj.TimePoint.Normalized);
        }

        public void UpdateLight(float ratio)
        {
            DayLight.color = DayLightGradient.Evaluate(ratio);
            NightLight.color = NightLightGradient.Evaluate(ratio);
            
            if (AmbientLight)
                AmbientLight.color = AmbientLightGradient.Evaluate(ratio);
                
            if (SunRimLight)
                SunRimLight.color = SunRimLightGradient.Evaluate(ratio);
                
            if (MoonRimLight)
                MoonRimLight.color = MoonRimLightGradient.Evaluate(ratio);

            LightsRoot.rotation = Quaternion.Euler(0, 0, 360.0f * ratio);

            UpdateShadow(ratio);
        }

        void UpdateShadow(float ratio)
        {
            var currentShadowAngle = ShadowAngle.Evaluate(ratio);
            var currentShadowLength = ShadowLength.Evaluate(ratio);

            var opposedAngle = currentShadowAngle + 0.5f;
            while (currentShadowAngle > 1.0f)
                currentShadowAngle -= 1.0f;

            foreach (var shadow in _shadows)
            {
                var t = shadow.transform;
                t.eulerAngles = new Vector3(0, 0, currentShadowAngle * 360.0f);
                t.localScale = new Vector3(1, 1f * shadow.BaseLength * currentShadowLength, 1);
            }

            foreach (var handler in _lightBlenders)
            {
                handler.SetRatio(ratio);
            }
        }

        public void RegisterShadow(ShadowInstance shadow)
        {
            _shadows.Add(shadow);
        }

        public void UnregisterShadow(ShadowInstance shadow)
        {
            _shadows.Remove(shadow);
        }

        public void RegisterLightBlender(LightInterpolator interpolator)
        {
            _lightBlenders.Add(interpolator);
        }

        public void UnregisterLightBlender(LightInterpolator interpolator)
        {
            _lightBlenders.Remove(interpolator);
        }
    }
}