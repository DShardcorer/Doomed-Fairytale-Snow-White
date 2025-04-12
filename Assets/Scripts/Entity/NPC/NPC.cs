using Entity.Detectors;
using Entity.NPC.Attack;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Chase;
using Entity.NPC.Idle;
using Entity.NPC.Move;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using GeneralManagers;
using Helpers;
using Item.Inventory;
using UnityEngine;

// The refactored NPC class remains mostly unchanged.
namespace Entity.NPC
{
    public class NPC : Entity, ILifecycle<NPCManager>, IUpdatable, IFixedUpdatable
    {
        // References and properties
        protected NPCManager _parent;
        protected NPCView _npcView;
        public NPCView NPCView => _npcView;
        protected NPCProperties _npcProperties;
        public NPCProperties NPCProperties => _npcProperties;
        protected FOVDetector _fovDetector;
        public FOVDetector FOVDetector => _fovDetector;
        protected ProximityDetector _proximityDetector;
        public ProximityDetector ProximityDetector => _proximityDetector;
        public bool IsBusy = false;

        #region StateDefinitions

        // Idling state
        protected NPCIdlingState _npcIdlingState;
        public NPCIdlingState NPCIdlingState => _npcIdlingState;
        protected NPCIdlingProperties _npcIdlingProperties;
        public NPCIdlingProperties NPCIdlingProperties => _npcIdlingProperties;

        // Moving state
        protected NPCMovingState _npcMovingState;
        public NPCMovingState NPCMovingState => _npcMovingState;

        // Attacking state
        protected NPCAttackingState _npcAttackingState;
        public NPCAttackingState NPCAttackingState => _npcAttackingState;

        // Chasing state
        private NPCChasingState _npcChasingState;
        public NPCChasingState NPCChasingState => _npcChasingState;

        // Being Interacted With state
        protected NPCBeingInteractedWithState _npcBeingInteractedWithState;
        public NPCBeingInteractedWithState NPCBeingInteractedWithState => _npcBeingInteractedWithState;

        #endregion

        protected NPCInteractSystem _npcInteractSystem;
        public NPCInteractSystem NPCInteractSystem => _npcInteractSystem;

        // Constructor used by the builder
        public NPC(
            NPCView view,
            NPCProperties properties,
            NPCIdlingState npcIdlingState,
            NPCMovingState npcMovingState,
            NPCChasingState npcChasingState,
            NPCAttackingState npcAttackingState,
            StatSystem statSystem,
            EquipmentSystem equipmentSystem,
            SkillSystem skillSystem,
            LevelSystem levelSystem,
            HealthSystem healthSystem,
            ManaSystem manaSystem,
            StaminaSystem staminaSystem,
            EntityStateMachine stateMachine,
            InventorySystem inventory
        ) : base(view, properties, statSystem, equipmentSystem, skillSystem, levelSystem, healthSystem, manaSystem,
            staminaSystem, stateMachine, inventory)
        {
            _npcView = view;
            _npcProperties = properties;
            _npcIdlingState = npcIdlingState;
            _npcMovingState = npcMovingState;
            _npcChasingState = npcChasingState;
            _npcAttackingState = npcAttackingState;
            // Create the "being interacted with" state with default settings.
            _npcBeingInteractedWithState = new NPCBeingInteractedWithState(
                HelperAnimationStateName.IS_IDLING,
                new NPCBeingInteractedWithProperties()
            );
        }

        public virtual void Initialize(NPCManager parent)
        {
            Debug.Log("NPC Initialize");
            _parent = parent;

            // Subscribe to update managers
            _parent.GameManager.UpdateManager.AddUpdatable(this);
            _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);

            // Initialize the view
            view.Initialize(this);

            // Setup detectors
            _fovDetector = view.GetComponentInChildren<FOVDetector>();
            _fovDetector.Initialize(this);
            _proximityDetector = view.GetComponentInChildren<ProximityDetector>();
            _proximityDetector.Initialize(this);

            _proximityDetector.OnEntityFromDifferentFactionSpottedInProximity +=
                OnEntityFromDifferentFactionSpottedInProximity;
            _fovDetector.OnClosestEntityFromDifferentFactionSpottedInFOV += OnClosestEntityFromDifferentFactionSpottedInFOV;

            // Initialize interact system
            _npcInteractSystem = view.GetComponent<NPCInteractSystem>();
            _npcInteractSystem.Initialize(this);

            // Initialize states
            _npcIdlingState.Initialize(this);
            _npcMovingState.Initialize(this);
            _npcChasingState.Initialize(this);
            _npcAttackingState.Initialize(this);
            _npcBeingInteractedWithState.Initialize(this);
            stateMachine.Initialize(_npcIdlingState);
            base.Initialize();
        }

        protected virtual void OnClosestEntityFromDifferentFactionSpottedInFOV(object sender, Entity e)
        {
            properties.target = e;
            stateMachine.ChangeState(_npcChasingState);
        }

        protected virtual void OnEntityFromDifferentFactionSpottedInProximity(object sender, Entity e)
        {
            properties.lastMovementVector = (e.View.transform.position - view.transform.position).normalized;
        }

        public void UpdateLogic()
        {
            if (IsBusy) return;
            stateMachine.UpdateLogic();
        }

        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            if (IsBusy) return;
            stateMachine.FixedUpdateLogic();
        }

        public override void Dispose()
        {
            _parent.GameManager.UpdateManager.RemoveUpdatable(this);
            _parent.GameManager.FixedUpdateManager.RemoveFixedUpdatable(this);

            _proximityDetector.OnEntityFromDifferentFactionSpottedInProximity -=
                OnEntityFromDifferentFactionSpottedInProximity;
            _fovDetector.OnClosestEntityFromDifferentFactionSpottedInFOV -= OnClosestEntityFromDifferentFactionSpottedInFOV;

            // Nullify references
            _parent = null;
            _npcView = null;
            _npcProperties = null;
            _fovDetector = null;
            _proximityDetector = null;
            _npcIdlingState = null;
            _npcMovingState = null;
            base.Dispose();
        }
    }
}