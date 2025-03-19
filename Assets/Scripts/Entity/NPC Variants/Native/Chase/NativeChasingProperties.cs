using UnityEngine;

public class NativeChasingProperties
{
    private float chaseSpeed = 3.0f;
    public float ChaseSpeed => chaseSpeed;

    private float chasingTime = 2.0f;
    public float ChasingTime => chasingTime;

    private float attackCooldown = 2.0f;
    public float AttackCooldown => attackCooldown;

    private float attackRange = 3.0f;
    public float AttackRange => attackRange;

    public NativeChasingProperties(float chaseSpeed = 3.0f, float chasingTime = 2.0f, float attackCooldown = 1.0f, float attackRange = 3.0f)
    {
        this.chaseSpeed = chaseSpeed;
        this.chasingTime = chasingTime;
        this.attackCooldown = attackCooldown;
        this.attackRange = attackRange;
    }
}
