using System.Collections.Generic;
using GeneralManagers;

namespace DefaultNamespace.EntitySystems.Buff
{
    public class BuffSystem: ILifecycle<EntityBase.Entity>
    {
        protected EntityBase.Entity parent;
        public EntityBase.Entity Parent => parent;
        
        protected List<Buff> buffs = new List<Buff>();
        public IReadOnlyList<Buff> Buffs => buffs;
        
        public BuffSystem()
        {
            // Constructor logic if needed
        }
        public void AddBuff(Buff buff)
        {
            buffs.Add(buff);
            buff.Initialize(this);
        }
        public void RemoveBuff(Buff buff)
        {
            if (buffs.Contains(buff))
            {
                buffs.Remove(buff);
                buff.Dispose();
            }
        }
        
        public void Initialize(EntityBase.Entity parent)
        {
            this.parent = parent;
        }

        public void Dispose()
        {
            parent = null;
        }
    }
}