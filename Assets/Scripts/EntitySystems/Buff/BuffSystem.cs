using System.Collections.Generic;
using GeneralManagers;

namespace DefaultNamespace.EntitySystems.Buff
{
    public class BuffSystem: ILifecycle<Entity.Entity>
    {
        protected Entity.Entity parent;
        public Entity.Entity Parent => parent;
        
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
        
        public void Initialize(Entity.Entity parent)
        {
            this.parent = parent;
        }

        public void Dispose()
        {
            parent = null;
        }
    }
}