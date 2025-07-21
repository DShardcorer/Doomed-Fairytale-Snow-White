using GeneralManagers;
using Item.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Barter
{
    public class StackSplitInputterUI : MonoBehaviour
    {
        [SerializeField] private GameObject splitterPanel;
        [SerializeField] private TMP_InputField quantityInput;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI maxQuantityText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button increaseButton;
        [SerializeField] private Button decreaseButton;
        
        private InventoryItem _currentItem;
        private BartererType _bartererType;
        private int _currentQuantity = 1;
        private int _maxQuantity = 1;
        
        
        private void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmClick);
            cancelButton.onClick.AddListener(OnCancelClick);
            increaseButton.onClick.AddListener(OnIncreaseClick);
            decreaseButton.onClick.AddListener(OnDecreaseClick);
            quantityInput.onValueChanged.AddListener(OnQuantityChanged);
            
            // Hide panel initially
            splitterPanel.SetActive(false);
        }
        
        public void Show(InventoryItem item, BartererType bartererType)
        {
            _currentItem = item;
            _bartererType = bartererType;
            
            _maxQuantity = item.stackSize;
            _currentQuantity = 1;
            
            itemNameText.text = item.itemDataSo.itemName;
            maxQuantityText.text = $"/ {_maxQuantity}";
            quantityInput.text = _currentQuantity.ToString();
            
            splitterPanel.SetActive(true);
        }
        
        private void OnQuantityChanged(string value)
        {
            if (int.TryParse(value, out int newQuantity))
            {
                _currentQuantity = Mathf.Clamp(newQuantity, 1, _maxQuantity);
                quantityInput.text = _currentQuantity.ToString();
            }
            else
            {
                quantityInput.text = _currentQuantity.ToString();
            }
        }
        
        private void OnIncreaseClick()
        {
            _currentQuantity = Mathf.Min(_currentQuantity + 1, _maxQuantity);
            quantityInput.text = _currentQuantity.ToString();
        }
        
        private void OnDecreaseClick()
        {
            _currentQuantity = Mathf.Max(_currentQuantity - 1, 1);
            quantityInput.text = _currentQuantity.ToString();
        }
        
        private void OnConfirmClick()
        {
            InventoryItem splitItem;
            
            // If entire stack is selected, use original item
            if (_currentQuantity >= _maxQuantity)
            {
                splitItem = _currentItem;
            }
            else
            {
                // Create a new item with the split quantity
                splitItem = new InventoryItem(_currentItem.itemDataSo, _currentQuantity);
            }
            
            // Add the split item to barter based on barterer type
            if (_bartererType == BartererType.Player)
                GameManager.Instance.BarterManager.AddPlayerBarteredItem(splitItem);
            else
                GameManager.Instance.BarterManager.AddNpcBarteredItem(splitItem);
            
            Hide();
        }
        
        private void OnCancelClick()
        {
            Hide();
        }
        
        private void Hide()
        {
            splitterPanel.SetActive(false);
            _currentItem = null;
        }
    }
}