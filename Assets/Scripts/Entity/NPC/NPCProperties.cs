using UnityEngine;

public class NPCProperties : EntityProperties
{
    protected float chaseRange;
    public float ChaseRange => chaseRange;
    public NPCProperties(EntityFaction entityFaction, float maxHealth, float chaseRange = 10) : base(entityFaction, maxHealth)
    {
        this.chaseRange = chaseRange;
    }

}

