using System.Collections.Generic;
using Entity.NPC.Spawning;
using Entity.NPC.StandardAI;
using UnityEngine;

namespace Entity.NPC.AI
{
    public static class NPCAIFactory
    {
        private static readonly Dictionary<NPCAIType, AICreationDelegate> _aiCreators = new();

        static NPCAIFactory()
        {
            RegisterDefaults();
        }

        private static void RegisterDefaults()
        {
            Register(NPCAIType.PatrolNormal, data => new PatrolNormalNPCAIController(data.aiConfiguration));
            Register(NPCAIType.GuardAgressive, data => new GuardAgressiveNPCAIController(data.aiConfiguration));
            Register(NPCAIType.WanderPassive, data => new WanderPassiveNPCAIController(data.aiConfiguration));
            Register(NPCAIType.KeepPositionPassive, data => new KeepPositionPassiveNPCAIController(data.aiConfiguration));
            // Register(NPCAIType.Ranged, data => new RangedNPCAIController(data.aiConfiguration));
            
            // Register(NPCAIType.Stealth, data => new StealthNPCAIController(data.aiConfiguration));
            // Register(NPCAIType.Merchant, data => new MerchantNPCAIController(data.aiConfiguration));
        }

        public static void Register(NPCAIType type, AICreationDelegate creator)
        {
            if (creator == null)
            {
                Debug.LogError($"[NPCAIFactory] Tried to register null delegate for {type}");
                return;
            }

            if (_aiCreators.ContainsKey(type))
            {
                Debug.LogWarning($"[NPCAIFactory] Overwriting existing AI creator for {type}");
            }

            _aiCreators[type] = creator;
        }

        public static NPCAIController Create(NPCSpawnData data)
        {
            if (data == null)
            {
                Debug.LogError("[NPCAIFactory] Spawn data is null.");
                return null;
            }

            if (_aiCreators.TryGetValue(data.aiType, out var creator))
            {
                return creator.Invoke(data);
            }

            Debug.LogError($"[NPCAIFactory] No AI creator registered for type: {data.aiType}");
            return null;
        }

        public static void Clear() => _aiCreators.Clear();
    }

    public delegate NPCAIController AICreationDelegate(NPCSpawnData data);
}
