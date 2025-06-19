using System.Collections.Generic;
using UnityEngine;
using Entity.Player.Overworld;
using GeneralManagers;
using UnityEngine.Tilemaps;

namespace Entity.Player
{
    public class PlayerViewManager : MonoBehaviour, ILifecycle<GameManager>
    {
        [SerializeField] private GameObject overworldPlayerObject;
        private List<string> overworldSceneNames = new List<string> { "Scene_Overworld" };

        private Player _player;
        private PlayerView _normalPlayerView;
        private OverworldPlayerView _overworldPlayerView;
        private bool _isInOverworld = false;

        public void Initialize(GameManager parent)
        {
            _player = parent.PlayerManager.Player;
            _normalPlayerView = parent.PlayerManager.Player.PlayerView;
            
            if (overworldPlayerObject != null)
            {
                _overworldPlayerView = overworldPlayerObject.GetComponent<OverworldPlayerView>();
                overworldPlayerObject.SetActive(false); // Hide it initially
            }
            else
            {
                Debug.LogWarning("Overworld player GameObject reference is missing.");
            }
        }

        public void Dispose()
        {
            _player = null;
            _normalPlayerView = null;
            _overworldPlayerView = null;
            Destroy(gameObject);
        }

        public void SwitchToOverworldView(Vector3 position)
        {
            // if (_isInOverworld) return;

            _normalPlayerView.gameObject.SetActive(false);

            if (_overworldPlayerView == null)
            {
                Debug.LogError("Overworld player view is not set. Please assign a reference in the inspector.");
                return;
            }
            
            _overworldPlayerView.gameObject.SetActive(true);
            _overworldPlayerView.transform.position = position;
            _overworldPlayerView.Initialize(_player);

            _isInOverworld = true;
        }

        public void SwitchToNormalView(Vector3 position)
        {
            // if (!_isInOverworld) return;
            if(_overworldPlayerView == null)
            {
                Debug.LogWarning("Overworld player view is null, cannot switch to normal view.");
                return;
            }

            _overworldPlayerView.gameObject.SetActive(false);
            _normalPlayerView.gameObject.SetActive(true);
            Debug.LogWarning("Current Pos:" + position);
            _normalPlayerView.transform.position = position;

            _isInOverworld = false;
        }

        public Vector3 GetCurrentPosition()
        {
            return _isInOverworld ? _overworldPlayerView.transform.position : _normalPlayerView.transform.position;
        }

        public bool IsOverworldScene(string sceneName)
        {
            return overworldSceneNames.Contains(sceneName);
        }
    }
}