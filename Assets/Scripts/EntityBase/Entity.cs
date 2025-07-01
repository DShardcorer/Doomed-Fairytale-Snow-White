using System;
using DefaultNamespace.EntitySystems.Buff;
using EntityBase.AttackCheck;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using EntitySystems.WeaponSystem;
using Item.Inventory;
using UnityEngine;

namespace EntityBase
{
    public abstract class Entity
    {
        protected EntityProfile profile;
        public EntityProfile Profile => profile;
        protected EntityStateMachine stateMachine;
        protected EntityView view;
        protected EntityProperties properties;
        protected ActiveSkillSystem activeSkillSystem;
        public ActiveSkillSystem ActiveSkillSystem => activeSkillSystem;
        protected PassiveSkillSystem passiveSkillSystem;
        public PassiveSkillSystem PassiveSkillSystem => passiveSkillSystem;
        protected EquipmentSystem equipmentSystem;
        public EquipmentSystem EquipmentSystem => equipmentSystem;
        protected StatSystem statSystem;
        public StatSystem StatSystem => statSystem;
        protected InventorySystem inventorySystem;
        public InventorySystem InventorySystem => inventorySystem;
        private LevelSystem levelSystem;
        public LevelSystem LevelSystem => levelSystem;
        private HealthSystem healthSystem;
        public HealthSystem HealthSystem => healthSystem;
        private ManaSystem manaSystem;
        public ManaSystem ManaSystem => manaSystem;
        private StaminaSystem staminaSystem;
        public StaminaSystem StaminaSystem => staminaSystem;
        protected BuffSystem buffSystem;
        public BuffSystem BuffSystem => buffSystem;
        protected WeaponSystem weaponSystem;
        public WeaponSystem WeaponSystem => weaponSystem;

        public EntityStateMachine StateMachine => stateMachine;
        public EntityView View => view;
        public EntityProperties Properties => properties;
        protected AttackHitbox attackHitbox;
        public AttackHitbox AttackHitbox => attackHitbox;
        protected AnimationTriggers animationTriggers;

        public AnimationTriggers AnimationTriggers => animationTriggers;

        public bool IsBusy = false;

        // Default states that any entity should have
        public EntityState IdleState { get; protected set; }

        // Abstract methods for weapon convenience
        public abstract bool IsAttacking();
        public abstract int CurrentAttackCounter();

        public Entity(EntityProfile profile,
            EntityView view, EntityProperties properties,
            StatSystem statSystem, EquipmentSystem equipmentSystem,
            ActiveSkillSystem activeSkillSystem, PassiveSkillSystem passiveSkillSystem,
            LevelSystem levelSystem,
            HealthSystem healthSystem, ManaSystem manaSystem, StaminaSystem staminaSystem,
            EntityStateMachine stateMachine, InventorySystem inventorySystem, BuffSystem buffSystem,
            WeaponSystem weaponSystem)
        {
            this.profile = profile;
            this.view = view;
            attackHitbox = view.GetComponentInChildren<AttackHitbox>();
            animationTriggers = view.GetComponentInChildren<AnimationTriggers>();
            this.properties = properties;
            this.statSystem = statSystem;
            this.equipmentSystem = equipmentSystem;
            this.activeSkillSystem = activeSkillSystem;
            this.passiveSkillSystem = passiveSkillSystem;
            this.levelSystem = levelSystem;
            this.healthSystem = healthSystem;
            this.manaSystem = manaSystem;
            this.staminaSystem = staminaSystem;
            this.stateMachine = stateMachine;
            this.inventorySystem = inventorySystem;
            this.buffSystem = buffSystem;
            this.weaponSystem = weaponSystem;
        }


        public virtual void FixedUpdateLogic()
        {
            properties.currentPosition = view.transform.position;
        }

        public virtual void Initialize()
        {
            equipmentSystem.Initialize(this);
            inventorySystem.Initialize(this);
            attackHitbox.Initialize(this);
            animationTriggers.Initialize(this);
            statSystem.Initialize(this);
            passiveSkillSystem.Initialize(this);
            healthSystem.Initialize(this);
            manaSystem.Initialize(this);
            staminaSystem.Initialize(this);
            weaponSystem.Initialize(this);
        }

        public virtual void TakeDamage(float damage, Entity damageSource)
        {
            view.PlayDamagedAnimation();
            healthSystem.TakeDamage((int)damage);
        }

        public virtual void Die()
        {
            if (properties.lastAttacker != null)
            {
                properties.lastAttacker.LevelSystem.AddExperience(properties.ExperienceDrop);
                if (properties.lastAttacker is NPC.NPC npc)
                {
                    npc.NPCProperties.target = null; // Clear target on death
                }
            }

            inventorySystem.DropAllItemsOnTheGround();
            view.PlayDeathAnimation();
            Dispose();
        }

        public virtual void Dispose()
        {
            try
            {
                // Dispose systems with null checks
                if (view != null)
                {
                    view.Dispose();
                    view = null;
                }

                if (equipmentSystem != null)
                {
                    equipmentSystem.Dispose();
                    equipmentSystem = null;
                }

                if (inventorySystem != null)
                {
                    inventorySystem.Dispose();
                    inventorySystem = null;
                }

                if (attackHitbox != null)
                {
                    attackHitbox.Dispose();
                    attackHitbox = null;
                }

                if (animationTriggers != null)
                {
                    animationTriggers.Dispose();
                    animationTriggers = null;
                }

                if (statSystem != null)
                {
                    statSystem.Dispose();
                    statSystem = null;
                }

                if (healthSystem != null)
                {
                    healthSystem.Dispose();
                    healthSystem = null;
                }

                if (manaSystem != null)
                {
                    manaSystem.Dispose();
                    manaSystem = null;
                }

                if (staminaSystem != null)
                {
                    staminaSystem.Dispose();
                    staminaSystem = null;
                }

                if (activeSkillSystem != null)
                {
                    activeSkillSystem.Dispose();
                    activeSkillSystem = null;
                }

                if (passiveSkillSystem != null)
                {
                    passiveSkillSystem.Dispose();
                    passiveSkillSystem = null;
                }

                if (buffSystem != null)
                {
                    buffSystem.Dispose();
                    buffSystem = null;
                }
                if (weaponSystem != null)
                {
                    weaponSystem.Dispose();
                    weaponSystem = null;
                }

                if (stateMachine != null)
                {
                    stateMachine.Dispose();
                    stateMachine = null;
                }

                // Clean up remaining references
                properties = null;
                profile = null;
                levelSystem = null;
                IsBusy = false;

                Debug.LogWarning("Entity disposed: " + this.GetType().Name);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error in Entity.Dispose(): {ex.Message}");
            }
        }
    }
}