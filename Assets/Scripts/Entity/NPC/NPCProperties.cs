using UnityEngine;

public class NPCProperties : EntityProperties
{
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

    public NPCProperties (NPCPropertiesSO npcStatsSO){
        maxHealth = npcStatsSO.maxHealth;
        currentHealth = maxHealth;
        moveSpeed = npcStatsSO.moveSpeed;
        attackRange = npcStatsSO.attackRange;
        attackDamage = npcStatsSO.attackDamage;
        attackCooldown = npcStatsSO.attackCooldown;
        entityFaction = EntityFaction.Native;
    }
}
