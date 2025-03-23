using UnityEngine;

public class NPCProperties : EntityProperties
{
    private float _chaseRange;
    public float ChaseRange => _chaseRange;

    public NPCProperties(EntityFaction entityFaction, float moveSpeed, float chaseRange) : base(entityFaction, moveSpeed)
    {
        _chaseRange = chaseRange;
    }

}

