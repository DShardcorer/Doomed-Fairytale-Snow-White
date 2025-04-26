namespace EntitySystems.Skill.ActiveSkills.Player.Dash
{
    public class DashActiveSkillInfoSO: ActiveSkillInfoSO
    {
        public float DashSpeed = 10f;
        public override ActiveSkill Create()
        {
            return new DashActiveSkill(this);
        }
    }
}