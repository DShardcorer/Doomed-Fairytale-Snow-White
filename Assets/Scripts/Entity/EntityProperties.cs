using UnityEngine;

public class EntityProperties
{
    public EntityFaction entityFaction;
    public Vector2 lastMovementVector = Vector2.down;
    public Entity target;
    public Vector2 currentPosition;
    public float currentHealth;
    protected float moveSpeed;
    public float MoveSpeed => moveSpeed;

    public EntityProperties(EntityFaction entityFaction, float maxHealth, float moveSpeed = 5f)
    {
        currentHealth = maxHealth;
        this.entityFaction = entityFaction;
        this.moveSpeed = moveSpeed;
    }

}
