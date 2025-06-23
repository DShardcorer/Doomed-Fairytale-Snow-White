using EntityBase.NPC.AI;
using Helpers;

namespace EntityBase.NPC.State.BeingInteractedWith
{
    public class NPCBeingInteractedWithState : NPCState
    {
        public NPCBeingInteractedWithState(NPCAIConfiguration npcaiConfiguration) :
            base(HelperAnimationStateName.IS_IDLING, new NPCBeingInteractedWithProperties())
        {
        }

        private NPCBeingInteractedWithState(string animationBoolName, EntityStateProperties entityStateProperties) :
            base(animationBoolName, entityStateProperties)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
        }
        
    }
}