using Entity.Detectors;
using Entity.NPC.AI;
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
        protected NPCAIController _npcAIController;
        public NPCAIController NPCAIController => _npcAIController;
        protected FOVDetector _fovDetector;
        public FOVDetector FOVDetector => _fovDetector;
        protected ProximityDetector _proximityDetector;
        public ProximityDetector ProximityDetector => _proximityDetector;


        protected NPCInteractSystem _npcInteractSystem;
        public NPCInteractSystem NPCInteractSystem => _npcInteractSystem;

        // Constructor used by the builder
        public NPC(
            NPCView view,
            NPCProperties properties,
            StatSystem statSystem,
            EquipmentSystem equipmentSystem,
            ActiveSkillSystem activeSkillSystem,
            PassiveSkillSystem passiveSkillSystem,
            LevelSystem levelSystem,
            HealthSystem healthSystem,
            ManaSystem manaSystem,
            StaminaSystem staminaSystem,
            EntityStateMachine stateMachine,
            InventorySystem inventory,
            NPCAIController aiController
        ) : base(view,
            properties,
            statSystem,
            equipmentSystem,
            activeSkillSystem, passiveSkillSystem,
            levelSystem, healthSystem,
            manaSystem,
            staminaSystem, stateMachine, inventory)
        {
            _npcView = view;
            _npcProperties = properties;
            _npcAIController = aiController;
            IdleState = aiController.GetState(HelperNPCStateName.Idle);
        }
        public virtual void Initialize(NPCManager parent)
        {
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

            // Initialize interact system
            _npcInteractSystem = view.GetComponent<NPCInteractSystem>();
            _npcInteractSystem.Initialize(this);

            activeSkillSystem.Initialize(this);
            passiveSkillSystem.Initialize(this);

            _npcAIController.Initialize(this);
            base.Initialize();
        }

        public void UpdateLogic()
        {
            if (IsBusy) return;
            _npcAIController.UpdateLogic();
        }

        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            if (IsBusy) return;
            _npcAIController.FixedUpdateLogic();
        }

        public override void Dispose()
        {
            _parent.GameManager.UpdateManager.RemoveUpdatable(this);
            _parent.GameManager.FixedUpdateManager.RemoveFixedUpdatable(this);
            // Nullify references
            _parent = null;
            _npcView = null;
            _npcProperties = null;
            _fovDetector = null;
            _proximityDetector = null;

            base.Dispose();
        }
    }
}