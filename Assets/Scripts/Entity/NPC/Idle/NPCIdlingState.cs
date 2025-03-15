using UnityEngine;

public class NPCIdlingState : NPCState
{
    private NPCIdlingProperties _npcIdlingProperties;
    public NPCIdlingState(NPCIdlingProperties npcIdlingProperties, string animationBoolName) : base(animationBoolName)
    {
        _npcIdlingProperties = npcIdlingProperties;
    }
    private float idleTimer = 0;
    public override void EnterState()
    {
        base.EnterState();
        idleTimer = _npcIdlingProperties.IdleTime;
    }

    public override void FixedUpdateState()
    {
        idleTimer -= Time.fixedDeltaTime;
        if (idleTimer <= 0)
        {
            _stateMachine.ChangeState(_npc.NPCMovingState);
        }
        base.FixedUpdateState();
    }
}
