using UnityEngine;

public class NPCProperties : EntityProperties
{
    protected float chaseRange;
    public float ChaseRange => chaseRange;
    public NPCProperties(NPCPropertiesSO entityPropertiesSO) : base(entityPropertiesSO)
    {
        chaseRange = entityPropertiesSO.chaseRange;
    }
}

