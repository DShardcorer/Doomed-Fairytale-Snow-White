using System;
using EntityBase.AttackCheck;
using EntityBase.NPC.AI;
using EntitySystems.Stats;

namespace EntityBase.NPC.StandardAI.Attack
{
    public class NPCMeleeAttackProperties: EntityStateProperties
    {
        public AttackType AttackType { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackDamage { get; private set; }
        public float AttackCooldown { get; private set; } = 1.0f;

        public NPCMeleeAttackProperties(NPCAIConfiguration config)
        {
            AttackType = config.attackType;
            AttackRange = config.attackRange;
            AttackDamage = config.attackDamage;
            AttackCooldown = config.attackCooldown;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
            AttackDamage = CombatStatBoard.PhysicalAttack.ModifiedValue;
        }
    }
}