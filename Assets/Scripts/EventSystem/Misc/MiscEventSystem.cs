using System;
using UnityEngine;
namespace Events.Misc
{
    public static class MiscEventSystem
    {
        public static Action CoinCollected;

        public static void InvokeCoinCollected()
        {
            CoinCollected?.Invoke();
        }
    }
}
