using System;
using DateTimeDayNightSystem;
using EventSystem.Time;
using Febucci.UI;
using GeneralManagers;
using UnityEngine;

namespace DefaultNamespace.UI.Time
{
    public class TimeUI: MonoBehaviour
    {
        [SerializeField] private TextAnimator_TMP dayText;
        [SerializeField] private TextAnimator_TMP hourText;
        [SerializeField] private TextAnimator_TMP minuteText;


        private void Awake()
        {
            TimeEventSystem.OnDateChanged += OnDateChanged;
            TimeEventSystem.OnTimeChanged += OnTimeChanged;
        }

        private void OnDestroy()
        {
            TimeEventSystem.OnDateChanged -= OnDateChanged;
            TimeEventSystem.OnTimeChanged -= OnTimeChanged;
        }

        private void OnTimeChanged(TimeEventSystem.TimeChangedEventArgs obj)
        {
            TimePoint timePoint = obj.TimePoint;
            minuteText.textFull = timePoint.GetMinuteString();
            if (String.Equals(hourText.textFull, timePoint.GetHourString()))
            {
                hourText.textFull = timePoint.GetHourString();
            }
        }

        private void OnDateChanged(TimeEventSystem.DateChangedEventArgs obj)
        {
            dayText.textFull = obj.Day.ToString();
        }
    }
}