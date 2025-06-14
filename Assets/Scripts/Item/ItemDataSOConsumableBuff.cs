using DefaultNamespace.EntitySystems.Buff;
using EntitySystems.Stats;
using UnityEngine;

namespace Item
{
    public class ItemDataSOConsumableBuff: ItemDataSOConsumable
    {
        public float buffDuration = 10f;
        public StatModifier statModifier;
        public override void UseItem(Entity.Entity entity)
        {
            // Create a new buff instance
            var buff = new Buff(statModifier, buffDuration);
            
            // Add the buff to the entity's BuffSystem
            if (entity.BuffSystem != null)
            {
                entity.BuffSystem.AddBuff(buff);
            }
            else
            {
                Debug.LogError("Entity does not have a BuffSystem!");
            }
        }
    }
}