using Entity.Player;
using InteractInterface;
using QuestSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item
{
    public class FieldItem : MonoBehaviour, IInteractable
    {
        private SpriteRenderer spriteRenderer;

        [FormerlySerializedAs("itemData")] [SerializeField]
        private ItemDataSO itemDataSo;

        public ItemDataSO ItemDataSo { get => itemDataSo; }

        public int Priority => 99;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemDataSo.icon;
        }


        public void SetItemData(ItemDataSO itemDataSo)
        {
            this.itemDataSo = itemDataSo;
            spriteRenderer.sprite = itemDataSo.icon;
        }


        public void Interact(Player player)
        {
            player.InventorySystem.AddItem(itemDataSo);
            ItemQuestEventTrigger itemQuestEventTrigger = GetComponent<ItemQuestEventTrigger>();
            if (itemQuestEventTrigger != null)
            {
                itemQuestEventTrigger.TriggerEvent();
            }
            Destroy(gameObject);
        }
    }
}
