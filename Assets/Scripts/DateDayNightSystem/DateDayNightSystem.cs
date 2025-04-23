using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DateDayNightSystem
{
    
    #region Events
    
    /// <summary>
    /// Represents different phases of the day
    /// </summary>

    
    #endregion
    /// <summary>
    /// Full system that manages both time and visual aspects
    /// Use this for easy setup
    /// </summary>
    [AddComponentMenu("Date & Time/Complete Day Night System")]
    public class DateDayNightSystem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameTimeManager timeManager;
        [FormerlySerializedAs("skyLightingController")] [FormerlySerializedAs("environmentController")] [SerializeField] private SkyLightingManager skyLightingManager;
        [SerializeField] private GameTimeEvents eventsSystem;
        
        [Header("Auto Creation")]
        [SerializeField] private bool autoCreateComponents = true;
        
        private void Awake()
        {
            if (!autoCreateComponents)
                return;
                
            if (!timeManager)
            {
                var timeObj = new GameObject("TimeManager");
                timeObj.transform.parent = transform;
                timeManager = timeObj.AddComponent<GameTimeManager>();
            }
            
            if (!skyLightingManager)
            {
                var envObj = new GameObject("EnvironmentController");
                envObj.transform.parent = transform;
                skyLightingManager = envObj.AddComponent<SkyLightingManager>();
            }
            
            if (!eventsSystem)
            {
                var eventsObj = new GameObject("TimeEvents");
                eventsObj.transform.parent = transform;
                eventsSystem = eventsObj.AddComponent<GameTimeEvents>();
            }
        }
    }
}