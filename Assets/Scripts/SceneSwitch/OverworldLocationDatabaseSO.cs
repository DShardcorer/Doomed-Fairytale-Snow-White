using System;
using UnityEngine;

namespace SceneSwitch
{
    [CreateAssetMenu(fileName = "OverworldLocationDatabaseSO", menuName = "Overworld/OverworldLocationDatabaseSO")]
    public class OverworldLocationDatabaseSO: ScriptableObject
    {
        [Serializable]
        public struct OverworldLocation
        {
            public SceneField scene;
            public Vector3 overworldStartPosition;
        }
        [SerializeField]
        public OverworldLocation[] overworldLocations;
        
    }
}