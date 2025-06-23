using System;
using EntityBase;

namespace EntitySystems.Skill.ActiveSkills.Player.Shoot
{
    public class ShootProperties: EntityStateProperties
    {
        private float _shootDamage = 20;
        private float _shootRange = 100;
        private float _shootKnockbackForce =3;

        public float ShootDamage => _shootDamage;
        public float ShootRange => _shootRange;
        public float ShootKnockbackForce => _shootKnockbackForce;

        public ShootProperties(ShootActiveSkillInfoSO shootActiveSkillInfoSO)
        {
            _shootDamage = shootActiveSkillInfoSO.ShootDamage;
            _shootRange = shootActiveSkillInfoSO.ShootRange;
            _shootKnockbackForce = shootActiveSkillInfoSO.ShootKnockbackForce;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
            _shootDamage = CombatStatBoard.PhysicalAttack.ModifiedValue;
        }
    }
}
