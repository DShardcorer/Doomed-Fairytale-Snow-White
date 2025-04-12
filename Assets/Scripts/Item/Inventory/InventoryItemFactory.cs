namespace Item.Inventory
{
    public static class InventoryItemFactory
    {
        public static InventoryItem CreateItem(ItemData itemData)
        {
            if(itemData is ItemData_Equipment)
            {
                return new EquipmentInventoryItem(itemData as ItemData_Equipment);
            }
            else
            {
                return new InventoryItem(itemData);
            }
        }
    }
}
