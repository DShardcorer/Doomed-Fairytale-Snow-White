using System.Collections.Generic;
using Entity.NPC.AI;
using Helpers;
using UnityEngine;

namespace Entity.NPC.Patrol
{
    public class NPCPatrolState:NPCState
    {
        private List<Vector3> _patrolPoints = new List<Vector3>();
        private int _currentPointIndex = 0;
        private float _waitTime = 2f;
        private float _waitTimer = 0f;
        private bool _isWaiting = false;
        private float _patrolRadius = 5f;
        
        // public NPCPatrolState(NPCAIConfiguration config): this(HelperAnimationStateName.IS_PATROLLING, new NPCPatrolProperties(config))
        // {
        //     // Initialize patrol points within a radius
        //     for (int i = 0; i < 5; i++)
        //     {
        //         Vector3 randomPoint = new Vector3(
        //             Random.Range(-_patrolRadius, _patrolRadius),
        //             0,
        //             Random.Range(-_patrolRadius, _patrolRadius)
        //         );
        //         _patrolPoints.Add(randomPoint);
        //     }
        // }
        public NPCPatrolState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
        }
    }
}