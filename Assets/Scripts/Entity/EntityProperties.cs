using UnityEngine;

public class EntityProperties
{
    public EntityFaction entityFaction;
    public Vector2 lastMovementVector = Vector2.down;
    public Entity target;
    public Vector2 currentPosition;
    protected float moveSpeed;
    public float MoveSpeed => moveSpeed;

    public EntityProperties(EntityFaction entityFaction, float moveSpeed)
    {
        this.entityFaction = entityFaction;
        this.moveSpeed = moveSpeed;
    }

}
