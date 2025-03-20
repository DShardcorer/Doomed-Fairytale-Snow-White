
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _stackSizeText;


    public InventoryItem inventoryItem;

    public void UpdateUI(InventoryItem item)
    {
        //print the item name
        Debug.Log("Updating UI for item: " + item.ItemData.itemName);
        inventoryItem = item;
        _icon.sprite = item.ItemData.icon;
        if(inventoryItem.stackSize > 1)
        {
            _stackSizeText.text = inventoryItem.stackSize.ToString();
        }
        else
        {
            _stackSizeText.text = "";
        }
    }


}
