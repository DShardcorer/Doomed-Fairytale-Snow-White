using UnityEngine;

public class NPCIdlingState : NPCState
{
    private NPCIdlingProperties _npcIdlingProperties;
    public NPCIdlingState(NPCIdlingProperties npcIdlingProperties, string animationBoolName) : base(animationBoolName)
    {
        _npcIdlingProperties = npcIdlingProperties;
    }
    public override void EnterState()
    {
        base.EnterState();
        _stateTimer = _npcIdlingProperties.IdleTime;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        _stateTimer -= Time.fixedDeltaTime;
        if (_stateTimer <= 0)
        {
            _stateMachine.ChangeState(_npc.NPCMovingState);
        }
        base.FixedUpdateState();
    }
}
