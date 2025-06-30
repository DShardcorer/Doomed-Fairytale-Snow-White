using EventBus.Player;

namespace EntityBase.Player
{
    public class PlayerProfile : EntityProfile
    {
        public PlayerProfile(string name, string description, BodyType bodyType) : base(name, description, bodyType)
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