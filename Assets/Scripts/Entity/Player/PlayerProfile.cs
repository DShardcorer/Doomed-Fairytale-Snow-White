using EventSystem.Player;

namespace Entity.Player
{
    public class PlayerProfile:EntityProfile
    {
        public override void SetName(string name)
        {
            base.SetName(name);
            PlayerProfileEventSystem.InvokePlayerNameChanged(name);
        }
    }
}