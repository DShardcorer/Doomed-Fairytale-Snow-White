using UnityEngine;

public class DashSkill : Skill
{
    private PlayerDashingState _dashingState;
    public DashSkill(string skillName, float cooldown, PlayerDashingState playerDashingState) : base(skillName, cooldown)
    {
        _dashingState = playerDashingState;
    }
    public override void Initialize(SkillSystem parent)
    {
        base.Initialize(parent);
        if (_parent.Parent is Player player)
        {
            _dashingState.Initialize(player);
        }
        else
        {
            Debug.LogError($"DashSkill initialized with a non-Player entity: {_parent.Parent}");
        }

    }


    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
    protected override void UseSkill()
    {
        base.UseSkill();
        _parent.StateMachine.ChangeState(_dashingState);
    }


}
