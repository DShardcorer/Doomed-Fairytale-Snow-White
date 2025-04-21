using Entity.AttackCheck;
using UnityEngine;

namespace Entity.NPC.AI
{
    [CreateAssetMenu(fileName = "NPCAIConfig", menuName = "Game/AI/NPC AI Configuration")]
    public class NPCAIConfiguration : ScriptableObject
    {
        [Header("Detection Settings")] public float detectionRadius = 10f;
        public float fieldOfViewAngle = 90f;

        [Header("Behavior Settings")] public bool shouldChaseTargets = true;
        public bool shouldAttackTargets = true;
        public bool shouldPatrol = false;
        public float maxChaseDistance = 20f;

        [Header("Patrol Settings")] public float patrolRadius = 10f;
        public float patrolWaitTime = 3f;
        
        [Header("Idle Settings")] 
        public float idleTime = 2f;
        
        [Header("Move Settings")] 
        public float moveSpeed = 2f;
        public float movingTime = 2f;
        
        [Header("Chase Settings")]
        public float chaseSpeed = 3f;
        public float attackCooldown = 2f;
        public float chaseRange = 5f;

        [Header("Attack Settings")] public AttackType attackType = AttackType.OverlapCircle;
        public float attackRange = 1f;
        public float attackDamage = 10f;
    }
}