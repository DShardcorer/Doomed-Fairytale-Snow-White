using Sirenix.OdinInspector;
using UnityEngine;

namespace Entity.NPC.Spawning
{
    [System.Serializable]
    public class SceneNPCData
    {
        [BoxGroup("Base")]
        public string npcKey;
        [BoxGroup("Base")]
        public NPCSpawnData npcData;
        [BoxGroup("Base")]
        public Transform spawnPoint;
        [BoxGroup("Base")]
        public bool spawnOnStart = true;
        [BoxGroup("Base"), Tooltip("Set to true if this NPC should persist when changing scenes")]
        public bool isPersistent = false;

        public virtual void Setup(NPC npc)
        {
            // This method should be overridden in derived classes to set up the NPC
        }
    }
}