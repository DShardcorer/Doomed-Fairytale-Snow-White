namespace Item.Inventory
{
    public class EquipmentInventoryItem : InventoryItem
    {
        private ItemData_Equipment _equipmentData;
        public ItemData_Equipment EquipmentData => _equipmentData;
        public bool isEquipped = false;
        public EquipmentInventoryItem(ItemData_Equipment itemData) : base(itemData)
        {
            _equipmentData = itemData;
        }
    }
}
