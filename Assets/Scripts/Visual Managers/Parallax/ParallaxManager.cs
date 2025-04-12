using UnityEngine;

namespace Visual_Managers.Parallax
{
    public class ParallaxManager : MonoBehaviour
    {
        public Transform cameraTransform;
        private Vector3 lastCameraPosition;
        public ParallaxLayer[] parallaxLayers;

        private void Start()
        {
            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;

            lastCameraPosition = cameraTransform.position;
        }

        private void FixedUpdate() {
            Vector2 deltaMovement = (Vector2)cameraTransform.position - (Vector2)lastCameraPosition;

            foreach (var layer in parallaxLayers)
            {
                layer.Move(deltaMovement);
            }

            lastCameraPosition = cameraTransform.position;
        }
    }
}
