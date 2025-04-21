namespace Entity.NPC.BeingInteractedWith
{
    public class NPCBeingInteractedWithState : NPCState
    {
        public NPCBeingInteractedWithState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
        }
        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
        }


    }
}
