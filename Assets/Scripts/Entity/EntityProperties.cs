using UnityEngine;

public class EntityProperties
{
    public EntityFaction entityFaction;
    public Vector2 lastMovementVector = Vector2.down;
    public Entity target;
    public Vector2 currentPosition;
    protected float moveSpeed;
    public float MoveSpeed => moveSpeed;

    public EntityProperties(EntityFaction entityFaction, float moveSpeed = 5f)
    {
        this.entityFaction = entityFaction;
        this.moveSpeed = moveSpeed;
    }

}
