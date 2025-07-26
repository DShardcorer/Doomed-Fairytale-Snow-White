using System.Collections.Generic;
using UnityEngine;

namespace EntityBase.NPC.BlackboardSystem
{
    [CreateAssetMenu(fileName = "BlackboardData", menuName = "NPC/Blackboard/BlackboardData", order = 1)]
    public class BlackboardData: ScriptableObject
    {
        public List<BlackboardEntryData> entries = new();

        public void SetValueOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in entries)
            {
                entry.SetValueOnBlackboard(blackboard);
            }
        }
    }
}