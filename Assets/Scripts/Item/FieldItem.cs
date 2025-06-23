using EntityBase.Player;
using GeneralManagers;
using Helpers;
using InteractInterface;
using Item.Inventory;
using QuestSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item
{
    public class FieldItem : MonoBehaviour, IInteractable
    {
        private SpriteRenderer spriteRenderer;

        [FormerlySerializedAs("itemData")] [SerializeField]
        protected ItemDataSO itemDataSo;

        public ItemDataSO ItemDataSo { get => itemDataSo; }
        [SerializeField] protected int stackSize = 1;
        public int StackSize => stackSize;
        public int Priority => 99;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemDataSo.icon;
        }

        
        public void Setup(ItemDataSO itemDataSo, Vector3 position, int stackSize = 1)
        {
            this.itemDataSo = itemDataSo;
            this.stackSize = stackSize;
            spriteRenderer.sprite = itemDataSo.icon;
            transform.position = position;
        }

        public void Setup(InventoryItem inventoryItem, Vector3 position)
        {
            transform.position = position;
            Setup(inventoryItem.itemDataSo, position, inventoryItem.stackSize);
        }


        public virtual void Interact(Player player)
        {
            player.InventorySystem.AddItem(itemDataSo, StackSize);
            GameManager.Instance.PoolManager.ReturnObject(HelperPoolKey.FieldItem, this.gameObject);
        }
    }
}
