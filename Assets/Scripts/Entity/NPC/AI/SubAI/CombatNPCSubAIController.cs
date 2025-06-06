using Entity.NPC.State.Idle;
using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class CombatNPCSubAIController : NPCSubAIController
    {
        private float _lostTargetTimer = 0f;
        private float _lostTargetTimeout = 3f;
        private Vector3 _lastTargetPosition;

        // Attack cooldown variables
        private float _attackCooldown = 0.3f;
        private float _attackCooldownTimer = 0f;
        private bool _isOnAttackCooldown = false;

        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            // Initialize attack cooldown from config if available
            if (config != null)
            {
                _attackCooldown = config.attackCooldown;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _lostTargetTimer = 0f;
            _attackCooldownTimer = 0f;
            _isOnAttackCooldown = false;

            if (HasTarget())
            {
                _lastTargetPosition = npc.NPCProperties.target.View.transform.position;

                if (IsInAttackRange() && !_isOnAttackCooldown)
                {
                    ChangeToState(HelperNPCStateName.Attack);
                }
                else
                {
                    ChangeToState(HelperNPCStateName.Chase);
                }
            }
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            // Update cooldown timer
            if (_isOnAttackCooldown)
            {
                _attackCooldownTimer += Time.deltaTime;
                if (_attackCooldownTimer >= _attackCooldown)
                {
                    _isOnAttackCooldown = false;
                    _attackCooldownTimer = 0f;
                }
            }

            if (!HasTarget())
            {
                HandleLostTarget();
                return;
            }

            _lostTargetTimer = 0f;
            _lastTargetPosition = npc.NPCProperties.target.View.transform.position;

            FaceTarget();

            string currentStateId = parent.GetCurrentStateId();
            // Debug.Log($"Current State ID: {currentStateId}");

            switch (currentStateId)
            {
                case HelperNPCStateName.Idle:
                    if (IsInAttackRange() && !_isOnAttackCooldown)
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    else if (!IsInAttackRange())
                    {
                        MoveToPosition(_lastTargetPosition);
                    }
                    break;

                case HelperNPCStateName.Move:
                case HelperNPCStateName.Chase:
                    if (IsInAttackRange() && !_isOnAttackCooldown)
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    break;

                case HelperNPCStateName.Attack:
                    // Start cooldown when in attack state
                    _isOnAttackCooldown = true;
                    _attackCooldownTimer = 0f;
                    break;
            }
        }

        private void HandleLostTarget()
        {
            _lostTargetTimer += Time.deltaTime;

            if (_lostTargetTimer < _lostTargetTimeout)
            {
                string currentStateId = parent.GetCurrentStateId();

                if (currentStateId == HelperNPCStateName.Idle || currentStateId == HelperNPCStateName.Chase)
                {
                    MoveToPosition(_lastTargetPosition);
                }
            }
            else
            {
                // Give up and request return to patrol
                parent.UnsetTarget();
                RequestControllerChange(HelperNPCSubAIControllerName.Patrol, "Lost target for too long");
            }
        }

        public override bool ShouldRemainActive()
        {
            // Remain active as long as we have a target or are still searching
            return HasTarget() || _lostTargetTimer < _lostTargetTimeout;
        }

        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            if (HasTarget())
            {
                FaceTarget();
            }
        }

        // Method to check if attack is on cooldown
        public bool IsAttackOnCooldown()
        {
            return _isOnAttackCooldown;
        }

        // Method to get remaining cooldown time
        public float GetRemainingCooldown()
        {
            if (_isOnAttackCooldown)
            {
                return _attackCooldown - _attackCooldownTimer;
            }
            return 0f;
        }
    }
}