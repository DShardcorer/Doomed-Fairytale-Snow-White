using System;
using EventBus.Player;
using UnityEngine;

namespace EventBus.Entity
{
    public class EntityVitalStatsEventSystem : MonoBehaviour
    {
        public static Action<global::Entity.Entity, HealthChangedEventArgs> HealthChanged { get; internal set; }

        public static void InvokeHealthChanged(global::Entity.Entity sender, HealthChangedEventArgs e)
        {
            HealthChanged?.Invoke(sender, e);
        }

    


    }
}
