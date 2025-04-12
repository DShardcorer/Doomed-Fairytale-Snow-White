using System.Collections.Generic;
using Entity.Faction;
using Entity.NPC_Variants.Native.Attack;
using Entity.NPC_Variants.Native.Chase;
using Entity.NPC_Variants.Native.Idle;
using Entity.NPC_Variants.Native.Move;
using Entity.NPC;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using Helpers;
using Item.Inventory;
using UnityEngine;

namespace Entity.NPC_Variants.Native
{
    public class NativeManager : NPCManager
    {
        [SerializeField] private GameObject _nativePrefab;
        [SerializeField] private AbilityStatboardSO _abilityStatboardSO;
        private List<NPC.NPC> _enemies = new List<NPC.NPC>();


        public void SpawnMeleeNative(Vector3 position)
        {
            // // EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);
            NativeView nativeView = _poolManager.GetObject(_nativePrefab.name).GetComponent<NativeView>();

            SkillSystem skillSystem = new SkillSystem(new List<Skill> { });

            //State creation
            NativeIdlingProperties nativeIdlingProperties = new NativeIdlingProperties(2);
            NativeIdlingState nativeIdlingState = new NativeIdlingState(HelperAnimationStateName.IS_IDLING, nativeIdlingProperties);

            NativeMovingProperties nativeMovingProperties = new NativeMovingProperties(2);
            NativeMovingState nativeMovingState = new NativeMovingState(nativeMovingProperties, HelperAnimationStateName.IS_MOVING);

            NativeChasingProperties nativeChasingProperties = new NativeChasingProperties();
            NativeMeleeChasingState nativeChasingState = new NativeMeleeChasingState(nativeChasingProperties, HelperAnimationStateName.IS_CHASING);

            NativeAttackingProperties nativeAttackingProperties = new NativeAttackingProperties();
            NativeMeleeAttackingState nativeAttackingState = new NativeMeleeAttackingState(HelperAnimationStateName.IS_ATTACKING, nativeAttackingProperties);


            EntityStateMachine stateMachine = new EntityStateMachine();

            //Stat system creation
            AbilityStatBoard abilityStatBoard = new AbilityStatBoard(_abilityStatboardSO);
            StatSystem statSystem = new StatSystem(abilityStatBoard, AttackStatType.Strength);

            //Equipment system creation
            EquipmentSystem equipmentSystem = new EquipmentSystem();


            NPCProperties npcProperties = new NPCProperties(EntityFaction.Native, new List<EntityFaction> { EntityFaction.Civilized }, 2, 10);

            //LevelSystem creation
            LevelSystem levelSystem = new LevelSystem();

            //HealthSystem creation (convert health to int)
            HealthSystem healthSystem = new HealthSystem((int)statSystem.CombatStatBoard.Health.ModifiedValue);

            //ManaSystem creation
            ManaSystem manaSystem = new ManaSystem((int)statSystem.CombatStatBoard.Mana.ModifiedValue);

            //StaminaSystem creation
            StaminaSystem staminaSystem = new StaminaSystem((int)statSystem.CombatStatBoard.Stamina.ModifiedValue);
            InventorySystem inventory = new InventorySystem();

            NPC.NPC npc = new NPC.NPC(nativeView, npcProperties, nativeIdlingState, nativeMovingState, nativeChasingState, nativeAttackingState,
                statSystem, equipmentSystem,
                skillSystem, levelSystem, healthSystem, manaSystem, staminaSystem, stateMachine, inventory);

            npc.Initialize(this);

            npc.NPCView.transform.position = position;
            _enemies.Add(npc);
        }

        public void DespawnMeleeNative(NPC.NPC npc)
        {
            _poolManager.ReturnObject(_nativePrefab.name, npc.NPCView.gameObject);
            npc.Dispose();
            _enemies.Remove(npc);
        }
        public void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.N))
            {
                SpawnMeleeNative(new Vector3(0, 0, 0));
            }
        }


        public override void Dispose()
        {
            base.Dispose();
            _enemies.Clear();
        }



    }
}
