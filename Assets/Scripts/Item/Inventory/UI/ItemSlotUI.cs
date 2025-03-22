
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _stackSizeText;
    private float _lastClickTime;
    private const float DoubleClickThreshold = 0.3f; // Time in seconds


    public InventoryItem inventoryItem;

    public void UpdateUI(InventoryItem item)
    {
        //print the item name
        Debug.Log("Updating UI for item: " + item.ItemData.itemName);
        inventoryItem = item;
        _icon.sprite = item.ItemData.icon;
        if (inventoryItem.stackSize > 1)
        {
            _stackSizeText.text = inventoryItem.stackSize.ToString();
        }
        else
        {
            _stackSizeText.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - _lastClickTime < DoubleClickThreshold)
        {
            OnDoubleClick();
        }
        _lastClickTime = Time.time;
    }

    private void OnDoubleClick()
    {
        if(inventoryItem.ItemData.itemType == ItemType.Equipment){
            Debug.Log("Equipping item: " + inventoryItem.ItemData.itemName);
        }
    }



}
