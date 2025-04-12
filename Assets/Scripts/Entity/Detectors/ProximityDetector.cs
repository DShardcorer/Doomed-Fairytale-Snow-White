using Helpers;
using UnityEngine;

namespace Entity.Detectors
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

        public event System.EventHandler<Entity> OnEntityFromDifferentFactionSpottedInProximity;

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & HelperLayer.EntityLayerMask) == 0) return;
            //Check if its an entity
            if (collision.TryGetComponent(out EntityView entityView))
            {
                //Check if the entity in hostile to factions
                if (_parent.NPCProperties.HostileToFactions.Contains(entityView.Controller.Properties.EntityFaction))
                {
                    //Invoke the event
                    OnEntityFromDifferentFactionSpottedInProximity?.Invoke(this, entityView.Controller);
                    Debug.Log("Entity Spotted in Proximity");
                }
            }
        }
    }
}
