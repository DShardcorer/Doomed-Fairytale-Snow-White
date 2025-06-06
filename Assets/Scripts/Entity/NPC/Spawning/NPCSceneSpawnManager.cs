using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using DefaultNamespace.Utility;
using GeneralManagers;

namespace Entity.NPC.Spawning
{
    /// <summary>
    /// Manages NPC spawning for a specific scene, using NPCSpawnManager as a service
    /// </summary>
    public class NPCSceneSpawnManager : MonoBehaviour
    {
        [System.Serializable]
        public class SceneNPCData
        {
            public string npcKey;
            public NPCSpawnData npcData;
            public Transform spawnPoint;
            public bool spawnOnStart = true;
            
            
            public List<Vector3> patrolPoints = new List<Vector3>();

            [Tooltip("Set to true if this NPC should persist when changing scenes")]
            public bool isPersistent = false;
        }

        [Header("Scene Configuration")] [SerializeField]
        private List<SceneNPCData> sceneNPCs = new List<SceneNPCData>();

        [SerializeField] private bool spawnAllOnStart = true;
        [SerializeField] private float staggeredSpawnDelay = 0.1f;
        private NPCSpawnManager spawnManager;

        // Track spawned NPCs by key for easy lookup
        private Dictionary<string, NPC> spawnedNPCs = new Dictionary<string, NPC>();

        // Additional dictionary to track NPCs by their name
        private Dictionary<string, NPC> spawnedNPCsByName = new Dictionary<string, NPC>();


        private async void Start()
        {
            await ServiceLocator.InitializationTask;
            spawnManager = GameManager.Instance.NPCSpawnManager;
            if (spawnAllOnStart && spawnManager != null)
            {
                SpawnAllSceneNPCs();
            }
        }

        public async void SpawnAllSceneNPCs()
        {
            for (int i = 0; i < sceneNPCs.Count; i++)
            {
                var npcData = sceneNPCs[i];
                if (npcData.spawnOnStart)
                {
                    // Stagger spawns to avoid performance hitches
                    await Task.Delay((int)(staggeredSpawnDelay * 1000));
                    SpawnSceneNPC(npcData.npcKey);
                }
            }
        }

        public async Task<NPC> SpawnSceneNPCAsync(string npcKey)
        {
            if (spawnManager == null)
            {
                Debug.LogError("Cannot spawn NPC: No NPCSpawnManager available");
                return null;
            }

            // Check if NPC already exists
            if (spawnedNPCs.TryGetValue(npcKey, out var existingNpc))
            {
                Debug.LogWarning($"NPC with key {npcKey} already spawned");
                return existingNpc;
            }

            // Find NPC data by key
            var npcData = sceneNPCs.FirstOrDefault(n => n.npcKey == npcKey);
            if (npcData == null)
            {
                Debug.LogError($"No NPC data found for key: {npcKey}");
                return null;
            }

            // Spawn NPC at the designated point
            Vector3 position = npcData.spawnPoint ? npcData.spawnPoint.position : transform.position;
            Quaternion rotation = npcData.spawnPoint ? npcData.spawnPoint.rotation : Quaternion.identity;

            // Use the spawn manager to handle the actual spawning
            // NPC spawnedNPC = await spawnManager.SpawnNPCAsync(npcData.npcData, position, rotation);
            NPC spawnedNPC = await spawnManager.SpawnNPCAsync(npcData.npcData, position, rotation);
            if (spawnedNPC != null)
            {
                // Add to both dictionaries
                spawnedNPCs[npcKey] = spawnedNPC;

                // Add to name-based dictionary
                string npcName = spawnedNPC.Profile.Name;
                if (!string.IsNullOrEmpty(npcName))
                {
                    // Handle name conflicts with a numbering scheme
                    if (spawnedNPCsByName.ContainsKey(npcName))
                    {
                        int counter = 1;
                        string uniqueName = $"{npcName}_{counter}";
                        while (spawnedNPCsByName.ContainsKey(uniqueName))
                        {
                            counter++;
                            uniqueName = $"{npcName}_{counter}";
                        }

                        Debug.LogWarning($"NPC name conflict: {npcName} already exists. Using {uniqueName} instead.");
                        spawnedNPCsByName[uniqueName] = spawnedNPC;
                    }
                    else
                    {
                        spawnedNPCsByName[npcName] = spawnedNPC;
                    }
                }

                // Set DontDestroyOnLoad if this NPC should persist
                if (npcData.isPersistent && spawnedNPC.NPCView != null)
                {
                    DontDestroyOnLoad(spawnedNPC.NPCView.gameObject);
                }
            }
            spawnedNPC.Initialize();
            return spawnedNPC;
        }
        
