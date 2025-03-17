using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    private NPC _parent;
    public void Initialize(NPC npc)
    {
        _parent = npc;
    }

    [SerializeField] private CircleCollider2D _collider;

    public CircleCollider2D Collider2D => _collider;

    public event System.EventHandler<Entity> OnEntityFromDifferentFactionSpottedInProximity;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerHelper.EntityLayerMask) == 0) return;
        //Check if its an entity
        if (collision.TryGetComponent(out EntityView entityView))
        {
            //Check if the entity is from a different faction
            if (entityView.Controller.Properties.entityFaction != _parent.Properties.entityFaction)
            {
                //Invoke the event
                OnEntityFromDifferentFactionSpottedInProximity?.Invoke(this, entityView.Controller);
                Debug.Log("Entity Spotted in Proximity");
            }
        }
    }
}
