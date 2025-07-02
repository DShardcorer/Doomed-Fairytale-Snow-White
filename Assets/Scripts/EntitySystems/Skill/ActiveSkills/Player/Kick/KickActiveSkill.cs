using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Kick
{
    public class KickActiveSkill: ActiveSkill
    {
        private KickState _kickingState;
        public KickActiveSkill(KickActiveSkillInfoSO activeSkillInfoSO) : base(activeSkillInfoSO)
        {
            _kickingState = new KickState(activeSkillInfoSO);
        }
        
        
        public override void Initialize(ActiveSkillSystem parent)
        {
            base.Initialize(parent);
            if (base.parent.Parent is EntityBase.Player.Player player)
            {
                _kickingState.Initialize(player);
            }
            else
            {
                Debug.LogError($"Kick initialized with a non-Player entity: {base.parent.Parent}");
            }
        }


        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        protected override void UseSkill()
        {
            base.UseSkill();
            parent.StateMachine.ChangeState(_kickingState);
        }
    }
}