        public NPC SpawnSceneNPC(string npcKey)
        {
            if (spawnManager == null)
            {
                Debug.LogError("Cannot spawn NPC: No NPCSpawnManager available");
                return null;
            }

            // Check if NPC already exists
            if (spawnedNPCs.TryGetValue(npcKey, out var existingNpc))
            {
                Debug.LogWarning($"NPC with key {npcKey} already spawned");
                return existingNpc;
            }

            // Find NPC data by key
            var npcData = sceneNPCs.FirstOrDefault(n => n.npcKey == npcKey);
            if (npcData == null)
            {
                Debug.LogError($"No NPC data found for key: {npcKey}");
                return null;
            }

            // Spawn NPC at the designated point
            Vector3 position = npcData.spawnPoint ? npcData.spawnPoint.position : transform.position;
            Quaternion rotation = npcData.spawnPoint ? npcData.spawnPoint.rotation : Quaternion.identity;

            // Use the spawn manager to handle the actual spawning
            NPC spawnedNPC = spawnManager.SpawnNPC(npcData.npcData, position, rotation);
            if (spawnedNPC != null)
            {
                // Add to both dictionaries
                spawnedNPCs[npcKey] = spawnedNPC;

                // Add to name-based dictionary
                string npcName = spawnedNPC.Profile.Name;
                if (!string.IsNullOrEmpty(npcName))
                {
                    // Handle name conflicts with a numbering scheme
                    if (spawnedNPCsByName.ContainsKey(npcName))
                    {
                        int counter = 1;
                        string uniqueName = $"{npcName}_{counter}";
                        while (spawnedNPCsByName.ContainsKey(uniqueName))
                        {
                            counter++;
                            uniqueName = $"{npcName}_{counter}";
                        }

                        Debug.LogWarning($"NPC name conflict: {npcName} already exists. Using {uniqueName} instead.");
                        spawnedNPCsByName[uniqueName] = spawnedNPC;
                    }
                    else
                    {
                        spawnedNPCsByName[npcName] = spawnedNPC;
                    }
                }

                // Set DontDestroyOnLoad if this NPC should persist
                if (npcData.isPersistent && spawnedNPC.NPCView != null)
                {
                    DontDestroyOnLoad(spawnedNPC.NPCView.gameObject);
                }
            }
            
            spawnedNPC.Initialize();
            return spawnedNPC;
        }

        public void DespawnSceneNPC(string npcKey)
        {
            if (spawnedNPCs.TryGetValue(npcKey, out var npc))
            {
                // Remove from name-based dictionary as well
                string npcName = FindNPCNameInDictionary(npc);
                if (npcName != null)
                {
                    spawnedNPCsByName.Remove(npcName);
                }

                Destroy(npc.NPCView.gameObject);
                spawnedNPCs.Remove(npcKey);
            }
        }

        // Helper method to find NPC name in the dictionary
        private string FindNPCNameInDictionary(NPC npc)
        {
            foreach (var pair in spawnedNPCsByName)
            {
                if (pair.Value == npc)
                {
                    return pair.Key;
                }
            }

            return null;
        }

        public NPC GetSpawnedNPC(string npcKey)
        {
            if (spawnedNPCs.TryGetValue(npcKey, out var npc))
            {
                return npc;
            }

            return null;
        }

        public NPC GetSpawnedNPCByName(string npcName)
        {
            if (spawnedNPCsByName.TryGetValue(npcName, out var npc))
            {
                return npc;
            }

            return null;
        }

        public bool IsNPCSpawned(string npcKey)
        {
            return spawnedNPCs.ContainsKey(npcKey);
        }

        public bool IsNPCWithNameSpawned(string npcName)
        {
            return spawnedNPCsByName.ContainsKey(npcName);
        }

        public List<NPC> GetAllSpawnedNPCs()
        {
            return spawnedNPCs.Values.ToList();
        }

        public List<string> GetAllSpawnedNPCKeys()
        {
            return spawnedNPCs.Keys.ToList();
        }

        public List<string> GetAllSpawnedNPCNames()
        {
            return spawnedNPCsByName.Keys.ToList();
        }

        public void DespawnAllNPCs()
        {
            foreach (var npcKey in spawnedNPCs.Keys.ToList())
            {
                DespawnSceneNPC(npcKey);
            }
        }

        private void OnDestroy()
        {
            foreach (var npc in spawnedNPCs.Values.ToList())
            {
                if (npc != null && npc.NPCView != null)
                {
                    Destroy(npc.NPCView.gameObject);
                }
            }

            spawnedNPCs.Clear();
            spawnedNPCsByName.Clear();
        }
    }
}