using System;
using System.Collections;
using EntityBase.Faction;
using Helpers;
using UnityEngine;

namespace EntityBase.Detectors
{
    public class FOVDetector : MonoBehaviour
    {
        private NPC.NPC npc;
    
        [SerializeField] private PolygonCollider2D _collider;
        public PolygonCollider2D Collider2D => _collider;

        // Event to send the closest detected entity every cycle.
        public event EventHandler<Entity> OnClosestEntityFromEnemyFactionSpottedInFOV;

        public void Initialize(NPC.NPC npc)
        {
            this.npc = npc;
            StartCoroutine(DetectionRoutine());
        }



        private IEnumerator DetectionRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(2f);
            while (true)
            {
                DetectEntities();
                yield return wait;
            }
        }

        private void DetectEntities()
        {
            // Setup a filter to only check for colliders on the desired layer.
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(HelperLayer.EntityLayerMask);
            filter.useTriggers = true; // Make sure triggers are included if needed.

            // Prepare an array to receive the overlapping colliders.
            Collider2D[] results = new Collider2D[20];
            int count = _collider.Overlap(filter, results);

            Entity closestEntity = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider2D col = results[i];
                if (col.TryGetComponent<EntityView>(out EntityView entityView))
                {
                    EntityFaction entityFaction = entityView.Parent.Properties.EntityFaction;
                    // Filter out non-hostile entities factions
                    if (FactionRegistry.AreEnemies(npc.Properties.EntityFaction, entityFaction))
                    {
                        float distance = Vector2.Distance(npc.View.transform.position, entityView.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestEntity = entityView.Parent;
                        }
                    }
                }
            }

            if (closestEntity != null)
            {
                OnClosestEntityFromEnemyFactionSpottedInFOV?.Invoke(this, closestEntity);
            }
        }

        /// <summary>
        /// Optional method to adjust the FOV collider rotation.
        /// </summary>
        public void SetColliderRotation(Vector2 direction)
        {
            transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, direction));
        }

        private void OnDrawGizmosSelected()
        {
            if (_collider == null || _collider.points.Length == 0)
                return;

            Gizmos.color = Color.green;
            Vector2[] points = _collider.points;
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 globalPointA = _collider.transform.TransformPoint(points[i]);
                Vector2 globalPointB = _collider.transform.TransformPoint(points[(i + 1) % points.Length]);
                Gizmos.DrawLine(globalPointA, globalPointB);
            }
        }
    }
}
