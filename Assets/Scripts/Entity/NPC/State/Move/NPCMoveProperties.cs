using System;
using Entity.NPC.AI;

namespace Entity.NPC.State.Move
{
    [Serializable]
    public class NPCMoveProperties: EntityStateProperties
    {
        private float moveSpeed = 2.0f;
        public float MoveSpeed => moveSpeed;
        

        public NPCMoveProperties(NPCAIConfiguration config)
        {
            this.moveSpeed = config.moveSpeed;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        
        }
    }
}
