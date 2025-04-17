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

        protected float cooldownTimer;


        public ActiveSkillInfoSO activeSkillInfo;


        public ActiveSkill(ActiveSkillInfoSO activeSkillInfoSO)
        {
            this.activeSkillInfo = activeSkillInfoSO;
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

            if (healthSystem.CurrentHealth < activeSkillInfo.healthCost)
            {
                return false;
            }

            if (manaSystem.CurrentMana < activeSkillInfo.manaCost)
            {
                return false;
            }

            if (staminaSystem.CurrentStamina < activeSkillInfo.staminaCost)
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
            cooldownTimer = activeSkillInfo.cooldown;
            healthSystem.TakeDamage(activeSkillInfo.healthCost);
            manaSystem.TryUseMana(activeSkillInfo.manaCost);
            staminaSystem.TryUseStamina(activeSkillInfo.staminaCost);
        }
    }
}