using System;
using Entity.NPC.AI;

namespace Entity.NPC.Move
{
    [Serializable]
    public class NPCMovingProperties: EntityStateProperties
    {
        private float moveSpeed = 2.0f;
        public float MoveSpeed => moveSpeed;

        private float movingTime = 2.0f;
        public float MovingTime => movingTime;

        public NPCMovingProperties(NPCAIConfiguration config)
        {
            this.moveSpeed = config.moveSpeed;
            this.movingTime = config.movingTime;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        
        }
    }
}
