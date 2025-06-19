using System;
using DateTimeDayNightSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DefaultNamespace.LightingSystem
{
    /// <summary>
    /// This class will blend between multiple Light2D shaped across the day. This allow to have moving Shadow, used for
    /// example by the house or barn shadow.
    /// </summary>
    [DefaultExecutionOrder(999)]
    public class LightInterpolator : MonoBehaviour
    {
        [Serializable]
        public class LightFrame
        {
            public Light2D ReferenceLight;
            public float NormalizedTime;
        }

        [Tooltip("The light of which the shape will be changed according to the defined frames")]
        public Light2D TargetLight;
        public LightFrame[] LightFrames;

        private DayCycleLightingManager _dayCycleLightingManager;

        private void OnEnable()
        {
            if (!_dayCycleLightingManager)
            {
                _dayCycleLightingManager = FindAnyObjectByType<DayCycleLightingManager>();
            }
            _dayCycleLightingManager.RegisterLightBlender(this);
        }

        private void OnDisable()
        {
            if (_dayCycleLightingManager)
            {
                _dayCycleLightingManager.UnregisterLightBlender(this);
            }
            DisableAllLights();
        }

        public void SetRatio(float t)
        {
            if(LightFrames.Length == 0)
                return;

            int startFrame = 0;

            while (startFrame < LightFrames.Length - 1 && LightFrames[startFrame + 1].NormalizedTime < t)
            {
                startFrame += 1;
            }

            if (startFrame == LightFrames.Length - 1)
            {
                //the last frame is the "start frame" so there is no frame to interpolate TO, so we just use the last settings
                Interpolate(LightFrames[startFrame].ReferenceLight, LightFrames[startFrame].ReferenceLight, 0.0f);
            }
            else
            {
                float frameLength = LightFrames[startFrame + 1].NormalizedTime - LightFrames[startFrame].NormalizedTime;
                float frameValue = t - LightFrames[startFrame].NormalizedTime;
                float normalizedFrame = frameValue / frameLength;

                Interpolate(LightFrames[startFrame].ReferenceLight, LightFrames[startFrame + 1].ReferenceLight, normalizedFrame);
            }
        }

        void Interpolate(Light2D start, Light2D end, float t)
        {
            TargetLight.color = Color.Lerp(start.color, end.color, t);
            TargetLight.intensity = Mathf.Lerp(start.intensity, end.intensity, t);

            var startPath = start.shapePath;
            var endPath = end.shapePath;

            var newPath = new Vector3[startPath.Length];

            for (int i = 0; i < startPath.Length; ++i)
            {
                newPath[i] = Vector3.Lerp(startPath[i], endPath[i], t);
            }

            TargetLight.SetShapePath(newPath);
        }

        public void DisableAllLights()
        {
            foreach (var frame in LightFrames)
            {
                frame.ReferenceLight?.gameObject.SetActive(false);
            }
        }
    }
}