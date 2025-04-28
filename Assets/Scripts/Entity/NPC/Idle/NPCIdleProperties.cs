using System;
using Entity.NPC.AI;

namespace Entity.NPC.Idle
{
    public class NPCIdleProperties: EntityStateProperties
    {
        private float idleTime = 2.0f;
        public float IdleTime => idleTime;

        public NPCIdleProperties(NPCAIConfiguration config)
        {
            idleTime = config.idleTime;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        }
    }
}
