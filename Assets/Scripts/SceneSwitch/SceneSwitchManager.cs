using System.Collections;
using Entity.Player;
using GeneralManagers;
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
        [SerializeField] private SceneField _overworldScene;


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

        public void Initialize()
        {
            _playerView = GameManager.Instance.PlayerManager.Player.PlayerView;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneFadeManager.Instance.StartFadeIn();
            Debug.Log("Scene Loaded");
            if (_loadToPortal)
            {
                //warp player to correct location
                FindPortal(_portalToSpawnTo);
                _playerView.transform.position = _portalSpawnPosition;
                _loadToPortal = false;
            }

            if (_loadToOverworld)
            {
                _playerView.transform.position = _overworldSpawnPosition;
                _loadToOverworld = false;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
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


        private void FindPortal(SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt)
        {
            SceneSwitchPortal[] portals = FindObjectsByType<SceneSwitchPortal>(FindObjectsSortMode.None);
            foreach (SceneSwitchPortal portal in portals)
            {
                if (portalToSpawnAt == portal.CurrentPortal)
                {
                    _portalSpawnPosition = portal.transform.position + (Vector3)portal.PlayerSpawnOffset;
                    break;
                }
            }
        }
    }
}