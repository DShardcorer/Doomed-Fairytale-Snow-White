using System;

namespace EventBus.Misc
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
