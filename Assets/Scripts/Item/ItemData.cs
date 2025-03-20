using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "ItemData")]
public class ItemData: ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public float weight =1;
}
