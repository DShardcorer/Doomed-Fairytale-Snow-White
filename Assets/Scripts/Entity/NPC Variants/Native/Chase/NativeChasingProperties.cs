using UnityEngine;

public class NativeChasingProperties
{
    private float chaseSpeed = 3.0f;
    public float ChaseSpeed => chaseSpeed;

    private float chasingTime = 2.0f;
    public float ChasingTime => chasingTime;

    public NativeChasingProperties(float chaseSpeed = 3.0f, float chasingTime = 2.0f)
    {
        this.chaseSpeed = chaseSpeed;
        this.chasingTime = chasingTime;
    }
}
