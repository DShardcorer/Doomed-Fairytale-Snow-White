using System.Collections.Generic;
using DefaultNamespace.EntitySystems.Buff;
using EntityBase.Detectors;
using EntityBase.Faction;
using EntityBase.NPC.AI;
using EntityBase.NPC.BehaviourTrees;
using EntityBase.NPC.BehaviourTrees.Strategies;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using EntitySystems.WeaponSystem;
using GeneralManagers;
using Helpers;
using Item.Inventory;
using UnityEngine;

// The refactored NPC class remains mostly unchanged.
namespace EntityBase.NPC
{
    public class NPC : Entity, IUpdatable, IFixedUpdatable
    {
        // References and properties
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
        
        private BehaviourTree _behaviourTree;
        public bool UseBehaviourTree = true;

        // Constructor used by the builder
        public NPC(
            EntityProfile profile,
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
            BuffSystem buffSystem,
            WeaponSystem weaponSystem,
            NPCAIController aiController
        ) : base(
            profile,
            view,
            properties,
            statSystem,
            equipmentSystem,
            activeSkillSystem, passiveSkillSystem,
            levelSystem, healthSystem,
            manaSystem,
            staminaSystem, stateMachine, 
            inventory, buffSystem, weaponSystem)
        {
            _npcView = view;
            _npcProperties = properties;
            _npcAIController = aiController;
            IdleState = aiController.GetState(HelperNPCStateName.Idle);
        }

        public override void Initialize()
        {
            // Subscribe to update managers
            GameManager.Instance.UpdateManager.AddUpdatable(this);
            GameManager.Instance.FixedUpdateManager.AddFixedUpdatable(this);

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

            if (UseBehaviourTree)
            {
                InitializeBehaviourTree();
            }
        }
        private void InitializeBehaviourTree()
        {
            // Create a simple behavior tree
            _behaviourTree = new BehaviourTree("NPC Behavior Tree");
            
            // Create patrol points (example)
            List<Vector3> patrolPoints = new List<Vector3>
            {
                view.transform.position + new Vector3(5, 0, 0),
                view.transform.position + new Vector3(0, 0, 5),
                view.transform.position + new Vector3(-5, 0, 0),
                view.transform.position + new Vector3(0, 0, -5)
            };
            
            // Create and add patrol strategy
            PatrolStrategy patrolStrategy = new PatrolStrategy(this, patrolPoints, 2f);
            Leaf patrolNode = new Leaf("Patrol", patrolStrategy);
            
            // Add patrol node to behavior tree
            _behaviourTree.AddChild(patrolNode);
        }
        


        public void UpdateLogic()
        {
            if (IsBusy) return;
            
            if (UseBehaviourTree)
            {
                // Use behavior tree approach
                _behaviourTree?.Process();
            }
            else
            {
                // Use existing FSM approach
                _npcAIController.UpdateLogic();
            }
        }

        public override void FixedUpdateLogic()
        {
            if (IsBusy) return;
            base.FixedUpdateLogic();
            
            if (!UseBehaviourTree)
            {
                _npcAIController.FixedUpdateLogic();
            }
        }
        public override bool IsAttacking()
        {
            return HelperNPCStateName.Attack == _npcAIController.CurrentStateId;
        }

        public override int CurrentAttackCounter()
        {
            return 0;
        }
        

        public override void TakeDamage(float damage, Entity damageSource)
        {
            base.TakeDamage(damage, damageSource);
            if (damageSource != null)
            {
                EntityFaction damageSourceFaction = damageSource.Properties.EntityFaction;
                if (FactionRegistry.AreAllies(Properties.EntityFaction, damageSourceFaction))
                {
                    GameManager.Instance.FactionManager.AddTemporaryEnemy(Properties.EntityFaction, damageSourceFaction,
                        10);
                }
            }
        }

        public override void Dispose()
        {
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
            GameManager.Instance.FixedUpdateManager.RemoveFixedUpdatable(this);

            // These dispose calls should happen before nulling references
            _npcAIController?.Dispose();
            _npcInteractSystem?.Dispose();

            // Set each reference to null only once
            _npcView = null;
            _npcProperties = null;
            _fovDetector = null;
            _proximityDetector = null;
            _npcAIController = null;
            _npcInteractSystem = null;

            base.Dispose();
        }
    }
}