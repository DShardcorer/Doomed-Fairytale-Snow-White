using System.Collections;
using System.Collections.Generic;
using GeneralManagers;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Utitlity.Camera
{
    using UnityEngine;

    [DefaultExecutionOrder(999)]
    [RequireComponent(typeof(Canvas))]
    public class CameraFinder : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        public CameraType cameraType;

        public enum CameraType
        {
            MainCamera,
            UIMainCamera
        }

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            //Start a coroutine that runs after 0.5s;
            StartCoroutine(FindCamera());
        }

        private IEnumerator FindCamera()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            switch (cameraType)
            {
                case CameraType.MainCamera:
                    canvas.worldCamera = GameManager.Instance.MainCamera;
                    break;
                case CameraType.UIMainCamera:
                    canvas.worldCamera = GameManager.Instance.UIMainCamera;
                    break;
            }
        }
    }
}