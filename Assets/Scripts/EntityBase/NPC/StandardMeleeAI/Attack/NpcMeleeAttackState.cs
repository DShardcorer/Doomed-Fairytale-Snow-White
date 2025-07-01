using System;
using EntityBase;
using EntityBase.NPC.State.Attack;
using EntityBase.NPC.AI;
using EntitySystems.WeaponSystem;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC.StandardAI.Attack
{
    public class NpcMeleeAttackState : NPCState
    {
        protected NPCMeleeAttackProperties NpcMeleeAttackProperties;
        
        // Attack handler component
        private AttackHandler _attackHandler;
        public int CurrentAttackCounter => _attackHandler.CurrentAttackCounter;
        
        // Assuming NPCs have weapon systems
        private WeaponSystem _weaponSystem;
        private Weapon _activeWeapon => _weaponSystem?.PrimaryWeapon;

        public NpcMeleeAttackState(NPCAIConfiguration npcaiConfiguration) :
            this(HelperAnimationStateName.IS_ATTACKING, new NPCMeleeAttackProperties(npcaiConfiguration))
        {
        }

        private NpcMeleeAttackState(string animationBoolName,
            NPCMeleeAttackProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            NpcMeleeAttackProperties = entityStateProperties;
            _attackHandler = new AttackHandler(numberOfAttacks: 2, resetTime: 0.5f);
        }

        private float attackCooldownTimer;

        public override void Initialize(NPC parent)
        {
            base.Initialize(parent);
            _weaponSystem = parent.WeaponSystem;
        }

        public override void Dispose()
        {
            base.Dispose();
            _attackHandler.Dispose();
            _weaponSystem = null;
        }

        public override void EnterState()
        {
            base.EnterState();
            npcAIController.SetCurrentSubControllerBusy(true);
            
            // Set weapon type and attack counter in view
            if (_activeWeapon != null)
            {
                _view.SetWeaponType(_activeWeapon.WeaponData.WeaponType);
                _activeWeapon.Enter();
            }
            else
            {
                _view.SetWeaponType(WeaponType.Barehanded);
            }
            _view.SetAttackCounter(CurrentAttackCounter);
        }

        public override void ExitState()
        {
            base.ExitState();
            npcAIController.SetCurrentSubControllerBusy(false);
            _attackHandler.IncrementCounter();
            _view.SetAttackCounter(CurrentAttackCounter);
            
            if (_activeWeapon != null)
            {
                _activeWeapon.Exit();
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();
            _attackHandler.Tick(Time.deltaTime);
            if (IsAnimationEnded())
            {
                npcAIController.ChangeState(HelperNPCStateName.Idle);
            }
        }

        protected override void OnTakingEffect()
        {
            base.OnTakingEffect();
            
            if (_activeWeapon != null)
            {
                return;
            }
            
            _entity.AttackHitbox.PerformAttack(NpcMeleeAttackProperties.AttackType,
                npc.StatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue);
        }
    }
}