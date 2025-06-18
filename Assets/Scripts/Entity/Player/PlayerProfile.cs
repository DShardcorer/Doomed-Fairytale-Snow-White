using EventBus.Player;

namespace Entity.Player
{
    public class PlayerProfile : EntityProfile
    {
        public PlayerProfile(string name, string description) : base(name, description)
        {
        }

        public override void SetName(string name)
        {
            base.SetName(name);
            PlayerProfileEventSystem.InvokePlayerNameChanged(
                new PlayerProfileEventSystem.PlayerNameChangedEventArgs(name));
        }
    }
}