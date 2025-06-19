using System;
using Entity.AttackCheck;
using Entity.Player.State;

namespace EntitySystems.Skill.ActiveSkills.Player.Attack
{
    public class PlayerAttackState : PlayerState
    {
        private PlayerAttackingProperties _playerAttackProperties;

        public PlayerAttackState(string animationBoolName, PlayerAttackingProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _playerAttackProperties = entityStateProperties;
        }

        public override void EnterState()
        {
            base.EnterState();
            _player.IsBusy = true;
            _rigidbody.linearVelocity =
                _playerAttackProperties.AttackVelocity * _player.PlayerProperties.lastMovementVector;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (!_isAnimationEnded)
            {
            }
            else
            {
                _stateMachine.ChangeState(_entity.IdleState);
            }
        }


        public override void ExitState()
        {
            _player.IsBusy = false;
            base.ExitState();
        }

        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            _entity.AttackHitbox.PerformAttack(AttackType.OverlapCircle,
                _player.StatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue);
        }
    }
}