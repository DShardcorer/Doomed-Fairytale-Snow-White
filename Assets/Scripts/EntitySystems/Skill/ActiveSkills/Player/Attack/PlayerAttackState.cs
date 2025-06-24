using System;
using EntityBase.AttackCheck;
using EntityBase.Player.State;
using EntitySystems.WeaponSystem;
using Helpers;
using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Attack
{
    public class PlayerAttackState : PlayerState
    {
        
        private PlayerAttackingProperties _playerAttackProperties;
        private WeaponSystem.WeaponSystem _weaponSystem;
        private Weapon _activeWeapon => _weaponSystem?.PrimaryWeapon;
        public PlayerAttackState(PlayerAttackingProperties entityStateProperties) : this(
            HelperAnimationStateName.IS_ATTACKING, entityStateProperties)
        {
        }
        public PlayerAttackState(string animationBoolName, PlayerAttackingProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _playerAttackProperties = entityStateProperties;
        }

        public override void Initialize(EntityBase.Player.Player parent)
        {
            base.Initialize(parent);
            _weaponSystem = parent.WeaponSystem;
        }

        public override void EnterState()
        {
            base.EnterState();
            _player.IsBusy = true;
            _rigidbody.linearVelocity =
                _playerAttackProperties.AttackVelocity * _player.PlayerProperties.lastMovementVector;
            _activeWeapon.Enter();

        }

        public override void UpdateState()
        {
            base.UpdateState();
            _activeWeapon.Update();
            if(_isAnimationEnded)
            {
                _stateMachine.ChangeState(_entity.IdleState);
            }
        }
        


        public override void ExitState()
        {
            _player.IsBusy = false;
            base.ExitState();
            _activeWeapon.Exit();
        }

        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            _entity.AttackHitbox.PerformAttack(AttackType.OverlapCircle,
                _player.StatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue);
        }
    }
}