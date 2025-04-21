using UnityEngine;

namespace Entity.NPC.Move
{
    public class NPCMovingState : NPCState
    {
        private NPCMovingProperties _npcMovingProperties;
        public NPCMovingState(string animationBoolName, NPCMovingProperties npcMovingProperties) : base(animationBoolName, npcMovingProperties)
        {
            _npcMovingProperties = npcMovingProperties;
        }
        private float movingTimer;


        public override void EnterState()
        {
            base.EnterState();
            movingTimer = _npcMovingProperties.MovingTime;
            _properties.lastMovementVector = GetRandomDirection();

        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            movingTimer -= Time.fixedDeltaTime;
            npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
            _rigidbody.linearVelocity = _properties.lastMovementVector * _npcMovingProperties.MoveSpeed;
            if (movingTimer <= 0)
            {
                _stateMachine.ChangeState(npcAIController.NPCIdlingState);
            }
            base.FixedUpdateState();
        }



        public Vector2 GetRandomDirection()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

    }
}
