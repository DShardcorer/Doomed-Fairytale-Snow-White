using System.Collections.Generic;
using EntityBase.NPC.AI.SubControllers;
using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EntityBase.NPC.Spawning.SceneNPCDataVariants
{
    [System.Serializable]
    public class PatrolSceneNPCData : SceneNPCData
    {
        [BoxGroup("Patrol")] public List<Transform> patrolPoints;

        public override void Setup(NPC npc)
        {
            PatrolNPCSubAIController controller = npc.NPCAIController.GetNPCSubAIController(HelperNPCSubAIControllerName.Patrol) as PatrolNPCSubAIController;
            if (controller != null)
            {
                controller.SetPatrolPoints(GetPatrolPointsAsVector3());
            }
            else
            {
                Debug.LogError("Patrol NPC Sub AI Controller not found in PatrolSceneNPCData.Setup");
            }
            
        }
        //helper class to turn transform list into a vector3 list
        public List<Vector3> GetPatrolPointsAsVector3()
        {
            List<Vector3> points = new List<Vector3>();
            foreach (Transform point in patrolPoints)
            {
                points.Add(point.position);
            }
            return points;
        }
    }
}