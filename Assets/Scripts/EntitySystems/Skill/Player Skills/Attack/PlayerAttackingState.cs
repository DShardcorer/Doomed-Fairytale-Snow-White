using System;
using Entity.AttackCheck;
using Entity.Player.State;

namespace EntitySystems.Skill.Player_Skills.Attack
{
    public class PlayerAttackingState : PlayerState
    {
        private PlayerAttackingProperties _playerAttackProperties;

        public PlayerAttackingState(string animationBoolName, PlayerAttackingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _playerAttackProperties = entityStateProperties;
        }

        public override void EnterState()
        {
            base.EnterState();
            _player.IsBusy = true;
            _rigidbody.linearVelocity = _playerAttackProperties.AttackVelocity * _player.PlayerProperties.lastMovementVector;
        }
        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (!_isAnimationEnded)
            {
            }
            else
            {
                _stateMachine.ChangeState(_player.PlayerIdlingState);
            }
        }


        public override void ExitState()
        {
            _player.IsBusy = false;
            base.ExitState();
        }

        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            _entity.AttackHitbox.PerformAttack(AttackType.OverlapCircle, _playerAttackProperties.AttackDamage);
        }








    }
}
