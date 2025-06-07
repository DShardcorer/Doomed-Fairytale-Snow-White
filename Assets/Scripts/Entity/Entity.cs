using Entity.AttackCheck;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using Item.Inventory;

namespace Entity
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


        public Entity(EntityProfile profile,
            EntityView view, EntityProperties properties,
            StatSystem statSystem, EquipmentSystem equipmentSystem,
            ActiveSkillSystem activeSkillSystem, PassiveSkillSystem passiveSkillSystem,
            LevelSystem levelSystem,
            HealthSystem healthSystem, ManaSystem manaSystem, StaminaSystem staminaSystem,
            EntityStateMachine stateMachine, InventorySystem inventorySystem)
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
        }

        public void TakeDamage(float damage)
        {
            view.PlayDamagedAnimation();
            healthSystem.TakeDamage((int)damage);
            
        }

        public virtual void Die()
        {
            if (properties.lastAttacker != null)
            {
                properties.lastAttacker.LevelSystem.AddExperience(50);
            }
        }

        public virtual void Dispose()
        {
            view.Dispose();
            equipmentSystem.Dispose();
            inventorySystem.Dispose();
            attackHitbox.Dispose();
            animationTriggers.Dispose();
            statSystem.Dispose();
            healthSystem.Dispose();
            manaSystem.Dispose();
            staminaSystem.Dispose();
        }
    }
}