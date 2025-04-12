using Entity.Player;
using InteractInterface;
using QuestSystem;
using UnityEngine;

namespace Item
{
    public class FieldItem : MonoBehaviour, IInteractable
    {
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private ItemData itemData;

        public ItemData ItemData { get => itemData; }

        public int Priority => 99;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemData.icon;
        }


        public void SetItemData(ItemData itemData)
        {
            this.itemData = itemData;
            spriteRenderer.sprite = itemData.icon;
        }


        public void Interact(Player player)
        {
            player.InventorySystem.AddItem(itemData);
            ItemQuestEventTrigger itemQuestEventTrigger = GetComponent<ItemQuestEventTrigger>();
            if (itemQuestEventTrigger != null)
            {
                itemQuestEventTrigger.TriggerEvent();
            }
            Destroy(gameObject);
        }
    }
}
