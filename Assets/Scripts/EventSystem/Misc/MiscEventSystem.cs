using System;

namespace EventSystem.Misc
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
