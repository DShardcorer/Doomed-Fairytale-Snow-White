using UnityEngine;

public class FieldItem : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private ItemData itemData;

    public ItemData ItemData { get => itemData; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.icon;
    }


    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.icon;
    }


    public void Interact(Player player)
    {
        player.Inventory.AddItem(itemData);
        Destroy(gameObject);
    }
}
