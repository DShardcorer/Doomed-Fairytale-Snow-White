using UnityEngine;

namespace NarrativeSystem
{
    [CreateAssetMenu(menuName = "Narrative/Condition/FlagValue")]
    public class FlagValueCondition : NarrativeCondition
    {
        public string flagName;
        public bool expectedValue = true;

        public override bool IsMet(WorldState worldState)
        {
            return worldState.GetFlag(flagName) == expectedValue;
        }
    }
}