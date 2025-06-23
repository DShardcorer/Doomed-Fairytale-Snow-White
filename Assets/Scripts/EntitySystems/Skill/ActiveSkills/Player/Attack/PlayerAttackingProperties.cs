using System;
using EntityBase;

namespace EntitySystems.Skill.ActiveSkills.Player.Attack
{
    public class PlayerAttackingProperties: EntityStateProperties
    {
        private float _attackDamage = 10f;
        public float AttackDamage => _attackDamage;

        private float _attackRange = 1f;
        public float AttackRange => _attackRange;

        private float _attackVelocity = 5f;
        public float AttackVelocity => _attackVelocity;

        public PlayerAttackingProperties(float attackDamage = 10f, float attackRange = 1f, float attackVelocity = 5f)
        {
            _attackDamage = attackDamage;
            _attackRange = attackRange;
            _attackVelocity = attackVelocity;
        }


        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
            _attackDamage = CombatStatBoard.PhysicalAttack.ModifiedValue;
        }
    }
}
