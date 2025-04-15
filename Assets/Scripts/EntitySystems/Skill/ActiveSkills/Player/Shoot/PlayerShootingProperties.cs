using System;
using Entity;

namespace EntitySystems.Skill.ActiveSkills.Player.Shoot
{
    public class PlayerShootingProperties: EntityStateProperties
    {
        private float _shootDamage = 20;
        private float _shootRange = 100;
        private float _shootKnockbackForce =3;

        public float ShootDamage => _shootDamage;
        public float ShootRange => _shootRange;
        public float ShootKnockbackForce => _shootKnockbackForce;

        public PlayerShootingProperties(float shootDamage = 20, float shootRange = 100, float shootKnockbackForce = 3)
        {
            _shootDamage = shootDamage;
            _shootRange = shootRange;
            _shootKnockbackForce = shootKnockbackForce;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
            _shootDamage = CombatStatBoard.PhysicalAttack.ModifiedValue;
        }
    }
}
