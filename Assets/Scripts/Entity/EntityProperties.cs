using UnityEngine;

public class EntityProperties
{
    public EntityFaction entityFaction;
    public Vector2 lastMovementVector = Vector2.zero;
    public Entity target;
    public Vector2 currentPosition;

    protected float maxHealth;
    protected float currentHealth;
    protected float moveSpeed;
    protected float attackRange;
    protected float attackDamage;
    protected float attackCooldown;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

    public EntityProperties (EntityPropertiesSO entityPropertiesSO){
        maxHealth = entityPropertiesSO.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = entityPropertiesSO.moveSpeed;
        attackRange = entityPropertiesSO.attackRange;
        attackDamage = entityPropertiesSO.attackDamage;
        attackCooldown = entityPropertiesSO.attackCooldown;
        entityFaction = entityPropertiesSO.faction;
    }

}
