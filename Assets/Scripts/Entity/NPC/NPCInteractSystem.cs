using System;
using UnityEngine;

public class NPCInteractSystem : MonoBehaviour, ILifecycle<NPC>, IInteractable
{
    private NPC npc;
    public NPC NPC => npc;

    public int Priority => 1;

    public void Initialize(NPC npc)
    {
        this.npc = npc;
    }
    [SerializeField] private TextAsset inkDialogue;
    [SerializeField] private String knotName = "RandomMan";
    public void Interact(Player player)
    {
        if (npc.NPCProperties.HostileToFactions.Contains(EntityFaction.Player))
        {
            return;
        }
        
        DialogueEventSystem.InvokeEnterDialogue(new DialogueEventSystem.EnterDialogueEventArgs(inkDialogue, knotName));
    }

    public void Dispose()
    {
        npc = null;
    }

}
