using System;
using DefaultNamespace.Utility;
using GeneralManagers;
using UnityEngine;

namespace DefaultNamespace.Utitlity.Camera
{
    public class CameraConfiner: MonoBehaviour
    {
        [SerializeField] PolygonCollider2D _confineArea;
        public PolygonCollider2D ConfineArea => _confineArea;

        private void Awake()
        {
            ServiceLocator.RegisterService(this);
        }
        

        private void OnDestroy()
        {
            ServiceLocator.UnregisterService(this);
        }
    }
}