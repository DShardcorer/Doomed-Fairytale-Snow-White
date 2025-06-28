using EntityBase.Faction;
using UnityEngine;

namespace EntityBase
{
    public class EntityProperties
    {
        private EntityFaction entityFaction;
        public EntityFaction EntityFaction => entityFaction;

        public Vector2 lastMovementVector = Vector2.down;
        private Vector2 _cardinalizedLastMovementVector = Vector2.down;
        public Vector2 CardinalizedLastMovementVector()
        {
            _cardinalizedLastMovementVector.Set(
                Mathf.Round(lastMovementVector.x),
                Mathf.Round(lastMovementVector.y)
            );
            return _cardinalizedLastMovementVector;
        }

        public Entity lastAttacker;
        public Entity target;
        public Vector2 currentPosition;
        protected float moveSpeed;
        public float MoveSpeed => moveSpeed;
        private int experienceDrop = 50;
        public int ExperienceDrop => experienceDrop;
        public EntityProperties(EntityFaction entityFaction, float moveSpeed)
        {
            this.entityFaction = entityFaction;
            this.moveSpeed = moveSpeed;
        }

    }
}
