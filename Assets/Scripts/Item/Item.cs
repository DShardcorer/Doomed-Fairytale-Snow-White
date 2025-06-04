using UnityEngine;
using UnityEngine.Serialization;

namespace Item
{
    public class Item : MonoBehaviour
    {
        [FormerlySerializedAs("itemData")] public ItemDataSO itemDataSo;
    }
}
