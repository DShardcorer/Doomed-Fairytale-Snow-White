using DefaultNamespace.Utility;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.LightingSystem
{
    [DefaultExecutionOrder(999)]
    // [ExecuteInEditMode]
    public class ShadowInstance : MonoBehaviour
    {
        [SerializeField] private DayCycleLightingManager _dayCycleLightingManager;
        [Range(0, 10f)] public float BaseLength = 1f;
        

        private async void OnEnable()
        {
            if (!_dayCycleLightingManager)
            {
                await ServiceLocator.InitializationTask;
                _dayCycleLightingManager = GameManager.Instance.DayCycleLightingManager;
            }
            _dayCycleLightingManager.RegisterShadow(this);
        }

        private void OnDisable()
        {
            _dayCycleLightingManager.UnregisterShadow(this);
        }
    }
}