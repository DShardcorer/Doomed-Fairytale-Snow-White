using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using DefaultNamespace.Utility;
using GeneralManagers;
using LBG;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace EntityBase.NPC.Spawning
{
    /// <summary>
    /// Manages NPC spawning for a specific scene, using NPCSpawnManager as a service
    /// </summary>
    public class NPCSceneSpawnManager : MonoBehaviour
    {
        [Header("Scene Configuration")] [OdinSerialize, SerializeReference, SubclassSelector]
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
                    SpawnSceneNPCByIndex(i);
                }
            }
        }

        private NPC SpawnSceneNPCByIndex(int index)
        {
            if (index < 0 || index >= sceneNPCs.Count)
            {
                Debug.LogError($"Invalid NPC index: {index}");
                return null;
            }
            
            return SpawnSceneNPCFromData(sceneNPCs[index]);
        }

        public async Task<NPC> SpawnSceneNPCAsync(string npcName)
        {
            if (spawnManager == null)
            {
                Debug.LogError("Cannot spawn NPC: No NPCSpawnManager available");
                return null;
            }

            // Check if NPC already exists
            if (spawnedNPCsByName.TryGetValue(npcName, out var existingNpc))
            {
                Debug.LogWarning($"NPC with name {npcName} already spawned");
                return existingNpc;
            }

            // Find NPC data for an NPC with a profile that will match this name
            // This is a heuristic and might not work perfectly without manual keys
            var npcData = sceneNPCs.FirstOrDefault(n => n.npcData.npcProfile.Name == npcName);
            if (npcData == null)
            {
                Debug.LogError($"No NPC data found that would produce an NPC named: {npcName}");
                return null;
            }

            // Spawn NPC at the designated point
            Vector3 position = npcData.spawnPoint ? npcData.spawnPoint.position : transform.position;
            Quaternion rotation = npcData.spawnPoint ? npcData.spawnPoint.rotation : Quaternion.identity;

            // Use the spawn manager to handle the actual spawning
            NPC spawnedNPC = await spawnManager.SpawnNPCAsync(npcData.npcData, position, rotation);
            if (spawnedNPC != null)
            {
                // Use profile name as key
                string actualKey = spawnedNPC.Profile.Name;

                // Check if NPC already exists with this key
                if (spawnedNPCs.ContainsKey(actualKey))
                {
                    int counter = 1;
                    string uniqueKey = $"{actualKey}_{counter}";
                    while (spawnedNPCs.ContainsKey(uniqueKey))
                    {
                        counter++;
                        uniqueKey = $"{actualKey}_{counter}";
                    }
                    actualKey = uniqueKey;
                }

                // Add to key-based dictionary
                spawnedNPCs[actualKey] = spawnedNPC;

                // Add to name-based dictionary
                string npcProfileName = spawnedNPC.Profile.Name;
                if (!string.IsNullOrEmpty(npcProfileName))
                {
                    // Handle name conflicts with a numbering scheme
                    if (spawnedNPCsByName.ContainsKey(npcProfileName))
                    {
                        int counter = 1;
                        string uniqueName = $"{npcProfileName}_{counter}";
                        while (spawnedNPCsByName.ContainsKey(uniqueName))
                        {
                            counter++;
                            uniqueName = $"{npcProfileName}_{counter}";
                        }

                        Debug.LogWarning($"NPC name conflict: {npcProfileName} already exists. Using {uniqueName} instead.");
                        spawnedNPCsByName[uniqueName] = spawnedNPC;
                    }
                    else
                    {
                        spawnedNPCsByName[npcProfileName] = spawnedNPC;
                    }
                }
            }

            spawnedNPC.Initialize();
            npcData.Setup(spawnedNPC);
            return spawnedNPC;
        }

        public NPC SpawnSceneNPC(string profileName)
        {
            if (spawnManager == null)
            {
                Debug.LogError("Cannot spawn NPC: No NPCSpawnManager available");
                return null;
            }

            // Find NPC data with matching profile name
            var npcData = sceneNPCs.FirstOrDefault(n => n.npcData.npcProfile.Name == profileName);
            if (npcData == null)
            {
                Debug.LogError($"No NPC data found with profile name: {profileName}");
                return null;
            }

            return SpawnSceneNPCFromData(npcData);
        }

        private NPC SpawnSceneNPCFromData(SceneNPCData npcData)
        {
            if (spawnManager == null)
            {
                Debug.LogError("Cannot spawn NPC: No NPCSpawnManager available");
                return null;
            }

            // Spawn NPC at the designated point
            Vector3 position = npcData.spawnPoint ? npcData.spawnPoint.position : transform.position;
            Quaternion rotation = npcData.spawnPoint ? npcData.spawnPoint.rotation : Quaternion.identity;

            // Use the spawn manager to handle the actual spawning
            NPC spawnedNPC = spawnManager.SpawnNPC(npcData.npcData, position, rotation);
            if (spawnedNPC != null)
            {
                // Use profile name as key
                string actualKey = spawnedNPC.Profile.Name;

                // Check if NPC already exists with this key
                if (spawnedNPCs.ContainsKey(actualKey))
                {
                    int counter = 1;
                    string uniqueKey = $"{actualKey}_{counter}";
                    while (spawnedNPCs.ContainsKey(uniqueKey))
                    {
                        counter++;
                        uniqueKey = $"{actualKey}_{counter}";
                    }
                    actualKey = uniqueKey;
                }

                // Add to key-based dictionary
                spawnedNPCs[actualKey] = spawnedNPC;

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
            }

            spawnedNPC.Initialize();
            npcData.Setup(spawnedNPC);
            return spawnedNPC;
        }

        public void DespawnSceneNPC(string npcKey)
        {
            if (spawnedNPCs.TryGetValue(npcKey, out var npc))
            {
                string npcName = FindNPCNameInDictionary(npc);
                if (npcName != null)
                {
                    spawnedNPCsByName.Remove(npcName);
                }

                npc.Dispose();
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
            int npcCount = spawnedNPCs.Values.Count;

            int disposedCount = 0;
            foreach (var npc in spawnedNPCs.Values.ToList())
            {
                try
                {
                    if (npc != null)
                    {
                        npc.Dispose();
                        disposedCount++;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error disposing NPC: {ex.Message}");
                }
            }

            spawnedNPCs.Clear();
            spawnedNPCsByName.Clear();
        }
    }
}