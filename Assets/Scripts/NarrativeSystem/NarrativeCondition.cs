using UnityEngine;

namespace NarrativeSystem
{
    public abstract class NarrativeCondition : ScriptableObject
    {
        public abstract bool IsMet(WorldState worldState);
    }
}