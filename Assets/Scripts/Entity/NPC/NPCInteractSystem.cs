using System;
using Entity.Faction;
using EventSystem.Dialogue;
using EventSystem.Player;
using GeneralManagers;
using InteractInterface;
using UnityEngine;

namespace Entity.NPC
{
    public class NPCInteractSystem : MonoBehaviour, ILifecycle<NPC>, IInteractable
    {
        private NPC npc;
        public NPC NPC => npc;

        public int Priority => 1;
        [SerializeField] private TextAsset inkDialogue;
        [SerializeField] private String knotName = "RandomMan";

        public void Initialize(NPC npc)
        {
            this.npc = npc;
            PlayerInteractEventSystem.EnterInteractionEvent += OnEnterInteraction;
            PlayerInteractEventSystem.ExitInteractionEvent += OnExitInteraction;
        }

        private void OnEnterInteraction(PlayerInteractEventSystem.EnterInteractionEventArgs args)
        {
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
            if (args.Interactable != this)
            {
                return;
            }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
            npc.Properties.lastMovementVector = (args.Player.View.transform.position - npc.View.transform.position).normalized;
            Debug.Log("Enter interaction with " + npc);
            npc.StateMachine.ChangeState(npc.NPCBeingInteractedWithState);

        }
        private void OnExitInteraction(PlayerInteractEventSystem.ExitInteractionEventArgs args)
        {
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
            if (args.Interactable != this)
            {
                return;
            }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
            npc.StateMachine.ChangeState(npc.NPCIdlingState);
        }


        public void Interact(Player.Player player)
        {
            if (npc.NPCProperties.HostileToFactions.Contains(EntityFaction.Player))
            {
                return;
            }
            //Set lastMovementvector to player direction
            DialogueEventSystem.InvokeEnterDialogue(new DialogueEventSystem.EnterDialogueEventArgs(inkDialogue, knotName));
        }

        public void Dispose()
        {
            npc = null;
            PlayerInteractEventSystem.EnterInteractionEvent -= OnEnterInteraction;
            PlayerInteractEventSystem.ExitInteractionEvent -= OnExitInteraction;
        }

    }
}
