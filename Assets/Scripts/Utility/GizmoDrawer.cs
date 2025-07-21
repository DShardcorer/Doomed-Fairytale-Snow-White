using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class GizmoDrawer: MonoBehaviour
    {
        private List<IDrawGizmo> _drawGizmoObjects;
        public static GizmoDrawer Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _drawGizmoObjects = new List<IDrawGizmo>();
            }
            else
            {
                Debug.LogWarning("Multiple instances of GizmoDrawer detected. Destroying the new instance.");
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
            _drawGizmoObjects.Clear();
        }


        private void OnDrawGizmosSelected()
        {
            if (_drawGizmoObjects == null)
                return;
            if(_drawGizmoObjects.Count == 0)
                return;
            foreach (var drawGizmo in _drawGizmoObjects)
            {
                drawGizmo.DrawGizmoSelected();
            }
        }
        private void OnDrawGizmos()
        {
            if (_drawGizmoObjects == null)
                return;
            if(_drawGizmoObjects.Count == 0)
                return;
            foreach (var drawGizmo in _drawGizmoObjects)
            {
                drawGizmo.DrawGizmo();
            }
        }

        public void AddDrawGizmoObject(IDrawGizmo drawGizmo)
        {
            _drawGizmoObjects.Add(drawGizmo);
        }
        public void RemoveDrawGizmoObject(IDrawGizmo drawGizmo)
        {
            _drawGizmoObjects.Remove(drawGizmo);
        }
    }
}