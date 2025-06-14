using EntitySystems.Stats;
using GeneralManagers;
using UnityEngine;

namespace DefaultNamespace.EntitySystems.Buff
{
    public class Buff: ILifecycle<BuffSystem>, IUpdatable
    {
        private BuffSystem _parent;
        public BuffSystem Parent => _parent;
        public StatModifier StatModifier { get; private set; }
        public float Duration { get; private set; }
        protected float _remainingDuration;
        
        public Buff(StatModifier statModifier, float duration)
        {
            StatModifier = statModifier;
            Duration = duration;
        }

        public void Initialize(BuffSystem parent)
        {
            _parent = parent;
            GameManager.Instance.UpdateManager.AddUpdatable(this);
        }

        public void Dispose()
        {
            _parent = null;
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
        }

        public void UpdateLogic()
        {
            if (_remainingDuration > 0)
            {
                _remainingDuration -= Time.deltaTime;
                if (_remainingDuration <= 0)
                {
                    _parent.RemoveBuff(this);
                }
            }
        }
        
    }
}