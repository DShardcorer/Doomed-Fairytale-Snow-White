using System;
using Entity.AttackCheck;

namespace Entity.NPC_Variants.Native.Attack
{
    public class NativeAttackingProperties: EntityStateProperties
    {
        public AttackType AttackType { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackDamage { get; private set; }

        public NativeAttackingProperties(AttackType attackType = AttackType.OverlapCircle, float attackRange = 1f, float attackDamage = 10f)
        {
            AttackType = attackType;
            AttackRange = attackRange;
            AttackDamage = attackDamage;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
            AttackDamage = CombatStatBoard.PhysicalAttack.ModifiedValue;
        }
    }
}
