using System;
using System.Collections;
using UnityEngine;

public class FOVDetector : MonoBehaviour
{
    private NPC _parent;
    
    [SerializeField] private PolygonCollider2D _collider;
    public PolygonCollider2D Collider2D => _collider;

    // Event to send the closest detected entity every cycle.
    public event EventHandler<Entity> OnClosestEntityFromDifferentFactionSpottedInFOV;

    public void Initialize(NPC npc)
    {
        _parent = npc;
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
        filter.SetLayerMask(LayerHelper.EntityLayerMask);
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
                // Filter out entities of the same faction.
                if (entityView.Controller.Properties.entityFaction != _parent.Properties.entityFaction)
                {
                    float distance = Vector2.Distance(_parent.View.transform.position, entityView.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestEntity = entityView.Controller;
                    }
                }
            }
        }

        if (closestEntity != null)
        {
            Debug.Log("Closest entity detected: " + closestEntity.View.name);
            OnClosestEntityFromDifferentFactionSpottedInFOV?.Invoke(this, closestEntity);
        }
    }

    /// <summary>
    /// Optional method to adjust the FOV collider rotation.
    /// </summary>
    public void SetColliderRotation(float angle)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
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
