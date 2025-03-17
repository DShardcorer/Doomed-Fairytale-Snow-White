using UnityEngine;

[CreateAssetMenu(fileName = "EntityProperties", menuName = "Entity/EntityProperties")]
public class EntityPropertiesSO : ScriptableObject
{
    public EntityFaction faction;
    public float maxHealth;
    public float moveSpeed;
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;

}
