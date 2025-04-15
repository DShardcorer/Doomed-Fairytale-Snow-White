using UnityEngine;


namespace SceneSwitch
{
    public class SceneSwitchPortal : MonoBehaviour
    {
        public enum PortalToSpawnAt
        {
            None,
            One,
            Two,
            Three,
            Four,
        }

        [Header("Spawn TO")] [SerializeField] private PortalToSpawnAt _portalToSpawnTo;
        [SerializeField] private SceneField _sceneToLoad;


        [Space(10f)] [Header("THIS PORTAL")] [SerializeField]
        private PortalToSpawnAt _currentPortal;
        public PortalToSpawnAt CurrentPortal { get { return _currentPortal; } }
        
        private Vector2 _playerSpawnOffset = new Vector2(0, -5f);
        public Vector2 PlayerSpawnOffset { get { return _playerSpawnOffset; } }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug null fields
                if (_sceneToLoad == null)
                {
                    Debug.LogError("Scene to load is null");
                    return;
                }
                if (SceneSwitchManager.Instance == null)
                {
                    Debug.LogError("SceneSwitchManager is null");
                    return;
                }
                
                SceneSwitchManager.Instance.SwitchSceneFromPortalUse(_sceneToLoad, _portalToSpawnTo);
            }
        }
    }
}