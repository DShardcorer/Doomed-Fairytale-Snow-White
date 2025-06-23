
using UnityEngine;
using Helpers;

namespace EntityBase.NPC.AI.SubAI
{
    public class CombatNPCSubAIController : NPCSubAIController
    {
        private float _lostTargetTimer = 0f;
        private float _lostTargetTimeout = 3f;
        private Vector3 _lastTargetPosition;

        private float _attackCooldown = 0.3f;
        private float _attackCooldownTimer = 0f;
        private bool _isOnAttackCooldown = false;

        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
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

            switch (currentStateId)
            {
                case HelperNPCStateName.Idle:
                    if (IsInAttackRange() && !_isOnAttackCooldown)
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    else if (!IsInAttackRange())
                    {
                        ChangeToState(HelperNPCStateName.Chase);
                    }
                    break;

                case HelperNPCStateName.Chase:
                    if (IsInAttackRange() && !_isOnAttackCooldown)
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    // No direct movement logic here; let the Chase state handle it
                    break;

                case HelperNPCStateName.Attack:
                    _isOnAttackCooldown = true;
                    _attackCooldownTimer = 0f;
                    break;
            }
        }

        private void HandleLostTarget()
        {
            RequestControllerChange(parent.InitialSubAIControllerId(), "Lost target, switching to initial controller");
        }

        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            return false;
        }

        public bool IsAttackOnCooldown() => _isOnAttackCooldown;

        public float GetRemainingCooldown()
        {
            if (_isOnAttackCooldown)
                return _attackCooldown - _attackCooldownTimer;
            return 0f;
        }
    }
}