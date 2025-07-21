using FlagSystem.FlagSystem;
using UnityEngine;

namespace FlagSystem
{
    [CreateAssetMenu(fileName = "NewFlagReference", menuName = "RPG/Flag Reference")]
    public class FlagReference : ScriptableObject
    {
        public enum FlagCategory
        {
            Quest,
            Dialogue,
            World,
            Player,
            Achievement
        }

        [Tooltip("Category for organization")]
        public FlagCategory category;

        [Tooltip("The flag data")]
        [SerializeReference] public GameFlag flag;

        [Tooltip("Description of this flag's purpose")]
        [TextArea] public string description;

        // Helper methods
        public string GetFlagId() => flag?.id;
        
        public bool GetBoolValue() 
        {
            if (flag is BoolGameFlag boolFlag)
                return boolFlag.value;
            return false;
        }
        
        public int GetIntValue() 
        {
            if (flag is IntGameFlag intFlag)
                return intFlag.value;
            return 0;
        }
        
        // Similar helpers for other types
        
        // Setter methods that update the FlagManager
        public void SetBoolValue(bool value)
        {
            if (flag != null)
                FlagManager.Instance.SetBool(flag.id, value);
        }
    }
}