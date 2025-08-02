using System.Collections.Generic;
using DefaultNamespace.EntitySystems.Buff;
using EntityBase.Faction;
using EntityBase.NPC;
using EntityBase.NPC.AI;
using EntityBase.NPC.StandardAI;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using EntitySystems.WeaponSystem;
using Helpers;
using Item.Inventory;
using UnityEngine;

namespace EntityBase.NPC_Variants.Native
{
    public class TestNPCManager : NPCManager
    {
        [SerializeField] private GameObject _nativePrefab;
        [SerializeField] private AbilityStatboardSO _abilityStatboardSO;
        private List<NPC.NPC> _enemies = new List<NPC.NPC>();


        public void SpawnMeleeNative(Vector3 position)
        {
            EntityProfile entityProfile = new EntityProfile("Native", "A native NPC");
            // // EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);
            NativeView nativeView = Instantiate(_nativePrefab).GetComponent<NativeView>();
            nativeView.transform.position = position;

            ActiveSkillSystem activeSkillSystem = new ActiveSkillSystem(new List<ActiveSkill> { });
            PassiveSkillSystem passiveSkillSystem = new PassiveSkillSystem(new List<PassiveSkill> { });

            EntityStateMachine stateMachine = new EntityStateMachine();

            //Stat system creation
            AbilityStatBoard abilityStatBoard = new AbilityStatBoard(_abilityStatboardSO);
            StatSystem statSystem = new StatSystem(abilityStatBoard, AttackStatType.Strength);

            //Equipment system creation
            EquipmentSystem equipmentSystem = new EquipmentSystem();


            NPCProperties npcProperties = new NPCProperties(EntityFaction.Native
                , 2);

            //LevelSystem creation
            LevelSystem levelSystem = new LevelSystem();

            //HealthSystem creation (convert health to int)
            HealthSystem healthSystem = new HealthSystem((int)statSystem.CombatStatBoard.Health.ModifiedValue);

            //ManaSystem creation
            ManaSystem manaSystem = new ManaSystem((int)statSystem.CombatStatBoard.Mana.ModifiedValue);

            //StaminaSystem creation
            StaminaSystem staminaSystem = new StaminaSystem((int)statSystem.CombatStatBoard.Stamina.ModifiedValue);
            InventorySystem inventory = new InventorySystem();
            NPCAIConfiguration config =
                UnityEngine.Resources.Load<NPCAIConfiguration>(HelperResourcePath.NPCAIConfigPath +
                                                               "StandardNPCMeleeAIConfig");
            
            NPCAIController enhancedMeleeAIController =
                new PatrolNormalNPCAIController(config);

            BuffSystem buffSystem = new BuffSystem();
            WeaponSystem weaponSystem = new WeaponSystem();
            NPCStateSystem stateSystem = new NPCStateSystem(config);
            NPC.NPC npc = new NPC.NPC(
                entityProfile,
                nativeView,
                npcProperties,
                statSystem,
                equipmentSystem,
                activeSkillSystem, passiveSkillSystem,
                levelSystem,
                healthSystem, manaSystem, staminaSystem,
                stateMachine,
                inventory, buffSystem,
                weaponSystem,
                enhancedMeleeAIController,
                stateSystem);
            npc.Initialize();
            // npc.NPCView.transform.position = position;
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