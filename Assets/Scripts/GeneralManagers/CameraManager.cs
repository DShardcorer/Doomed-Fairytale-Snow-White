using Unity.Cinemachine;
using UnityEngine;

namespace GeneralManagers
{
    public class CameraManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _gameManager;
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Dispose()
        {
            _gameManager = null;
            Destroy(gameObject);
        }

        [SerializeField] private CinemachineCamera cinemachineCamera;


        /// <summary>
        /// Sets the camera to follow the given target.
        /// </summary>
        public void SetFollowTarget(Transform newTarget)
        {
            if (cinemachineCamera != null && newTarget != null)
            {
                cinemachineCamera.Follow = newTarget;
            }
        }
    }
}
