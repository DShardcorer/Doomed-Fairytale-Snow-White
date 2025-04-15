
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.Skill
{
    public abstract class ActiveSkill : ILifecycle<ActiveSkillSystem>, IUpdatable, IFixedUpdatable
    {
        protected ActiveSkillSystem parent;
        public ActiveSkillSystem Parent => parent;

        protected HealthSystem healthSystem;
        public HealthSystem HealthSystem => healthSystem;
        protected ManaSystem manaSystem;
        public ManaSystem ManaSystem => manaSystem;
        protected StaminaSystem staminaSystem;
        public StaminaSystem StaminaSystem => staminaSystem;

        protected string skillName;
        protected float cooldown;
        protected float cooldownTimer;
        protected bool isMindBound = false;
        
        public bool IsMindBound => isMindBound;
        public string SkillName => skillName;
        public float Cooldown => cooldown;
        public float CooldownTimer => cooldownTimer;


        protected float _healthCost;
        public float HealthCost => _healthCost;
        protected float _manaCost;
        public float ManaCost => _manaCost;
        protected float _staminaCost;
        public float StaminaCost => _staminaCost;


        public ActiveSkill(string skillName, float cooldown, float healthCost = 0, float manaCost = 0, float staminaCost = 30)
        {
            this.skillName = skillName;
            this.cooldown = cooldown;
            _healthCost = healthCost;
            _manaCost = manaCost;
            _staminaCost = staminaCost;
        }
        public virtual void Initialize(ActiveSkillSystem parent)
        {

            this.parent = parent;
            GameManager.Instance.FixedUpdateManager.AddFixedUpdatable(this);
            GameManager.Instance.UpdateManager.AddUpdatable(this);
            cooldownTimer = 0;
            healthSystem = this.parent.Parent.HealthSystem;
            manaSystem = this.parent.Parent.ManaSystem;
            staminaSystem = this.parent.Parent.StaminaSystem;

        }

        public void Dispose()
        {
            parent = null;
            GameManager.Instance.FixedUpdateManager.RemoveFixedUpdatable(this);
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
        }
        public virtual void UpdateLogic()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
        }

        public virtual void FixedUpdateLogic()
        {
       
        }


        public virtual bool CanUseSkill()
        {
            if (cooldownTimer > 0)
            {
                return false;
            }
            if (healthSystem.CurrentHealth < _healthCost)
            {
                return false;
            }
            if (manaSystem.CurrentMana < _manaCost)
            {
                return false;
            }
            if (staminaSystem.CurrentStamina < _staminaCost)
            {
                return false;
            }
            return true;
        }

        public virtual bool TryUseSkill()
        {
            if (CanUseSkill())
            {
                UseSkill();
                return true;
            }
            return false;
        }

        protected virtual void UseSkill()
        {
            cooldownTimer = cooldown;
            healthSystem.TakeDamage(_healthCost);
            manaSystem.TryUseMana(_manaCost);
            staminaSystem.TryUseStamina(_staminaCost);
        }





    }
}
