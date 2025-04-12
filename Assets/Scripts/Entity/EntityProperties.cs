using Entity.Faction;
using UnityEngine;

namespace Entity
{
    public class EntityProperties
    {
        private EntityFaction entityFaction;
        public EntityFaction EntityFaction => entityFaction;

        public Vector2 lastMovementVector = Vector2.down;
        public Entity lastAttacker;
        public Entity target;
        public Vector2 currentPosition;
        protected float moveSpeed;
        public float MoveSpeed => moveSpeed;

        public EntityProperties(EntityFaction entityFaction, float moveSpeed)
        {
            this.entityFaction = entityFaction;
            this.moveSpeed = moveSpeed;
        }

    }
}
