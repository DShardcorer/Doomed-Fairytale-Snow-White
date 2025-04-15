namespace EntitySystems.Skill
{
    public interface ISkill
    {
        public string SkillName { get; protected set; }
        public bool IsMindBound { get; protected set; }
    }
}