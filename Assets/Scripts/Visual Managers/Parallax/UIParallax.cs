using Unity.Cinemachine;
using UnityEngine;

namespace Visual_Managers.Parallax
{
    public class UIParallax : MonoBehaviour
    {
        public CinemachineCamera virtualCamera;
        public float parallaxSpeed = 0.1f; // Adjust for effect intensity
        private Vector3 lastCameraPosition;
        private RectTransform rectTransform;

        private void Start()
        {

            if (virtualCamera != null)
                lastCameraPosition = virtualCamera.transform.position;

            rectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (virtualCamera == null) return;

            Vector3 deltaMovement = virtualCamera.transform.position - lastCameraPosition;
            rectTransform.anchoredPosition += new Vector2(deltaMovement.x, deltaMovement.y) * parallaxSpeed;

            lastCameraPosition = virtualCamera.transform.position;
        }
    }
}
