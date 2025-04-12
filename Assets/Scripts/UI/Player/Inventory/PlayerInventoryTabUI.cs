using GeneralManagers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Player.Inventory
{
    public class PlayerInventoryTabUI : MonoBehaviour, IPointerDownHandler, ILifecycle<PlayerInventoryUI>
    {
        private PlayerInventoryUI _playerInventoryUI;
        public PlayerInventoryUI PlayerInventoryUI => _playerInventoryUI;
        public PlayerInventoryType playerInventoryType;
        public void Initialize(PlayerInventoryUI parent)
        {
            _playerInventoryUI = parent;
        }

        public void Dispose()
        {
            _playerInventoryUI = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _playerInventoryUI.SwitchToInventoryType(playerInventoryType);
        }

        public void SelectTab()
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }

        public void DeselectTab()
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
    }
}

