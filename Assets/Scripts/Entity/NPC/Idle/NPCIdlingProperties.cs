using UnityEngine;

public class NPCIdlingProperties
{
    private float idleTime = 2.0f;
    public float IdleTime => idleTime;

    public NPCIdlingProperties(float idleTime)
    {
        this.idleTime = idleTime;
    }
}
