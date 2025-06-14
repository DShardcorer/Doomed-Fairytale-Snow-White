using System;

namespace EventSystem.Player
{
    public static class PlayerProfileEventSystem
    {
        public class PlayerNameChangedEventArgs : EventArgs
        {
            public string NewName;

            public PlayerNameChangedEventArgs(string newName)
            {
                NewName = newName;
            }
        }
        public static Action<PlayerNameChangedEventArgs> OnPlayerNameChanged;
        
        public static void InvokePlayerNameChanged(PlayerNameChangedEventArgs eventArgs)
        {
            OnPlayerNameChanged?.Invoke(eventArgs);
        }
    }
}