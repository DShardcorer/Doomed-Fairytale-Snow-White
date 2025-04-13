using System.Collections.Generic;
using UnityEngine.Events;

namespace NarrativeSystem
{
    [System.Serializable]
    public class NarrativeTrigger
    {
        public List<NarrativeCondition> conditions;
        public NarrativeNode nextNode;
        public UnityEvent onTriggered;

        public bool AreConditionsMet(WorldState worldState)
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsMet(worldState))
                    return false;
            }

            return true;
        }
    }
}