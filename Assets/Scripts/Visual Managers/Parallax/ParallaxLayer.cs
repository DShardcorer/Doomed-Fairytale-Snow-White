using UnityEngine;

namespace Visual_Managers.Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        public float parallaxFactor = 1f; // Adjust this per layer
        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.position;
        }

        public void Move(Vector2 deltaMovement)
        {
            Debug.Log(deltaMovement);
            transform.position = initialPosition + new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);
        }
    }
}
