using UnityEngine;

[CreateAssetMenu(fileName = "NPCProperties", menuName = "NPC/NPCProperties", order = 1)]
public class NPCPropertiesSO : ScriptableObject
{
    public float maxHealth;
    public float moveSpeed;
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;

}