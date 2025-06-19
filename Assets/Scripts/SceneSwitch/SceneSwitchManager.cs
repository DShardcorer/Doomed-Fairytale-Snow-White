using System.Collections;
using DefaultNamespace.Utility;
using Entity.Player;
using GeneralManagers;
using Tile;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSwitch
{
    public class SceneSwitchManager : MonoBehaviour
    {
        public static SceneSwitchManager Instance;

        private SceneSwitchPortal.PortalToSpawnAt _portalToSpawnTo;
        private bool _loadToPortal = false;
        private bool _loadToOverworld = false;
        private PlayerView _playerView;
        private Vector3 _portalSpawnPosition;
        private Vector3 _overworldSpawnPosition = new Vector3(0, 0, 0);
        public Vector3 OverworldSpawnPosition => _overworldSpawnPosition;
        [SerializeField] private SceneField _overworldScene;
        private PlayerViewManager _playerViewManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


// Update Initialize method
        public void Initialize()
        {
            _playerView = GameManager.Instance.PlayerManager.Player.PlayerView;
        }


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneFadeManager.Instance.StartFadeIn();

            // Check if this is an overworld scene
            if (_playerViewManager == null)
            {
                _playerViewManager = GameManager.Instance.PlayerViewManager;
            }

            bool isOverworld = _playerViewManager.IsOverworldScene(scene.name);


            if (isOverworld)
            {

                if (_loadToOverworld)
                {
                    _playerViewManager.SwitchToOverworldView(_overworldSpawnPosition);
                    _loadToOverworld = false;
                }
                else if (_loadToPortal)
                {
                    FindPortal(_portalToSpawnTo);
                    _playerViewManager.SwitchToOverworldView(_portalSpawnPosition);
                    _loadToPortal = false;
                }
            }
            else
            {
                // For non-overworld scenes, use the normal player view
                if (_loadToOverworld)
                {
                    _playerViewManager.SwitchToNormalView(_overworldSpawnPosition);
                    _loadToOverworld = false;
                }
                else if (_loadToPortal)
                {
                    FindPortal(_portalToSpawnTo);
                    Debug.Log("Portal spawn position: " + _portalSpawnPosition);
                    _playerViewManager.SwitchToNormalView(_portalSpawnPosition);
                    _loadToPortal = false;
                }
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        public void SetLastOverworldSpawnPosition(Vector3 position)
        {
            _overworldSpawnPosition = position;
        }

        public void SwitchSceneToPortal(SceneField sceneToLoad,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            _loadToPortal = true;
            StartCoroutine(FadeOutThenChangeScene(sceneToLoad, portalToSpawnAt));
        }

        public void SwitchSceneFromOverworldToPortal(SceneField sceneToLoad,
            Vector3 overworldEnterPosition,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            _loadToPortal = true;
            _overworldSpawnPosition = overworldEnterPosition;
            StartCoroutine(FadeOutThenChangeScene(sceneToLoad, portalToSpawnAt));
        }

        public void SwitchSceneToOverworld()
        {
            _loadToOverworld = true;
            StartCoroutine(FadeOutThenChangeScene(_overworldScene));
        }

        public void SwitchSceneSpecial(SceneField sceneToLoad)
        {
            // Disable player movement/interaction
            if (_playerView != null)
            {
                _playerView.gameObject.SetActive(false);
            }

            StartCoroutine(FadeOutThenLoadSpecialScene(sceneToLoad));
        }

        private IEnumerator FadeOutThenLoadSpecialScene(SceneField sceneToLoad)
        {
            SceneFadeManager.Instance.StartFadeOut();
            if (SceneFadeManager.Instance.IsFadingOut)
            {
                yield return null;
            }

            // No need to set portal or position variables
            SceneManager.LoadScene(sceneToLoad);
        }

// Add this method to re-enable the player when leaving a special scene
        public void ExitSpecialScene(SceneField destinationScene, Vector3 spawnPosition)
        {
            // Re-enable the player
            if (_playerView != null)
            {
                _playerView.gameObject.SetActive(true);
            }

            // Use existing scene switch method to return to a normal scene
            SwitchSceneToOverworld(spawnPosition);
        }

        public void ExitSpecialScene(SceneField destinationScene,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            // Re-enable the player
            if (_playerView != null)
            {
                _playerView.gameObject.SetActive(true);
            }

            // Use existing scene switch method to return to a normal scene
            SwitchSceneToPortal(destinationScene, portalToSpawnAt);
        }

        public void SwitchSceneToOverworld(Vector3 overworldSpawnPosition)
        {
            _loadToOverworld = true;
            _overworldSpawnPosition = overworldSpawnPosition;
            StartCoroutine(FadeOutThenChangeScene(_overworldScene));
        }

        private IEnumerator FadeOutThenChangeScene(SceneField sceneToLoad,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            SceneFadeManager.Instance.StartFadeOut();
            if (SceneFadeManager.Instance.IsFadingOut)
            {
                yield return null;
            }

            _portalToSpawnTo = portalToSpawnAt;
            SceneManager.LoadScene(sceneToLoad);
        }

        #region STRING OVERLOADS

        // String-based overload for SwitchSceneToPortal
        public void SwitchSceneToPortal(string sceneName,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            _loadToPortal = true;
            StartCoroutine(FadeOutThenChangeScene(sceneName, portalToSpawnAt));
        }

// String-based overload for SwitchSceneFromOverworldToPortal
        public void SwitchSceneFromOverworldToPortal(string sceneName,
            Vector3 overworldEnterPosition,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            _loadToPortal = true;
            _overworldSpawnPosition = overworldEnterPosition;
            StartCoroutine(FadeOutThenChangeScene(sceneName, portalToSpawnAt));
        }

// String-based overload for SwitchSceneToOverworld
        public void SwitchSceneToOverworld(string overworldSceneName)
        {
            _loadToOverworld = true;
            StartCoroutine(FadeOutThenChangeScene(overworldSceneName));
        }

// String-based overload for SwitchSceneSpecial
        public void SwitchSceneSpecial(string sceneName)
        {
            if (_playerView != null)
            {
                _playerView.gameObject.SetActive(false);
            }

            StartCoroutine(FadeOutThenLoadSpecialScene(sceneName));
        }

// String-based overload for ExitSpecialScene with position
        public void ExitSpecialScene(string destinationScene, Vector3 spawnPosition)
        {
            if (_playerView != null)
            {
                _playerView.gameObject.SetActive(true);
            }

            _loadToOverworld = true;
            _overworldSpawnPosition = spawnPosition;
            StartCoroutine(FadeOutThenChangeScene(destinationScene));
        }

// String-based overload for ExitSpecialScene with portal
        public void ExitSpecialScene(string destinationScene,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            if (_playerView != null)
            {
                _playerView.gameObject.SetActive(true);
            }

            SwitchSceneToPortal(destinationScene, portalToSpawnAt);
        }

// Additional helper methods for string-based scene loading
        private IEnumerator FadeOutThenLoadSpecialScene(string sceneName)
        {
            SceneFadeManager.Instance.StartFadeOut();
            if (SceneFadeManager.Instance.IsFadingOut)
            {
                yield return null;
            }

            SceneManager.LoadScene(sceneName);
        }

        private IEnumerator FadeOutThenChangeScene(string sceneName,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            SceneFadeManager.Instance.StartFadeOut();
            if (SceneFadeManager.Instance.IsFadingOut)
            {
                yield return null;
            }

            _portalToSpawnTo = portalToSpawnAt;
            SceneManager.LoadScene(sceneName);
        }

        #endregion


        private void FindPortal(SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt)
        {
            SceneSwitchPortal[] portals = FindObjectsByType<SceneSwitchPortal>(FindObjectsSortMode.None);
            foreach (SceneSwitchPortal portal in portals)
            {
                //Log out the two portal types
                if (portalToSpawnAt == portal.CurrentPortal)
                {
                    _portalSpawnPosition = portal.transform.position + (Vector3)portal.PlayerSpawnOffset;
                    Debug.LogWarning("Portal spawn position: " + _portalSpawnPosition);
                    break;
                }
            }
        }
    }
}