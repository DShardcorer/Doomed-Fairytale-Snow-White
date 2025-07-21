using GeneralManagers;
using Helpers;
using Item;
using Item.Inventory;
using TMPro;
using UI.Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Player.Inventory
{
    public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _stackSizeText;
        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f; // Time in seconds
        

        public InventoryItem item;
        private GameObject itemInfoGameObject;
        private EntityBase.Player.Player _player => GameManager.Instance.PlayerManager.Player;

        public virtual void UpdateUI(InventoryItem item)
        {
            this.item = item;
            _icon.sprite = item.itemDataSo.icon;
            if (this.item.stackSize > 1)
            {
                _stackSizeText.text = this.item.stackSize.ToString();
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

        protected virtual void OnDoubleClick()
        {
            if (item.itemDataSo.itemType == ItemType.Equipment)
            {
                Debug.Log("Equipping item: " + item.itemDataSo.itemName);
            }

            if (item.itemDataSo.itemType == ItemType.Consumable)
            {
                ItemDataSOConsumable consumableData = item.itemDataSo as ItemDataSOConsumable;
                consumableData.UseItem(_player);
                _player.InventorySystem.RemoveItem(item, 1);
                DisableItemInfoPopup();
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            itemInfoGameObject = GameManager.Instance.PoolManager.GetObject(HelperUIName.ItemInfoPopupUI);
            if (itemInfoGameObject == null)
            {
                Debug.LogError("Failed to get ItemInfoPopupUI from pool.");
                return;
            }

            // Set parent but don't reset scale/position yet
            itemInfoGameObject.transform.SetParent(UIManager.Instance.PopupContainer.transform, false);
            ItemInfoPopupUI itemInfoUI = itemInfoGameObject.GetComponent<ItemInfoPopupUI>();
            itemInfoUI.Setup(item);

            // Get necessary components
            RectTransform popupRectTransform = itemInfoGameObject.GetComponent<RectTransform>();
            RectTransform slotRectTransform = GetComponent<RectTransform>();

            // Convert slot position to screen space
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, slotRectTransform.position);

            // Calculate offset in screen space
            float offsetX = 1.5f; // Right offset in pixels
            float offsetY = -1f;
            screenPosition.x += offsetX;
            screenPosition.y += offsetY;
            // Convert screen position to popup's local space in its new parent
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                UIManager.Instance.PopupContainer.GetComponent<RectTransform>(),
                screenPosition,
                null,
                out localPosition);

            // Set the popup position
            popupRectTransform.localPosition = localPosition;

            // Ensure the popup stays within screen bounds
            Canvas canvas = UIManager.Instance.PopupContainer.GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                Vector3[] corners = new Vector3[4];
                popupRectTransform.GetWorldCorners(corners);

                // Adjust position if the popup goes out of bounds
                if (corners[0].x < 0) // Left edge
                {
                    localPosition.x += Mathf.Abs(corners[0].x);
                }

                if (corners[2].x > canvasRect.rect.width) // Right edge
                {
                    localPosition.x -= corners[2].x - canvasRect.rect.width;
                }

                if (corners[0].y < 0) // Bottom edge
                {
                    localPosition.y += Mathf.Abs(corners[0].y);
                }

                if (corners[1].y > canvasRect.rect.height) // Top edge
                {
                    localPosition.y -= corners[1].y - canvasRect.rect.height;
                }

                // Apply adjusted position
                popupRectTransform.localPosition = localPosition;
            }

            // Ensure it's visible
            itemInfoGameObject.SetActive(true);

            // // Force refresh layout if needed
            // LayoutRebuilder.ForceRebuildLayoutImmediate(popupRectTransform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DisableItemInfoPopup();
        }

        protected void DisableItemInfoPopup()
        {
            if (itemInfoGameObject != null)
            {
                GameManager.Instance.PoolManager.ReturnObject(HelperUIName.ItemInfoPopupUI, itemInfoGameObject);
                itemInfoGameObject = null;
            }
        }
    }
}