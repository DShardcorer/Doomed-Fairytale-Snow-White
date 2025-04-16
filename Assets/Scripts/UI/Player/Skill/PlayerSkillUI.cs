
using UnityEngine;

namespace UI.Player.Skill
{
    public class PlayerSkillUI: IngameMenuPageUI
    {
        [SerializeField] private ActiveSkillUI activeSkillUI;
        [SerializeField] private PassiveSkillUI passiveSkillUI;
        public override void Initialize(IngameMenuUI parent)
        {
            base.Initialize(parent);
            passiveSkillUI.Initialize(this);
        }
        
    }
}