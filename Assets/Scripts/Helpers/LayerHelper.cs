

using UnityEngine;

public static class LayerHelper
{
    public static LayerMask InteractableLayerMask => LayerMask.GetMask("Interactable");

    public static LayerMask PlayerLayerMask => LayerMask.GetMask("Player");

    public static LayerMask NonHostileLayerMask => LayerMask.GetMask("NonHostile");

    public static LayerMask HostileLayerMask => LayerMask.GetMask("Hostile");
}
