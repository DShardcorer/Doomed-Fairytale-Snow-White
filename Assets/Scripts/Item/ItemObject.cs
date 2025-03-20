using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerView>().Player.Inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
