using System.Collections.Generic;
using UnityEngine;
using Entity.Player.Overworld;
using GeneralManagers;
using UnityEngine.Tilemaps;

namespace Entity.Player
{
    public class PlayerViewManager : MonoBehaviour
    {
        [SerializeField] private GameObject overworldPlayerPrefab;
        private List<string> overworldSceneNames = new List<string> { "Scene_Overworld" };

        private Player _player;
        private PlayerView _normalPlayerView;
        private OverworldPlayerView _overworldPlayerView;
        private bool _isInOverworld = false;

        public void Initialize(GameManager parent)
        {
            _player = parent.PlayerManager.Player;
            _normalPlayerView = parent.PlayerManager.Player.PlayerView;
        }

        public void SwitchToOverworldView(Vector3 position)
        {
            if (_isInOverworld) return;

            _normalPlayerView.gameObject.SetActive(false);

            if (_overworldPlayerView == null)
            {
                GameObject overworldPlayerObj = Instantiate(overworldPlayerPrefab, position, Quaternion.identity);
                _overworldPlayerView = overworldPlayerObj.GetComponent<OverworldPlayerView>();
                _overworldPlayerView.Initialize(_player);
            }
            else
            {
                _overworldPlayerView.gameObject.SetActive(true);
                _overworldPlayerView.transform.position = position;
            }

            _isInOverworld = true;
        }

        public void SwitchToNormalView(Vector3 position)
        {
            if (!_isInOverworld) return;

            _overworldPlayerView.gameObject.SetActive(false);
            _normalPlayerView.gameObject.SetActive(true);
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