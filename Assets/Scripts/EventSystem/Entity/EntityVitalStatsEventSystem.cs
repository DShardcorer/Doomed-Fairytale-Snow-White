using System;
using UnityEngine;

public class EntityVitalStatsEventSystem : MonoBehaviour
{
    public static Action<Entity, HealthChangedEventArgs> HealthChanged { get; internal set; }

    public static void InvokeHealthChanged(Entity sender, HealthChangedEventArgs e)
    {
        HealthChanged?.Invoke(sender, e);
    }

    


}
