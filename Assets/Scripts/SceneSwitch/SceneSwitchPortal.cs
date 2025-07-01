using UnityEngine;


namespace SceneSwitch
{
    public class SceneSwitchPortal : MonoBehaviour
    {
        public enum PortalToSpawnAt
        {
            None,
            Left,
            Right,
            Up,
            Down,
            Spawn
        }

        [SerializeField]
        private bool _isPortalToOverworld = false;

        [SerializeField] private PortalToSpawnAt _portalToSpawnTo;
        [SerializeField] private SceneField _sceneToLoad;


        [Space(10f)] [SerializeField]
        private PortalToSpawnAt _currentPortal;

        public PortalToSpawnAt CurrentPortal
        {
            get { return _currentPortal; }
        }

        [SerializeField] private Vector2 _playerSpawnOffset = new Vector2(0, -5f);

        public Vector2 PlayerSpawnOffset
        {
            get { return _playerSpawnOffset; }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (SceneSwitchManager.Instance == null)
                {
                    Debug.LogError("SceneSwitchManager is null");
                    return;
                }

                if (_isPortalToOverworld)
                {
                    SceneSwitchManager.Instance.SwitchSceneToOverworld();
                }
                else
                {
                    //Debug null fields
                    if (_sceneToLoad == null)
                    {
                        Debug.LogError("Scene to load is null");
                        return;
                    }
                    SceneSwitchManager.Instance.SwitchSceneToPortal(_sceneToLoad, _portalToSpawnTo);
                }
            }
        }
    }
}