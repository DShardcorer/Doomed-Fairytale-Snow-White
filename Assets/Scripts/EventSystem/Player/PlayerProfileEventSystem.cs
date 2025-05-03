using System;

namespace EventSystem.Player
{
    public class PlayerProfileEventSystem
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
        
        public static void InvokePlayerNameChanged(string newName)
        {
            OnPlayerNameChanged?.Invoke(new PlayerNameChangedEventArgs(newName));
        }
    }
}