using EntitySystems.Level;
using EntitySystems.Stats;
using GeneralManagers;

namespace EntitySystems.Skill
{
    public abstract class PassiveSkill : ILifecycle<PassiveSkillSystem>
    {
        protected PassiveSkillSystem passiveSkillSystem;
        public PassiveSkillSystem PassiveSkillSystem => passiveSkillSystem;
        protected StatSystem statSystem;
        public StatSystem StatSystem => statSystem;
        
        public SkillInfoSO SkillInfo { get; protected set; }

        public PassiveSkill(SkillInfoSO skillInfo)
        {
            SkillInfo = skillInfo;
        }

        public void Initialize(PassiveSkillSystem parent)
        {
            this.passiveSkillSystem = parent;
            statSystem = this.passiveSkillSystem.Parent.StatSystem;
        }

        public void Dispose()
        {
            passiveSkillSystem = null;
            statSystem = null;
        }

        public abstract void ApplyEffect();
        public abstract void UnapplyEffect();
    }
}