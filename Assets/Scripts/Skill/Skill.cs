
using UnityEngine;

public abstract class Skill : ILifecycle<SkillSystem>, IUpdatable, IFixedUpdatable
{
    protected SkillSystem _parent;
    public SkillSystem Parent => _parent;

    protected string _skillName;
    protected float _cooldown;
    protected float _cooldownTimer;

    public string SkillName => _skillName;
    public float Cooldown => _cooldown;
    public float CooldownTimer => _cooldownTimer;
    public Skill(string skillName, float cooldown)
    {
        _skillName = skillName;
        _cooldown = cooldown;
    }
    public virtual void Initialize(SkillSystem parent)
    {

        _parent = parent;
        GameManager.Instance.FixedUpdateManager.AddFixedUpdatable(this);
        GameManager.Instance.UpdateManager.AddUpdatable(this);
        _cooldownTimer = 0;

    }

    public void Dispose()
    {
        _parent = null;
        GameManager.Instance.FixedUpdateManager.RemoveFixedUpdatable(this);
        GameManager.Instance.UpdateManager.RemoveUpdatable(this);
    }
    public virtual void UpdateLogic()
    {
        Debug.Log(_cooldownTimer);
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
        return _cooldownTimer <= 0;
    }

    public virtual bool TryUseSkill()
    {
        Debug.Log("Cooldown timer: " + _cooldownTimer);
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
    }





}
