namespace Item.Inventory
{
    public class EquipmentInventoryItem : InventoryItem
    {
        private ItemDataSOEquipment _soEquipmentDataSo;
        public ItemDataSOEquipment SoEquipmentDataSo => _soEquipmentDataSo;
        public bool isEquipped = false;
        public EquipmentInventoryItem(ItemDataSOEquipment itemDataSo, int quantity) : base(itemDataSo, quantity)
        {
            _soEquipmentDataSo = itemDataSo;
        }
        
    }
}
