using EntityBase.NPC.AI;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC.State.Chase
{
    public class NPCChaseState : NPCState
    {
        private NPCChasingProperties _npcChasingProperties;

        private float _updateTargetInterval = 0.5f;
        private float _updateTargetTimer = 0f;
        private float _minDistanceToTarget = 1.5f; // Configurable minimum distance

        // New: Out-of-range timer and threshold
        private float _outOfRangeTimer = 0f;
        private float _outOfRangeTimeout = 3f; // seconds

        public NPCChaseState(NPCAIConfiguration config) : this(HelperAnimationStateName.IS_CHASING,
            new NPCChasingProperties())
        {
            if (config != null)
            {
                _minDistanceToTarget = config.attackRange * 0.8f;
            }
        }

        public NPCChaseState(string animationBoolName, NPCChasingProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _npcChasingProperties = entityStateProperties;
        }

        public override void EnterState()
        {
            base.EnterState();
            astarAI.canMove = true;
            _updateTargetTimer = 0f;
            _outOfRangeTimer = 0f;
        }
        public override void ExitState()
        {
            base.ExitState();
            astarAI.canMove = false;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (npc.NPCProperties.target == null)
            {
                _outOfRangeTimer = 0f;
                return;
            }

            _updateTargetTimer += Time.deltaTime;

            // Check distance to target
            float distance = Vector3.Distance(npc.View.transform.position,
                npc.NPCProperties.target.View.transform.position);
            if (distance > _minDistanceToTarget * 3f) // Arbitrary "too far" multiplier
            {
                _outOfRangeTimer += Time.deltaTime;
                if (_outOfRangeTimer >= _outOfRangeTimeout)
                {
                    npcAIController.UnsetTarget();
                    _outOfRangeTimer = 0f;
                }
            }
            else
            {
                _outOfRangeTimer = 0f;
            }

            if (!npcAIController.HasTarget())
            {
                return;
            }

            // Update destination periodically
            if (_updateTargetTimer >= _updateTargetInterval)
            {
                _updateTargetTimer = 0f;
                UpdateTargetPosition();
            }
        }

        private void UpdateTargetPosition()
        {
            Vector3 targetPosition =
                npc.NPCProperties.target.View.transform.position;
            Vector3 directionToTarget = (targetPosition - npc.View.transform.position).normalized;
            Vector3 destinationPosition = targetPosition - (directionToTarget * _minDistanceToTarget);
            AstarAI.destination = destinationPosition;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            Vector3 directionToTarget = (npc.NPCProperties.target.View.transform.position -
                                         npc.View.transform.position).normalized;
            npc.NPCProperties.lastMovementVector = directionToTarget;
        }


        public void SetMinDistanceToTarget(float distance)
        {
            if (distance > 0)
            {
                _minDistanceToTarget = distance;
            }
        }

        public void SetUpdateTargetInterval(float interval)
        {
            if (interval > 0)
            {
                _updateTargetInterval = interval;
            }
        }
    }
}