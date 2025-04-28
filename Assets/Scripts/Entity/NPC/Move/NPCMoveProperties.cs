using System;
using Entity.NPC.AI;

namespace Entity.NPC.Move
{
    [Serializable]
    public class NPCMoveProperties: EntityStateProperties
    {
        private float moveSpeed = 2.0f;
        public float MoveSpeed => moveSpeed;

        private float movingTime = 2.0f;
        public float MovingTime => movingTime;

        public NPCMoveProperties(NPCAIConfiguration config)
        {
            this.moveSpeed = config.moveSpeed;
            this.movingTime = config.movingTime;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        
        }
    }
}
