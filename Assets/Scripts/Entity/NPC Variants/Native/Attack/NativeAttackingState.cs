using Entity.NPC.Attack;

namespace Entity.NPC_Variants.Native.Attack
{
    public class NativeAttackingState : NPCAttackingState
    {
        protected NativeAttackingProperties _nativeAttackingProperties;

        public NativeAttackingState(string animationBoolName, NativeAttackingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _nativeAttackingProperties = entityStateProperties;
        }

        public NativeAttackingProperties NativeAttackingProperties => _nativeAttackingProperties;

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (!_isAnimationEnded)
            {
                _rigidbody.linearVelocity = 0.5f * _npc.NPCProperties.lastMovementVector * _npc.NPCProperties.MoveSpeed;
            }
            else
            {
                _stateMachine.ChangeState(_npc.NPCChasingState);
            }
        }


    }
}
