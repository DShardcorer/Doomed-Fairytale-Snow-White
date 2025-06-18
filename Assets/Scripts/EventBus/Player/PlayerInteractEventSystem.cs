using System;
using InteractInterface;

namespace EventBus.Player
{
    public static class PlayerInteractEventSystem
    {
        public class EnterInteractionEventArgs : EventArgs
        {
            public IInteractable Interactable { get; private set; }
            public global::Entity.Player.Player Player { get; private set; }

            public EnterInteractionEventArgs(IInteractable interactable, global::Entity.Player.Player player)
            {
                Interactable = interactable;
                Player = player;
            }
        }

        public static Action<EnterInteractionEventArgs> EnterInteractionEvent;
        public static void InvokeEnterInteraction(EnterInteractionEventArgs args)
        {
            EnterInteractionEvent?.Invoke(args);
        }

        public class ExitInteractionEventArgs : EventArgs
        {
            public IInteractable Interactable { get; private set; }
            public global::Entity.Player.Player Player { get; private set; }

            public ExitInteractionEventArgs(IInteractable interactable, global::Entity.Player.Player player)
            {
                Interactable = interactable;
                Player = player;
            }
        }

        public static Action<ExitInteractionEventArgs> ExitInteractionEvent;
        public static void InvokeExitInteraction(ExitInteractionEventArgs args)
        {
            ExitInteractionEvent?.Invoke(args);
        }

    }
}
