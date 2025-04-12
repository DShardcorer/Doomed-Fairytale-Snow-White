

using UnityEngine;

namespace Helpers
{
    public static class HelperLayer
    {
        public static LayerMask InteractableLayerMask => LayerMask.GetMask("Interactable");

        public static LayerMask EntityLayerMask => LayerMask.GetMask("Entity");
    }
}
