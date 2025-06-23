using EntityBase.NPC.AI.SubControllers;
using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EntityBase.NPC.Spawning.SceneNPCDataVariants
{
    [System.Serializable]
    public class GuardSceneNPCData : SceneNPCData
    {
        [BoxGroup("Guard")] public Transform guardPoint;

        public override void Setup(NPC npc)
        {
            GuardNPCSubAIController controller =
                npc.NPCAIController.GetNPCSubAIController(HelperNPCSubAIControllerName
                    .Guard) as GuardNPCSubAIController;
            if (controller != null)
            {
                controller.SetGuardPosition(guardPoint.position);
            }
            else
            {
                Debug.LogError("Guard NPC Sub AI Controller not found in GuardSceneNPCData.Setup");
            }
        }
    }
}