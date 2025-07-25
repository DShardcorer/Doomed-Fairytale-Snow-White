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
using Pathfinding;
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
        private IAstarAI _astarAI;
        public IAstarAI AstarAI => _astarAI;

        protected NPCInteractSystem _npcInteractSystem;
        public NPCInteractSystem NPCInteractSystem => _npcInteractSystem;

        private BehaviourTree _behaviourTree;
        public bool UseBehaviourTree = true;

        //Test bools
        private bool inDanger = true;
        private bool treasure1Present = true;
        private bool treasure2Present = true;

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
            _astarAI = view.GetComponent<IAstarAI>();
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
            _behaviourTree = new BehaviourTree("NPC Behavior Tree");

            PrioritySelector prioritySelector = new PrioritySelector("Agent Logic");
            _behaviourTree.AddChild(prioritySelector);


            //Run to safety subtree
            Sequence runToSafetySequence = new Sequence("Run to Safety", 100);
            prioritySelector.AddChild(runToSafetySequence);
            Vector3 safetyPosition = new Vector3(-15, 0, 0); // Example safety position

            bool IsInDanger()
            {
                if (!inDanger)
                {
                    runToSafetySequence.Reset();
                    return false;
                }

                return true;
            }

            runToSafetySequence.AddChild(new Leaf("Is In Danger?", new Condition(IsInDanger)));
            runToSafetySequence.AddChild(new Leaf("Run to Safety", new ActionStrategy(() =>
                {
                    //Placeholder code. Should call MoveToPosition from the ai to change state in the FSM. 
                    _astarAI.destination = safetyPosition;
                    _astarAI.canMove = true;
                    Debug.Log("Run to Safety");
                }
            )));

            //Collect treasures subtree
            Selector goToTreasureSelector = new RandomSelector("Get Treasures", 50);
            prioritySelector.AddChild(goToTreasureSelector);
            Sequence getTreasure1Sequence = new Sequence("Get Treasure 1");
            goToTreasureSelector.AddChild(getTreasure1Sequence);
            getTreasure1Sequence.AddChild(new Leaf("Is Treasure 1 Present", new Condition(() => true)));
            getTreasure1Sequence.AddChild(new Leaf("Move to Treasure 1",
                new ActionStrategy(() =>
                {
                    Vector3 treasure1Position = new Vector3(11, 11, 0); // Example treasure position
                    _astarAI.destination = treasure1Position;
                    _astarAI.canMove = true;
                    Debug.Log("Move to Treasure 1");
                })));
            getTreasure1Sequence.AddChild(new Leaf("Collect Treasure 1",
                new ActionStrategy(() =>
                {
                    // Placeholder for treasure collection logic
                    treasure1Present = false; // Simulate collection
                    Debug.Log("Collected Treasure 1");
                })));



            Sequence getTreasure2Sequence = new Sequence("Get Treasure 2");
            goToTreasureSelector.AddChild(getTreasure2Sequence);
            getTreasure2Sequence.AddChild(new Leaf("Is Treasure 2 Present", new Condition(() => true)));
            getTreasure2Sequence.AddChild(new Leaf("Move to Treasure 2",
                new ActionStrategy(() =>
                {
                    Vector3 treasure2Position = new Vector3(15, -5, 0); // Example treasure position
                    _astarAI.destination = treasure2Position;
                    _astarAI.canMove = true;
                })));
            getTreasure2Sequence.AddChild(new Leaf("Collect Treasure 2",
                new ActionStrategy(() =>
                {
                    // Placeholder for treasure collection logic
                    treasure2Present = false; // Simulate collection
                    Debug.Log("Collected Treasure 2");
                })));



            //Patrol subtree
            
            // Create patrol points (example)
            List<Vector3> patrolPoints = new List<Vector3>
            {
                view.transform.position + new Vector3(5, 0, 0),
                view.transform.position + new Vector3(0, 5, 0),
                view.transform.position + new Vector3(-5, 0, 0),
                view.transform.position + new Vector3(0, 0, 0)
            };

            Leaf patrolNode = new Leaf("Patrol", new PatrolStrategy(this, patrolPoints, 2f), 0);
            prioritySelector.AddChild(patrolNode);
            
            
            
            
        }


        public void UpdateLogic()
        {
            Testing();
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

        private void Testing()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                inDanger = !inDanger;
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