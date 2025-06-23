using EntityBase.Faction;
using Helpers;
using UnityEngine;

namespace EntityBase.Detectors
{
    public class ProximityDetector : MonoBehaviour
    {
        private NPC.NPC _parent;
        public void Initialize(NPC.NPC npc)
        {
            _parent = npc;
        }

        [SerializeField] private CircleCollider2D _collider;

        public CircleCollider2D Collider2D => _collider;

        public event System.EventHandler<Entity> OnEntityFromEnemyFactionSpottedInProximity;

        public void SetDetectionRadius(float detectionRadius)
        {
            _collider.radius = detectionRadius;
        }

        public virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & HelperLayer.EntityLayerMask) == 0) return;
            //Check if its an entity
            if (collision.TryGetComponent(out EntityView entityView))
            {
                EntityFaction entityFaction = entityView.Parent.Properties.EntityFaction;
                //Check if the entity in hostile to factions
                if (FactionRegistry.AreEnemies(_parent.Properties.EntityFaction, entityFaction))
                {
                    //Invoke the event
                    OnEntityFromEnemyFactionSpottedInProximity?.Invoke(this, entityView.Parent);
                    Debug.Log("Entity Spotted in Proximity");
                }
            }
        }
    }
}
