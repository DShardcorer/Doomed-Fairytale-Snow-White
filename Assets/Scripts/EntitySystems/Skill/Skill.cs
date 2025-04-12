
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.Skill
{
    public abstract class Skill : ILifecycle<SkillSystem>, IUpdatable, IFixedUpdatable
    {
        protected SkillSystem _parent;
        public SkillSystem Parent => _parent;

        protected HealthSystem _healthSystem;
        public HealthSystem HealthSystem => _healthSystem;
        protected ManaSystem _manaSystem;
        public ManaSystem ManaSystem => _manaSystem;
        protected StaminaSystem _staminaSystem;
        public StaminaSystem StaminaSystem => _staminaSystem;

        protected string _skillName;
        protected float _cooldown;
        protected float _cooldownTimer;

        public string SkillName => _skillName;
        public float Cooldown => _cooldown;
        public float CooldownTimer => _cooldownTimer;


        protected float _healthCost;
        public float HealthCost => _healthCost;
        protected float _manaCost;
        public float ManaCost => _manaCost;
        protected float _staminaCost;
        public float StaminaCost => _staminaCost;


        public Skill(string skillName, float cooldown, float healthCost = 0, float manaCost = 0, float staminaCost = 30)
        {
            _skillName = skillName;
            _cooldown = cooldown;
            _healthCost = healthCost;
            _manaCost = manaCost;
            _staminaCost = staminaCost;
        }
        public virtual void Initialize(SkillSystem parent)
        {

            _parent = parent;
            GameManager.Instance.FixedUpdateManager.AddFixedUpdatable(this);
            GameManager.Instance.UpdateManager.AddUpdatable(this);
            _cooldownTimer = 0;
            _healthSystem = _parent.Parent.HealthSystem;
            _manaSystem = _parent.Parent.ManaSystem;
            _staminaSystem = _parent.Parent.StaminaSystem;

        }

        public void Dispose()
        {
            _parent = null;
            GameManager.Instance.FixedUpdateManager.RemoveFixedUpdatable(this);
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
        }
        public virtual void UpdateLogic()
        {
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
        }

        public virtual void FixedUpdateLogic()
        {
       
        }


        public virtual bool CanUseSkill()
        {
            if (_cooldownTimer > 0)
            {
                return false;
            }
            if (_healthSystem.CurrentHealth < _healthCost)
            {
                return false;
            }
            if (_manaSystem.CurrentMana < _manaCost)
            {
                return false;
            }
            if (_staminaSystem.CurrentStamina < _staminaCost)
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
            _cooldownTimer = _cooldown;
            _healthSystem.TakeDamage(_healthCost);
            _manaSystem.TryUseMana(_manaCost);
            _staminaSystem.TryUseStamina(_staminaCost);
        }





    }
}
