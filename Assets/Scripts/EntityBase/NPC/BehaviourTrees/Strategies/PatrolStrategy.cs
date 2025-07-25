using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

namespace EntityBase.NPC.BehaviourTrees.Strategies
{
    public class PatrolStrategy : IStrategy
    {
        private NPC npc;
        private IAstarAI astarAI;
        private List<Vector3> patrolPoints = new List<Vector3>();
        private int currentPatrolPointIndex;
        private float waitTime = 2f;
        private float waitCounter = 0f;
        private bool isWaiting = false;
        private float reachDistance = 0.25f;
        private Vector3 spawnPosition;
        private float patrolRadius = 5f;

        public PatrolStrategy(NPC npc, List<Vector3> patrolPoints = null, float waitTime = 2f,
            float reachDistance = 0.5f)
        {
            this.npc = npc;
            this.astarAI = npc.View.GetComponent<IAstarAI>();
            this.waitTime = waitTime;
            this.reachDistance = reachDistance;
            this.spawnPosition = npc.View.transform.position;

            // Generate patrol points if none provided
            if (patrolPoints != null)
            {
                SetPatrolPoints(patrolPoints);
            }


            // Start moving immediately
            if (this.patrolPoints.Count > 0)
            {
                MoveToCurrentPatrolPoint();
            }
        }

        public void SetPatrolPoints(List<Vector3> newPatrolPoints)
        {
            if (newPatrolPoints != null && newPatrolPoints.Count > 0)
            {
                patrolPoints.Clear();
                patrolPoints.AddRange(newPatrolPoints);
                currentPatrolPointIndex = 0;

                // Log patrol points for debugging
                for (int i = 0; i < patrolPoints.Count; i++)
                {
                    // Debug.LogWarning($"Patrol Strategy Point {i}: {patrolPoints[i]}");
                }
            }
            else
            {
                Debug.LogWarning("PatrolStrategy: Attempted to set empty patrol points");
            }
        }
        

        public Node.Status Process()
        {
            // if (npc.NPCProperties.target != null)
            // {
            //     return Node.Status.Failure; // Let other nodes handle combat
            // }

            // If waiting at a point
            if (isWaiting)
            {
                HandleWaiting();
                return Node.Status.Running;
            }
            

            if (astarAI.reachedDestination && !isWaiting)
            {
                // We've reached the point, start waiting
                Debug.LogWarning($"Reached patrol point {currentPatrolPointIndex}, waiting {waitTime} seconds");
                isWaiting = true;
                astarAI.canMove = false;
            }
            else
            {
                Vector3 dirToTarget = (patrolPoints[currentPatrolPointIndex] - npc.View.transform.position).normalized;
                npc.NPCProperties.SetLastMovementVector(dirToTarget);
            }

            // CRITICAL: Always return Running to keep the patrol behavior active
            return Node.Status.Running;
        }

        private void HandleWaiting()
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                waitCounter = 0f;
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
                Debug.LogWarning($"Waiting finished, moving to next patrol point {currentPatrolPointIndex}");
                MoveToCurrentPatrolPoint();
            }
        }

        private void MoveToCurrentPatrolPoint()
        {
            if (patrolPoints.Count > 0 && astarAI != null)
            {
                Debug.LogWarning(
                    $"Moving to patrol point {currentPatrolPointIndex}: {patrolPoints[currentPatrolPointIndex]}");
                astarAI.canMove = true;
                astarAI.destination = patrolPoints[currentPatrolPointIndex];

                // Update facing direction immediately
                Vector3 dirToTarget = (patrolPoints[currentPatrolPointIndex] - npc.View.transform.position).normalized;
                npc.NPCProperties.SetLastMovementVector(dirToTarget);
            }
        }

        public void Reset()
        {
            currentPatrolPointIndex = 0;
            isWaiting = false;
            waitCounter = 0f;

            // Start patrolling immediately on reset
            MoveToCurrentPatrolPoint();
        }
    }
}