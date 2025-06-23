using UnityEngine;

namespace EntityBase.NPC
{
    [CreateAssetMenu(fileName = "NewNPCProfile", menuName = "NPC/NPC Profile")]
    public class NPCProfile : ScriptableObject
    {
        [Header("Identity")]
        public string npcName;

        [Header("Prefab")]
        public GameObject npcPrefab;

    
    }
}
