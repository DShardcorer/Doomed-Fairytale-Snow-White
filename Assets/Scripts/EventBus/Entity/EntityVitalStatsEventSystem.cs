using System;
using EventBus.Player;
using UnityEngine;

namespace EventBus.Entity
{
    public class EntityVitalStatsEventSystem : MonoBehaviour
    {
        public static Action<global::EntityBase.Entity, HealthChangedEventArgs> HealthChanged { get; internal set; }

        public static void InvokeHealthChanged(global::EntityBase.Entity sender, HealthChangedEventArgs e)
        {
            HealthChanged?.Invoke(sender, e);
        }

    


    }
}
