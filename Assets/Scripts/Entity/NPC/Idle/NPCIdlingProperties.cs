using System;

namespace Entity.NPC.Idle
{
    public class NPCIdlingProperties: EntityStateProperties
    {
        private float idleTime = 2.0f;
        public float IdleTime => idleTime;

        public NPCIdlingProperties(float idleTime)
        {
            this.idleTime = idleTime;
        }

        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        }
    }
}
