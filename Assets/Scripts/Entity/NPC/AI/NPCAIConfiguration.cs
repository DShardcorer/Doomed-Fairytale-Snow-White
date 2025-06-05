using UnityEngine;
using Entity.AttackCheck;

namespace Entity.NPC.AI
{
    [CreateAssetMenu(fileName = "NPCAIConfig", menuName = "Game/AI/NPC AI Configuration")]
    public class NPCAIConfiguration : ScriptableObject
    {
        [Header("Detection Settings")]
        public float detectionRadius = 10f;
        public float fieldOfViewAngle = 90f;

        [Header("Behavior Settings")]
        public bool shouldChaseTargets = true;
        public bool shouldAttackTargets = true;
        public bool shouldPatrol = false;
        public float maxChaseDistance = 20f;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float chaseSpeed = 3f;
        public float chaseRange = 5f;

        [Header("Patrol Settings")]
        public float patrolRadius = 10f;
        public float patrolWaitTime = 3f;

        [Header("Attack Settings")]
        public AttackType attackType = AttackType.OverlapCircle;
        public float attackRange = 2f;
        public float attackDamage = 10f;
        public float attackCooldown = 2f;

        [Header("Advanced Behavior Settings")]
        public float preferredDistance = 5f;
        public float fleeDistance = 10f;
        public float fleeTime = 5f;
        public float healthFleeThreshold = 0.25f;

        // Factory Methods for Presets
        public static NPCAIConfiguration CreateMeleeConfig()
        {
            var config = CreateInstance<NPCAIConfiguration>();
            config.shouldChaseTargets = true;
            config.attackRange = 1.5f;
            config.preferredDistance = 1.5f;
            config.fleeDistance = 8f;
            config.fleeTime = 3f;
            config.patrolRadius = 5f;
            config.patrolWaitTime = 2f;
            config.healthFleeThreshold = 0.2f;
            return config;
        }

        public static NPCAIConfiguration CreateRangedConfig()
        {
            var config = CreateInstance<NPCAIConfiguration>();
            config.shouldChaseTargets = true;
            config.attackRange = 8f;
            config.preferredDistance = 6f;
            config.fleeDistance = 10f;
            config.fleeTime = 4f;
            config.patrolRadius = 7f;
            config.patrolWaitTime = 1.5f;
            config.healthFleeThreshold = 0.3f;
            return config;
        }

        public static NPCAIConfiguration CreateSkittishConfig()
        {
            var config = CreateInstance<NPCAIConfiguration>();
            config.shouldChaseTargets = false;
            config.attackRange = 1f;
            config.preferredDistance = 8f;
            config.fleeDistance = 15f;
            config.fleeTime = 8f;
            config.patrolRadius = 3f;
            config.patrolWaitTime = 4f;
            config.healthFleeThreshold = 0.8f;
            return config;
        }
    }
}
