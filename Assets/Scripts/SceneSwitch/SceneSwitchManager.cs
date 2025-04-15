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
        private bool _loadFromPortalUse = false;
        private PlayerView _playerView;
        private Vector3 _portalStartPosition;
        

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Initialize()
        {
            _playerView = GameManager.Instance.PlayerManager.GetPlayer().PlayerView;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneFadeManager.Instance.StartFadeIn();
            Debug.Log("Scene Loaded");
            if (_loadFromPortalUse)
            {
                //warp player to correct location
                FindPortal(_portalToSpawnTo);
                _playerView.transform.position = _portalStartPosition;
                _loadFromPortalUse = false; 
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        public void SwitchSceneFromPortalUse(SceneField sceneToLoad,
            SceneSwitchPortal.PortalToSpawnAt portalToSpawnAt = SceneSwitchPortal.PortalToSpawnAt.None)
        {
            _loadFromPortalUse = true;
            StartCoroutine(FadeOutThenChangeScene(sceneToLoad, portalToSpawnAt));

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
                    _portalStartPosition = portal.transform.position + (Vector3)portal.PlayerSpawnOffset;
                    break;
                }
            }
        }
    }
}