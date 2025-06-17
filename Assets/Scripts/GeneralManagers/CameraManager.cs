using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeneralManagers
{
    public class CameraManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _gameManager;
        private CinemachineCamera cinemachineCamera;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            SceneManager.sceneLoaded += OnSceneLoaded;
            FollowPlayer();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            //find the cinemachine camera in the scene
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
            SetFollowTarget(GameManager.Instance.PlayerManager.Player.View.transform);
        }

        public void Dispose()
        {
            _gameManager = null;
            cinemachineCamera = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }


        /// <summary>
        /// Sets the camera to follow the given target.
        /// </summary>
        private void SetFollowTarget(Transform newTarget)
        {
            if (cinemachineCamera != null && newTarget != null)
            {
                cinemachineCamera.Follow = newTarget;
            }
        }
    }
}