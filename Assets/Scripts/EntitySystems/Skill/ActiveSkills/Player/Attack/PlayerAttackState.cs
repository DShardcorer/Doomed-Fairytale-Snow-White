using System;
using EntityBase.AttackCheck;
using EntityBase.Player.State;
using EntitySystems.Equipment;
using EntitySystems.WeaponSystem;
using Helpers;
using UnityEngine;
using Utility;

namespace EntitySystems.Skill.ActiveSkills.Player.Attack
{
    public class PlayerAttackState : PlayerState
    {
           
        private PlayerAttackingProperties _playerAttackProperties;
        private WeaponSystem.WeaponSystem _weaponSystem;
        private Weapon _activeWeapon => _weaponSystem?.PrimaryWeapon;
        private EquipmentSystem _equipmentSystem => _player.EquipmentSystem;
        
        
        //Counter fields
        private int numberOfAttacks = 2;
        private int currentAttackCounter = 0;
        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set
            {
                currentAttackCounter = value;
                if (currentAttackCounter >= numberOfAttacks)
                {
                    currentAttackCounter = 0;
                }
            }
        }
        public Timer AttackCounterResetTimer { get; private set; } = new Timer(0.5f);
        
        private void ResetAttackCounter()
        {
            CurrentAttackCounter = 0;
        }
        
        
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
            AttackCounterResetTimer.OnTimerEnded += ResetAttackCounter;
        }

        public override void Dispose()
        {
            base.Dispose();
            _weaponSystem = null;
            AttackCounterResetTimer.OnTimerEnded -= ResetAttackCounter;
        }

        public override void EnterState()
        {
            base.EnterState();
            _view.SetAttackCounter(CurrentAttackCounter);
            _player.IsBusy = true;
            if (_equipmentSystem.IsPrimaryWeaponEquipped())
            {
                _activeWeapon.Enter();
            }

            AttackCounterResetTimer.StopTimer();
            _player.InvokeOnAttackStarts();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            AttackCounterResetTimer.Tick(Time.deltaTime);
            if(_isAnimationEnded)
            {
                _stateMachine.ChangeState(_entity.IdleState);
            }
        }
        


        public override void ExitState()
        {
            _player.IsBusy = false;
            CurrentAttackCounter++;
            _view.SetAttackCounter(CurrentAttackCounter);
            if (_equipmentSystem.IsPrimaryWeaponEquipped())
            {
                _activeWeapon.Exit();
            }
            AttackCounterResetTimer.StartTimer();
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