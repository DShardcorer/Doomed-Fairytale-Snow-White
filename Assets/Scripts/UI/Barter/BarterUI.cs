using DefaultNamespace.EventSystem.Barter;
using GeneralManagers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Barter
{
    public class BarterUI : MonoBehaviour, ILifecycle<UIManager>
    {
        [Header("NPC Components")] [SerializeField]
        private Image npcBartererImage;

        [Header("Barter UI Components")] [SerializeField]
        private BartererInventoryPageUI playerBartererInventoryPageUI;

        [SerializeField] private BartererInventoryPageUI npcBartererInventoryPageUI;
        [SerializeField] private BarteredItemsHolderUI playerBarteredItemsHolderUI;
        [SerializeField] private BarteredItemsHolderUI npcBarteredItemsHolderUI;
        [SerializeField] private StackSplitInputterUI stackSplitInputterUI;
        [SerializeField] private Button togglePlayerNpcInventoryButton;
        [SerializeField] private Button completeBarterButton;

        #region public getters

        public BartererInventoryPageUI PlayerBartererInventoryPageUI => playerBartererInventoryPageUI;
        public BartererInventoryPageUI NpcBartererInventoryPageUI => npcBartererInventoryPageUI;
        public BarteredItemsHolderUI PlayerBarteredItemsHolderUI => playerBarteredItemsHolderUI;
        public BarteredItemsHolderUI NpcBarteredItemsHolderUI => npcBarteredItemsHolderUI;
        public StackSplitInputterUI StackSplitInputterUI => stackSplitInputterUI;

        #endregion


        public void Initialize(UIManager parent)
        {
            //Initialize the barterer inventory pages
            playerBartererInventoryPageUI.Initialize(this, BartererType.Player);
            npcBartererInventoryPageUI.Initialize(this, BartererType.NPC);

            playerBarteredItemsHolderUI.Initialize(this, BartererType.Player);
            npcBarteredItemsHolderUI.Initialize(this, BartererType.NPC);


            togglePlayerNpcInventoryButton.onClick.AddListener(ToggleBartererInventory);
            completeBarterButton.onClick.AddListener(CompleteBarter);
            BarterEventSystem.OnBarterStart += OnBarterStart;

            //Disable all UI elements initially
            playerBartererInventoryPageUI.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void CompleteBarter()
        {
            BarterEventSystem.InvokeBarterComplete();
            gameObject.SetActive(false);
        }

        private void OnBarterStart(BarterEventSystem.BarterStartEventArgs obj)
        {
            //Enable the barter UI
            gameObject.SetActive(true);
            //Update the NPC image
            // npcBartererImage.sprite = obj.NpcImage;
            //Update the barterer inventories
            playerBartererInventoryPageUI.UpdateUI(GameManager.Instance.BarterManager.PlayerInventory.ItemList);
            npcBartererInventoryPageUI.UpdateUI(GameManager.Instance.BarterManager.NpcInventory.ItemList);
            //Update the bartered items holders
            playerBarteredItemsHolderUI.UpdateUI(GameManager.Instance.BarterManager.PlayerBarteredItemsHolder
                .BarteredItems);
            npcBarteredItemsHolderUI.UpdateUI(GameManager.Instance.BarterManager.NpcBarteredItemsHolder.BarteredItems);
            GameManager.Instance.BarterManager.PlayerInventory.OnItemListChangedAction +=
                playerBartererInventoryPageUI.UpdateUI;
            GameManager.Instance.BarterManager.NpcInventory.OnItemListChangedAction +=
                npcBartererInventoryPageUI.UpdateUI;
            GameManager.Instance.BarterManager.PlayerBarteredItemsHolder.OnItemsListChanged +=
                playerBarteredItemsHolderUI.UpdateUI;
            GameManager.Instance.BarterManager.NpcBarteredItemsHolder.OnItemsListChanged +=
                npcBarteredItemsHolderUI.UpdateUI;
        }

        private void ToggleBartererInventory()
        {
            //Toggle the visibility of the barterer inventory pages
            playerBartererInventoryPageUI.gameObject.SetActive(!playerBartererInventoryPageUI.gameObject.activeSelf);
            npcBartererInventoryPageUI.gameObject.SetActive(!npcBartererInventoryPageUI.gameObject.activeSelf);
        }

        public void Dispose()
        {
            //Unhook from inventory events
            GameManager.Instance.BarterManager.PlayerInventory.OnItemListChangedAction -=
                playerBartererInventoryPageUI.UpdateUI;
            GameManager.Instance.BarterManager.NpcInventory.OnItemListChangedAction -=
                npcBartererInventoryPageUI.UpdateUI;
            GameManager.Instance.BarterManager.PlayerBarteredItemsHolder.OnItemsListChanged -=
                playerBarteredItemsHolderUI.UpdateUI;
            GameManager.Instance.BarterManager.NpcBarteredItemsHolder.OnItemsListChanged -=
                npcBarteredItemsHolderUI.UpdateUI;

            togglePlayerNpcInventoryButton.onClick.RemoveListener(ToggleBartererInventory);
            BarterEventSystem.OnBarterStart -= OnBarterStart;

            //Clean up references
            playerBartererInventoryPageUI = null;
            npcBartererInventoryPageUI = null;
            playerBarteredItemsHolderUI = null;
            npcBarteredItemsHolderUI = null;
            stackSplitInputterUI = null;
        }
    }
}