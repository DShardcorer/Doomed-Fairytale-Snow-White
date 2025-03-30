using Events.Misc;
using UnityEngine;

public class ItemCollectCoinsQuestEventTrigger : ItemQuestEventTrigger
{
    public override void TriggerEvent()
    {
        MiscEventSystem.InvokeCoinCollected();
    }
}
