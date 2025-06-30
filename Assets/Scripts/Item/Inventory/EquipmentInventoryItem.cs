namespace Item.Inventory
{
    public class EquipmentInventoryItem : InventoryItem
    {
        private ItemDataSOEquipment _equipmentDataSo;
        public ItemDataSOEquipment EquipmentDataSo => _equipmentDataSo;
        public bool isEquipped = false;
        public EquipmentInventoryItem(ItemDataSOEquipment itemDataSo, int quantity) : base(itemDataSo, quantity)
        {
            _equipmentDataSo = itemDataSo;
        }
        
    }
}
