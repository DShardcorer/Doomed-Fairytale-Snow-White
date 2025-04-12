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
        protected EntityStateMachine stateMachine;
        protected EntityView view;
        protected EntityProperties properties;
        protected SkillSystem skillSystem;
        public SkillSystem SkillSystem => skillSystem;
        protected EquipmentSystem equipmentSystem;
        public EquipmentSystem EquipmentSystem => equipmentSystem;
        protected StatSystem _statSystem;
        public StatSystem StatSystem => _statSystem;
        protected InventorySystem _inventorySystem;
        public InventorySystem InventorySystem => _inventorySystem;
        private LevelSystem _levelSystem;
        public LevelSystem LevelSystem => _levelSystem;
        private HealthSystem _healthSystem;
        public HealthSystem HealthSystem => _healthSystem;
        private ManaSystem _manaSystem;
        public ManaSystem ManaSystem => _manaSystem;
        private StaminaSystem _staminaSystem;
        public StaminaSystem StaminaSystem => _staminaSystem;


        public EntityStateMachine StateMachine => stateMachine;
        public EntityView View => view;
        public EntityProperties Properties => properties;
        protected AttackHitbox _attackHitbox;
        public AttackHitbox AttackHitbox => _attackHitbox;
        protected AnimationTriggers _animationTriggers;

        public AnimationTriggers AnimationTriggers => _animationTriggers;


        public Entity(EntityView view, EntityProperties properties, 
            StatSystem statSystem, EquipmentSystem equipmentSystem, SkillSystem skillSystem, LevelSystem levelSystem,
            HealthSystem healthSystem, ManaSystem manaSystem, StaminaSystem staminaSystem,
            EntityStateMachine stateMachine, InventorySystem inventorySystem)
        {
            this.view = view;
            _attackHitbox = view.GetComponentInChildren<AttackHitbox>();
            _animationTriggers = view.GetComponentInChildren<AnimationTriggers>();
            this.properties = properties;
            _statSystem = statSystem;
            this.equipmentSystem = equipmentSystem;
            this.skillSystem = skillSystem;
            _levelSystem = levelSystem;
            _healthSystem = healthSystem;
            _manaSystem = manaSystem;
            _staminaSystem = staminaSystem;
            this.stateMachine = stateMachine;
            _inventorySystem = inventorySystem;
        }


        public virtual void FixedUpdateLogic()
        {
            properties.currentPosition = view.transform.position;
        }
        public virtual void Initialize()
        {
            equipmentSystem.Initialize(this);
            _inventorySystem.Initialize(this);
            _attackHitbox.Initialize(this);
            _animationTriggers.Initialize(this);
            _statSystem.Initialize(this);
            _healthSystem.Initialize(this);
            _manaSystem.Initialize(this);
            _staminaSystem.Initialize(this);
        }
        public void TakeDamage(float damage)
        {
            view.PlayDamagedAnimation();
            _healthSystem.TakeDamage((int)damage);
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
            _inventorySystem.Dispose();
            _attackHitbox.Dispose();
            _animationTriggers.Dispose();
            _statSystem.Dispose();
            _healthSystem.Dispose();
            _manaSystem.Dispose();
            _staminaSystem.Dispose();
        }
    }
}
