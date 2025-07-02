using System;
using EntityBase;

namespace EntitySystems.Skill.ActiveSkills.Player.Kick
{
    public class KickProperties: EntityStateProperties
    {
        private float _kickDamage;
        private float _kickRange;
        private float _kickVelocity;
        private float _kickKnockbackForce;

        public float KickDamage => _kickDamage;
        public float KickRange => _kickRange;
        public float KickVelocity => _kickVelocity;
        public float KickKnockbackForce => _kickKnockbackForce;

        public KickProperties(KickActiveSkillInfoSO shootActiveSkillInfoSO)
        {
            _kickDamage = shootActiveSkillInfoSO.KickDamage;
            _kickRange = shootActiveSkillInfoSO.KickRange;
            _kickVelocity = shootActiveSkillInfoSO.KickVelocity;
            _kickKnockbackForce = shootActiveSkillInfoSO.KickKnockbackForce;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
            _kickDamage = CombatStatBoard.PhysicalAttack.ModifiedValue;
        }
        
    }
}