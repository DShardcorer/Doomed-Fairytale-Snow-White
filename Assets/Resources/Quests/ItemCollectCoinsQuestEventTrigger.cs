using EventSystem.Misc;
using QuestSystem;

namespace Resources.Quests
{
    public class ItemCollectCoinsQuestEventTrigger : ItemQuestEventTrigger
    {
        public override void TriggerEvent()
        {
            MiscEventSystem.InvokeCoinCollected();
        }
    }
}
