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
            _npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
            _npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
        }
        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
        }


    }
}
