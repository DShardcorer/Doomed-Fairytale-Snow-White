using UnityEngine;

namespace EntitySystems.Stats
{
    [CreateAssetMenu(fileName = "AbilityStatboardSO", menuName = "Stats/AbilityStatboardSO", order = 1)]
    public class AbilityStatboardSO : ScriptableObject
    {
        public float Strength =10;
        public float Dexterity =10;
        public float Constitution =10;
        public float Intelligence =10;
        public float Wisdom =10;
        public float Charisma =10;
    
    }
}
