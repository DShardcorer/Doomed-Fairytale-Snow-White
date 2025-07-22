using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

namespace EntityBase.NPC.BehaviourTrees.Strategies
{
    public class PatrolStrategy : IStrategy
    {
        private NPC npc;
        private IAstarAI astarAI;
        private List<Vector3> patrolPoints;
        private int currentPatrolPointIndex;
        private float waitTime = 2f;
        private float waitCounter = 0f;
        private bool isWaiting = false;
        private float reachDistance = 0.5f;

        public PatrolStrategy(NPC npc, List<Vector3> patrolPoints, float waitTime = 2f, float reachDistance = 0.5f)
        {
            this.npc = npc;
            this.astarAI = npc.View.GetComponent<IAstarAI>();
            this.patrolPoints = patrolPoints;
            this.waitTime = waitTime;
            this.reachDistance = reachDistance;
            currentPatrolPointIndex = 0;
        }

        public Node.Status Process()
        {
            // Safety check
            if (npc == null || astarAI == null || patrolPoints == null || patrolPoints.Count == 0)
            {
                Debug.LogWarning("PatrolStrategy: Missing required components or patrol points");
                return Node.Status.Failure;
            }

            // If waiting at a point
            if (isWaiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter >= waitTime)
                {
                    // Waiting finished
                    isWaiting = false;
                    waitCounter = 0f;

                    // Move to next patrol point
                    currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
                }
                else
                {
                    // Still waiting
                    return Node.Status.Running;
                }
            }

            // Check if we've reached the current patrol point
            Vector3 currentDestination = patrolPoints[currentPatrolPointIndex];
            float distanceToTarget = Vector3.Distance(npc.View.transform.position, currentDestination);

            if (distanceToTarget <= reachDistance)
            {
                // We've reached the point, start waiting
                isWaiting = true;
                waitCounter = 0f;

                // Update facing direction toward next point
                if (patrolPoints.Count > 1)
                {
                    Vector3 nextPoint = patrolPoints[(currentPatrolPointIndex + 1) % patrolPoints.Count];
                    Vector3 dirToNextPoint = (nextPoint - npc.View.transform.position).normalized;
                    npc.NPCProperties.lastMovementVector = dirToNextPoint;
                }

                // Stop movement
                astarAI.canMove = false;
                return Node.Status.Running;
            }
            else
            {
                // Move toward the current patrol point
                astarAI.canMove = true;
                astarAI.destination = currentDestination;

                // Update facing direction
                npc.NPCProperties.SetLastMovementVector((currentDestination - npc.View.transform.position).normalized);


                return Node.Status.Running;
            }
        }

        public void Reset()
        {
            currentPatrolPointIndex = 0;
            isWaiting = false;
            waitCounter = 0f;

            if (astarAI != null)
            {
                astarAI.canMove = true;
                if (patrolPoints != null && patrolPoints.Count > 0)
                {
                    astarAI.destination = patrolPoints[0];
                }
            }
        }
    }
}