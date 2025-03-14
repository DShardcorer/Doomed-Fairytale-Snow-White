using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProperties", menuName = "Enemy/EnemyProperties")]
public class EnemyPropertiesSO : ScriptableObject
{
    public float maxHealth;
    public float moveSpeed;
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;

}