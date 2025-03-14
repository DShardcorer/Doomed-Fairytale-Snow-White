using UnityEngine;

public class EnemyIdlingProperties
{
    private float idleTime = 2.0f;
    public float IdleTime => idleTime;

    public EnemyIdlingProperties(float idleTime)
    {
        this.idleTime = idleTime;
    }
}
