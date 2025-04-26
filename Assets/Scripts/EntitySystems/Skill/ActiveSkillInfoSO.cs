using UnityEngine;

namespace EntitySystems.Skill
{
    [CreateAssetMenu(fileName = "ActiveSkillInfoSO", menuName = "SkillInfoSO/ActiveSkillInfoSO")]
    public abstract class ActiveSkillInfoSO:SkillInfoSO
    {
        public float cooldown;
        public float healthCost;
        public float manaCost;
        public float staminaCost;

        public abstract ActiveSkill Create();
    }
}