using System;

namespace Entity.NPC_Variants.Native.Chase
{
    public class NativeChasingProperties: EntityStateProperties
    {
        private float chaseSpeed = 3.0f;
        public float ChaseSpeed => chaseSpeed;

        private float chasingTime = 2.0f;
        public float ChasingTime => chasingTime;

        private float attackCooldown = 2.0f;
        public float AttackCooldown => attackCooldown;

        private float attackRange = 3.0f;

        public NativeChasingProperties(float chaseSpeed = 3f, float chasingTime = 2f, float attackCooldown = 3f)
        {
            this.chaseSpeed = chaseSpeed;
            this.chasingTime = chasingTime;
            this.attackCooldown = attackCooldown;
        }

        public float AttackRange => attackRange;



        protected override void UpdateDerivedProperties(object sender, EventArgs e)
        {
        }
    }
}
