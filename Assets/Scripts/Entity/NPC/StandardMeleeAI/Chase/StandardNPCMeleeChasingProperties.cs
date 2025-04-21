using System;
using Entity.NPC.AI;

namespace Entity.NPC.StandardAI.Chase
{
    public class StandardNPCMeleeChasingProperties : EntityStateProperties
    {
        private float chaseSpeed = 3.0f;
        public float ChaseSpeed => chaseSpeed;
        
        private float chaseRange = 5.0f;
        public float ChaseRange => chaseRange;
        
        private float attackRange = 3.0f;
        public float AttackRange => attackRange;

        public StandardNPCMeleeChasingProperties(NPCAIConfiguration config)
        {
            this.chaseSpeed = config.chaseSpeed;
            this.chaseRange = config.chaseRange;
            this.attackRange = config.attackRange;
        }


        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        }
    }
}