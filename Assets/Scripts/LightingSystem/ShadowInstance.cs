using GeneralManagers;
using UnityEngine;

namespace DefaultNamespace.LightingSystem
{
    [DefaultExecutionOrder(999)]
    [ExecuteInEditMode]
    public class ShadowInstance : MonoBehaviour
    {
        [Range(0, 10f)] public float BaseLength = 1f;

        private void OnEnable()
        {
            GameManager.Instance.DayCycleLightingManager.RegisterShadow(this);
        }

        private void OnDisable()
        {
            GameManager.Instance.DayCycleLightingManager.UnregisterShadow(this);

        }
    }
}