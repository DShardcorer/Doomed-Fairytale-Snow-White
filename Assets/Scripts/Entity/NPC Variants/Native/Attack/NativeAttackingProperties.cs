using UnityEngine;

public class NativeAttackingProperties
{
    public AttackType AttackType { get; private set; }
    public float AttackRange { get; private set; }
    public float AttackDamage { get; private set; }

    public NativeAttackingProperties(AttackType attackType = AttackType.OverlapCircle, float attackRange = 3, float attackDamage = 10)
    {
        AttackType = attackType;
        AttackRange = attackRange;
        AttackDamage = attackDamage;
    }

}
