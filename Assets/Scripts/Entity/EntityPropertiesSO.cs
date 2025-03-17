using UnityEngine;

[CreateAssetMenu(fileName = "EntityProperties", menuName = "Entity/EntityProperties")]
public class EntityPropertiesSO : ScriptableObject
{
    public EntityFaction faction;
    public float maxHealth;
    public float moveSpeed;

    public float attackDamage;
    public float attackCooldown;

    public AttackType attackType; // Select attack type in Inspector
    public float attackRadius = 1.5f; // For OverlapCircle
    public Vector2 attackBoxSize = new Vector2(2f, 1f); // For OverlapBox
    public Vector2 attackCapsuleSize = new Vector2(2f, 1f); // For OverlapCapsule
    public float attackRange = 2f;

}
