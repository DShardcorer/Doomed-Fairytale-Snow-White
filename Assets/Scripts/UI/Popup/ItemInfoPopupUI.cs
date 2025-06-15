using Item.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Popup
{
    public class ItemInfoPopupUI: MonoBehaviour
    {
        public InventoryItem Item;
        
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI itemTypeText;
        [SerializeField] private TextMeshProUGUI itemValueText;
        [SerializeField] private TextMeshProUGUI itemWeightText;
        [SerializeField] private TextMeshProUGUI itemStackSizeText;
        [SerializeField] private TextMeshProUGUI itemStackValueText;
        [SerializeField] private TextMeshProUGUI itemStackWeightText;

        
        public void Setup(InventoryItem item)
        {
            Item = item;
            itemIcon.sprite = item.itemDataSo.icon;
            itemNameText.text = item.itemDataSo.itemName;
            itemDescriptionText.text = item.itemDataSo.description;
            itemTypeText.text = item.itemDataSo.itemType.ToString();
            itemValueText.text = $"Value: {item.itemDataSo.value}";
            itemWeightText.text = $"Weight: {item.itemDataSo.weight}";
            itemStackSizeText.text = $"Stack: {item.stackSize}";
            itemStackValueText.text = $"T.Value: {item.stackSize * item.itemDataSo.value}";
            itemStackWeightText.text = $"T.Weight: {item.stackSize * item.itemDataSo.weight}";
        }
    }
}