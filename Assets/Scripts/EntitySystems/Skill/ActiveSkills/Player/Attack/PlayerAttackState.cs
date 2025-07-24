using System;
using EntityBase;
using EntityBase.AttackCheck;
using EntityBase.Player.State;
using EntitySystems.Equipment;
using EntitySystems.WeaponSystem;
using Helpers;
using UnityEngine;
using Utilities;

namespace EntitySystems.Skill.ActiveSkills.Player.Attack
{
    public class PlayerAttackState : PlayerState
    {
        private PlayerAttackingProperties _playerAttackProperties;
        private WeaponSystem.WeaponSystem _weaponSystem;
        private Weapon _activeWeapon => _weaponSystem?.PrimaryWeapon;
        private EquipmentSystem _equipmentSystem => _player.EquipmentSystem;
        
        // Attack handler component
        private AttackHandler _attackHandler;
        public int CurrentAttackCounter => _attackHandler.CurrentAttackCounter;
        
        public PlayerAttackState(PlayerAttackingProperties entityStateProperties) : this(
            HelperAnimationStateName.IS_ATTACKING, entityStateProperties)
        {
        }

        public PlayerAttackState(string animationBoolName, PlayerAttackingProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _playerAttackProperties = entityStateProperties;
            _attackHandler = new AttackHandler(numberOfAttacks: 2, resetTime: 0.5f);
        }

        public override void Initialize(EntityBase.Player.Player parent)
        {
            base.Initialize(parent);
            _weaponSystem = parent.WeaponSystem;
        }

        public override void Dispose()
        {
            base.Dispose();
            _weaponSystem = null;
            _attackHandler.Dispose();
        }

        public override void EnterState()
        {
            base.EnterState();
            
            _view.SetAttackCounter(CurrentAttackCounter);
            _player.IsBusy = true;
            if (_equipmentSystem.IsPrimaryWeaponEquipped())
            {
                _view.SetWeaponType(_activeWeapon.WeaponData.WeaponType);
                _activeWeapon.Enter();
            }
            else
            {
                _view.SetWeaponType(WeaponType.Barehanded);
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();
            _attackHandler.Tick(Time.deltaTime);
            if(_isAnimationEnded)
            {
                _stateMachine.ChangeState(_entity.IdleState);
            }
        }

        public override void ExitState()
        {
            _player.IsBusy = false;
            _attackHandler.IncrementCounter();
            _view.SetAttackCounter(CurrentAttackCounter);
            if (_equipmentSystem.IsPrimaryWeaponEquipped())
            {
                _activeWeapon.Exit();
            }
            base.ExitState();
        }

        protected override void OnTakingEffect()
        {
            if (_equipmentSystem.IsPrimaryWeaponEquipped())
            {
                return;
            }
            _entity.AttackHitbox.PerformAttack(AttackType.OverlapCircle,
                _player.StatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue);
        }
    }
}