namespace Item.Inventory
{
    public static class InventoryItemFactory
    {
        public static InventoryItem CreateItem(ItemDataSO itemDataSo)
        {
            if(itemDataSo is ItemDataSOEquipment)
            {
                return new EquipmentInventoryItem(itemDataSo as ItemDataSOEquipment);
            }
            else
            {
                return new InventoryItem(itemDataSo);
            }
        }
    }
}
