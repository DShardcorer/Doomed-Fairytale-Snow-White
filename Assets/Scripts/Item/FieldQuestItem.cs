using Entity.Player;
using QuestSystem;

namespace Item
{
    public class FieldQuestItem : FieldItem
    {
        public override void Interact(Player player)
        {
            player.InventorySystem.AddItem(itemDataSo);
            TryGetComponent<ItemQuestEventTrigger>(out var itemQuestEventTrigger);
            if (itemQuestEventTrigger != null)
            {
                itemQuestEventTrigger.TriggerEvent();
            }
            Destroy(gameObject);
        }
    }
}