using UnityEngine;

public class EntityProperties
{
    public EntityFaction entityFaction;
    public Vector2 lastMovementVector = Vector2.down;
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


    protected AttackType attackType; // Select attack type in Inspector
    protected float attackRadius = 1.5f; // For OverlapCircle
    protected Vector2 attackBoxSize = new Vector2(2f, 1f); // For OverlapBox
    protected Vector2 attackCapsuleSize = new Vector2(2f, 1f); // For OverlapCapsule


    public AttackType AttackType => attackType;
    public float AttackRadius => attackRadius;
    public Vector2 AttackBoxSize => attackBoxSize;
    public Vector2 AttackCapsuleSize => attackCapsuleSize;
    


    public EntityProperties(EntityPropertiesSO entityPropertiesSO)
    {
        maxHealth = entityPropertiesSO.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = entityPropertiesSO.moveSpeed;
        attackRange = entityPropertiesSO.attackRange;
        attackDamage = entityPropertiesSO.attackDamage;
        attackCooldown = entityPropertiesSO.attackCooldown;
        entityFaction = entityPropertiesSO.faction;
        attackType = entityPropertiesSO.attackType;
        attackRadius = entityPropertiesSO.attackRadius;
        attackBoxSize = entityPropertiesSO.attackBoxSize;
        attackCapsuleSize = entityPropertiesSO.attackCapsuleSize;
    }

}
