using System.Collections;
using DefaultNamespace.Utility;
using DefaultNamespace.Utitlity.Camera;
using SceneSwitch;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeneralManagers
{
    public class CameraManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _gameManager;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private CinemachinePositionComposer cinemachinePositionComposer;
        [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
        private Vector3 originalDamping;
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            FollowPlayer();
            originalDamping = cinemachinePositionComposer.Damping;
            SetupConfiner();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            // Make sure we're following the correct target
            FollowPlayer();

            // Set damping to 0 when a new scene is loaded
            cinemachineCamera.CancelDamping(updateNow:true);
            cinemachinePositionComposer.Damping = Vector3.zero;
            StartCoroutine(ResetDamping(1));
            SetupConfiner();
        }

        private void SetupConfiner()
        {
            PolygonCollider2D confinerCollider = ServiceLocator.GetService<CameraConfiner>().ConfineArea;
            if (confinerCollider != null)
            {
                cinemachineConfiner2D.BoundingShape2D = confinerCollider;
            }
            else
            {
                Debug.LogWarning("No confiner collider found. Camera confiner will not be set.");
            }
        }

        private void FollowPlayer()
        {
            // cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
            SetFollowTarget(GameManager.Instance.PlayerManager.Player.View.transform);
        }
        private IEnumerator ResetDamping(float time)
        {
            yield return new WaitForSeconds(time);
            cinemachinePositionComposer.Damping = originalDamping;
        }
        

        public void Dispose()
        {
            _gameManager = null;
            cinemachineCamera = null;
            cinemachinePositionComposer = null;
            cinemachineConfiner2D = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }


        /// <summary>
        /// Sets the camera to follow the given target.
        /// </summary>
        private void SetFollowTarget(Transform newTarget)
        {
            cinemachineCamera.Follow = newTarget;
        }
    }
}