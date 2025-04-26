using System;
using Entity;

namespace EntitySystems.Skill.ActiveSkills.Player.Dash
{
    public class DashProperties: EntityStateProperties
    {
        private float _dashSpeed = 10f;

        public float DashSpeed => _dashSpeed;

        public DashProperties(DashActiveSkillInfoSO activeSkillInfoSO)
        {
            _dashSpeed = activeSkillInfoSO.DashSpeed;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        }
    }
}
